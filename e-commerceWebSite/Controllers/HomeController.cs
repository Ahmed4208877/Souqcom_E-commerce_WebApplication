using e_commerceWebSite.Models;
using e_commerceWebSite.Repository;
using e_commerceWebSite.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace e_commerceWebSite.Controllers
{
    [Authorize(Roles = "User")]
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public HomeController(IProductService productService,ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            VmUserHome model = new VmUserHome();
            model.products = _productService.GetAll();
            model.categories = _categoryService.GetAll();
            return View(model);
        }

        public IActionResult ProductDetails(int Id)
        {
            VmProductDetails model = new VmProductDetails();
            TbProduct product = _productService.GetById(Id);
            model.Product = product;
            List<TbProduct> products = _productService.GetAllParCategory(product.CategoryId);
            model.RelatedProducts = products.Where(p =>p.Id != product.Id &&( p.Price == product.Price || p.Price == (product.Price + 1000) || p.Price == (product.Price - 1000))).ToList();

            return View(model);
        }
        public IActionResult GetAjaxProducts(int CategoryId)
        {
            List<VmProduct> vmProducts = _productService.GetAllVmProduct();
            if (CategoryId != 0)
            {
                vmProducts = vmProducts.Where(c => c.CategoryId == CategoryId).ToList();

            }
            return Json(vmProducts);
        }

    }
}