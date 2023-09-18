using e_commerceWebSite.Models;
using e_commerceWebSite.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace e_commerceWebSite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ManageUsersController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IToastNotification _toastNotification;

        public ManageUsersController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IToastNotification toastNotification)
        {
            _userManager = userManager;
            _toastNotification = toastNotification;
            _signInManager = signInManager;
        }
        [HttpGet]
        public async Task<IActionResult> ListSeller()
        {
            IEnumerable<ApplicationUser> Sellers = await _userManager.GetUsersInRoleAsync("Seller");
            return View(Sellers);
        }
        [HttpGet]
        public IActionResult RegisterSeller()
        {
            return PartialView("_AddSellerPartial", new VmRegister());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterSeller(VmRegister NewUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = NewUser.UserName;
                user.PasswordHash = NewUser.Password;
                user.Email = NewUser.Email;
                IdentityResult result = await _userManager.CreateAsync(user, NewUser.Password);
                if (result.Succeeded)
                {
                    IdentityResult resultrole = await _userManager.AddToRoleAsync(user, "Seller");
                    if (resultrole.Succeeded)
                    {
                        _toastNotification.AddSuccessToastMessage("New Seller Added Successfully");
                        return RedirectToAction("ListSeller", "ManageUsers");
                    }
                    else
                    {
                        foreach (var erroe in resultrole.Errors)
                        {
                            ModelState.AddModelError("", erroe.Description);
                        }
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                }

            }
            return View(NewUser);
        }
        [HttpGet]
        public IActionResult ReSetPassword(string _UserId)
        {
            return PartialView("_ResetPasswordPartial", new VmResetPassword() { UserId= _UserId });
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> ReSetPasswordSeller(VmResetPassword ModelResetPassword)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser User = await _userManager.FindByIdAsync(ModelResetPassword.UserId);
                var code = await _userManager.GeneratePasswordResetTokenAsync(User);
                IdentityResult result = await _userManager.ResetPasswordAsync(User, code, ModelResetPassword.NewPassword);
                if (result.Succeeded)
                {
                    _toastNotification.AddSuccessToastMessage("Reset Password Successfully");
                    return RedirectToAction(nameof(ListSeller));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            return View(ModelResetPassword);
        }
        public async Task<IActionResult> ChangeActivity(string UserId)
        {
            ApplicationUser User = await _userManager.FindByIdAsync(UserId);
            User.Id = UserId;
            User.UserName = User.UserName;
            User.Email = User.Email;
            User.PhoneNumber = User.PhoneNumber;
            User.ImagePath = User.ImagePath;
            if (User.IsActive == true)
            {
                User.IsActive = false;

            }
            else
            {
                User.IsActive = true;
            }
            IdentityResult result = await _userManager.UpdateAsync(User);
            if (result.Succeeded)
            {
                return Json(true);
            }
            return Json(false);
        }
        [HttpGet]
        public async Task<IActionResult> ListCustomers()
        {
            IEnumerable<ApplicationUser> Sellers = await _userManager.GetUsersInRoleAsync("User");
            return View(Sellers);
        }
    }
}
