using e_commerceWebSite.Models;
using e_commerceWebSite.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace e_commerceWebSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly INotificationService _NotificationService;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(INotificationService NotificationService, ICartService cartService, IProductService productService, ICategoryService categoryService, UserManager<ApplicationUser> userManager)
        {
            _NotificationService = NotificationService;
            _cartService = cartService;
            _productService = productService;
            _categoryService = categoryService;
            _userManager = userManager;
        }
        public IActionResult Dashboard()
        {
            ViewBag.Reveneow = _cartService.GetAll().Sum(t => t.Total_Price);
            ViewBag.TotalProduct = _productService.GetAll().Count();

            return View();
        }
        public IActionResult ListNotifications()
        {
            List<TbNotification> notifications = _NotificationService.GetAll();
            
            return View(notifications);
        }
        public IActionResult DeleteMessage(int Id)
        {
            _NotificationService.Delete(Id);
            return RedirectToAction(nameof(ListNotifications));
        }
        public async Task<IActionResult> ChangeActivity(string UserId)
        {
            ApplicationUser User = await _userManager.FindByIdAsync(UserId);
            User.Id = UserId;
            User.UserName = User.UserName;
            User.Email = User.Email;
            User.PhoneNumber = User.PhoneNumber;
            User.ImagePath = User.ImagePath;
            User.IsActive = true;
            IdentityResult result = await _userManager.UpdateAsync(User);

            return RedirectToAction(nameof(ListNotifications));
        }
    }
}
