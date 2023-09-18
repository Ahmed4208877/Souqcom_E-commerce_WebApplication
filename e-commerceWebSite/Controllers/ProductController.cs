using e_commerceWebSite.Models;
using e_commerceWebSite.Repository;
using e_commerceWebSite.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Security.Claims;

namespace e_commerceWebSite.Controllers
{
    [Authorize(Roles = "Seller")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IToastNotification _toastNotification;

        public ProductController(IProductService productService,ICategoryService categoryService, IToastNotification toastNotification)
        {
            _productService = productService;
            _categoryService = categoryService;
            _toastNotification = toastNotification;
        }

        public IActionResult ListProducts()
        {
            string SellerId = User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier).Value;
            List<TbProduct> productModel = _productService.GetAllParSeller(SellerId);
            return View(productModel);
        }
        [HttpGet]
        public IActionResult AddNewProduct()
        {
            ViewBag.Categories = _categoryService.GetAll();
            return PartialView("_AddAndEditProductPartial",new TbProduct());
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddNewProduct(TbProduct product, List<IFormFile> Files)
        {            
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var file in Files)
                    {
                        if (file.Length > 0)
                        {
                            string ImagePath = Guid.NewGuid().ToString() + ".jpg";
                            var filepaths = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images\Products", ImagePath);
                            using (var stream = System.IO.File.Create(filepaths))
                            {
                                await file.CopyToAsync(stream);
                            }
                            product.ImagePath = ImagePath;
                        }
                    }
                    _productService.Insert(product);
                    _toastNotification.AddSuccessToastMessage("New Product Added Successfully");
                    return RedirectToAction("ListProducts", "Product");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
               
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult EditProduct(int ProductId)
        {
            TbProduct Productmodel = _productService.GetById(ProductId);
            ViewBag.Categories = _categoryService.GetAll();
            return PartialView("_AddAndEditProductPartial",Productmodel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> EditProduct(TbProduct EditProduct, List<IFormFile> Files)
        {
            TbProduct CurrentProduct = _productService.GetById(EditProduct.Id);
            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var file in Files)
                    {
                        if (file.Length > 0)
                        {
                            string ImagePath = Guid.NewGuid().ToString() + ".jpg";
                            var filepaths = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images\Products", ImagePath);
                            using (var stream = System.IO.File.Create(filepaths))
                            {
                                await file.CopyToAsync(stream);
                            }
                            EditProduct.ImagePath = ImagePath;
                        }
                    }
                    CurrentProduct.Id = EditProduct.Id;
                    CurrentProduct.Name = EditProduct.Name;
                    CurrentProduct.Description = EditProduct.Description;
                    CurrentProduct.CategoryId = EditProduct.CategoryId;
                    CurrentProduct.SellerId = EditProduct.SellerId;
                    CurrentProduct.IsActive = EditProduct.IsActive;
                    CurrentProduct.Price = EditProduct.Price;
                    CurrentProduct.StockQuantity = EditProduct.StockQuantity;
                    CurrentProduct.ExipirationDate = EditProduct.ExipirationDate;
                    if (string.IsNullOrEmpty(EditProduct.ImagePath))
                    {
                        CurrentProduct.ImagePath = CurrentProduct.ImagePath;
                    }
                    else
                    {
                        CurrentProduct.ImagePath = EditProduct.ImagePath;
                    }
                    _productService.Update(CurrentProduct);
                    _toastNotification.AddSuccessToastMessage("Product Edited Successfully");
                    return RedirectToAction("ListProducts", "Product");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);

                }
            }
            return View(EditProduct);
        }

        public IActionResult DeleteProduct(int Id)
        {
            _productService.Delete(Id);
            return Json(true);
        }


        //Test Ajax With Product to Seller
        public IActionResult GetAllProductsPerSeller()
        {
            string SellerId = User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier).Value;
            List<TbProduct> products = _productService.GetAllParSeller(SellerId);
            List<VmProduct> ListProducts = new List<VmProduct>();
            foreach (var item in products)
            {
                ListProducts.Add(new VmProduct()
                {
                    Id = item.Id,
                    Name = item.Name,
                    CategoryId =item.CategoryId,
                    CategoryName =item.Category.Name,
                    Description =item.Description,
                    SellerId =item.SellerId,
                    Price =item.Price,
                    ImagePath =item.ImagePath,
                    StockQuantity =item.StockQuantity,
                    IsActive =item.IsActive

                });
            }
            return Json(ListProducts);
        }

    }
}
