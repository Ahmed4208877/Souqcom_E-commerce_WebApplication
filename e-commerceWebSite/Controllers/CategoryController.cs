using e_commerceWebSite.Bl;
using e_commerceWebSite.Models;
using e_commerceWebSite.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_commerceWebSite.Controllers
{
    [Authorize(Roles = "Admin")]

    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult ListCategories()
        {
            List<TbCategory> categories = _categoryService.GetAll();
            return View(categories);
        }
        [HttpGet]
        public IActionResult AddNewCategory()
        {
            return PartialView("_AddAndEditCategoryPartial", new TbCategory());
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult AddNewCategory(TbCategory Newcategory)
        {
            Newcategory.InsertionDate = DateTime.Now;
            Newcategory.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    _categoryService.Insert(Newcategory);
                    return RedirectToAction("ListCategories", "Category");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }

            }
            return View(Newcategory);
        }
        [HttpGet]
        public IActionResult EditCategory(int CategoryId)
        {
            TbCategory Categorymodel = _categoryService.GetById(CategoryId);
            return PartialView("_AddAndEditCategoryPartial", Categorymodel);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult EditCategory(TbCategory EditCategory)
        {
            TbCategory CurrentProduct = _categoryService.GetById(EditCategory.Id);
            CurrentProduct.ModifiedDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                try
                {
                    CurrentProduct.Id = EditCategory.Id;
                    CurrentProduct.Name = EditCategory.Name;
                    CurrentProduct.InsertionDate = EditCategory.InsertionDate;
                    _categoryService.Update(CurrentProduct);
                    return RedirectToAction("ListCategories", "Category");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);

                }
            }
            return View(EditCategory);
        }

        public IActionResult DeleteCategory(int Id)
        {
            _categoryService.Delete(Id);
            return Json(true);
        }

    }
}
