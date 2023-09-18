using e_commerceWebSite.Bl;
using e_commerceWebSite.Models;
using e_commerceWebSite.Repository;
using e_commerceWebSite.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Security.Claims;
using System.Timers;

namespace e_commerceWebSite.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IToastNotification _toastNotification;
        private readonly INotificationService _NotificationService;

        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IToastNotification toastNotification, INotificationService NotificationService)
        {
            _userManager = userManager;
            _toastNotification = toastNotification;
            _signInManager = signInManager;
            _NotificationService = NotificationService;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(VmRegister NewUser)
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
                    await _signInManager.SignInAsync(user, NewUser.RememberMe);
                    IdentityResult resultrole = await _userManager.AddToRoleAsync(user, "User");
                    if (resultrole.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
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
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(VMLogin LoginUser)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser usermodel = await _userManager.FindByEmailAsync(LoginUser.Email);
                if (usermodel != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(usermodel, LoginUser.Password);
                    bool IsActive = usermodel.IsActive;
                    if (found == true)
                    {
                        if (IsActive == true)
                        {
                            await _signInManager.SignInAsync(usermodel, LoginUser.RememberMe);
                            IEnumerable<string> Roles = await _userManager.GetRolesAsync(usermodel);

                            if (Roles.Contains("Admin"))
                            {
                                return RedirectToAction("Dashboard", "Admin");
                            }
                            else if (Roles.Contains("Seller"))
                            {
                                return RedirectToAction("ListProducts", "Product");

                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            return RedirectToAction("SendNotification", new { UserId = usermodel.Id });
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Password is wrong!");

                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Email is wrong");

                }

            }
            return View(LoginUser);
        }
        [HttpGet]
        public IActionResult SendNotification(string UserId)
        {
            return View(new TbNotification() { UserId = UserId });
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult SendNotification(TbNotification notification)
        {
            if (ModelState.IsValid)
            {
                _NotificationService.Insert(notification);
                return RedirectToAction(nameof(Login));
            }
            return View(notification);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile()
        {
            var userId = HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            VmEditProfile UsereditProfile = new VmEditProfile();
            UsereditProfile.UserId = user.Id;
            UsereditProfile.UserName = user.UserName;
            UsereditProfile.Email = user.Email;
            UsereditProfile.Phone = user.PhoneNumber;
            UsereditProfile.ImagePath = user.ImagePath;
            return View(UsereditProfile);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize]
        public async Task<IActionResult> EditProfile(VmEditProfile UserEdit, List<IFormFile> Files)
        {

            if (ModelState.IsValid)
            {
                foreach (var file in Files)
                {
                    if (file.Length > 0)
                    {
                        string ImagePath = Guid.NewGuid().ToString() + ".jpg";
                        var filepaths = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Images\Users", ImagePath);
                        using (var stream = System.IO.File.Create(filepaths))
                        {
                            await file.CopyToAsync(stream);
                        }
                        UserEdit.ImagePath = ImagePath;
                    }
                }
                ApplicationUser user = await _userManager.FindByIdAsync(UserEdit.UserId);
                user.Id = UserEdit.UserId;
                user.UserName = UserEdit.UserName;
                user.Email = UserEdit.Email;
                user.PhoneNumber = UserEdit.Phone;
                if (string.IsNullOrEmpty(UserEdit.ImagePath))
                {
                    user.ImagePath = user.ImagePath;
                }
                else
                {
                    user.ImagePath = UserEdit.ImagePath;
                }
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    _toastNotification.AddSuccessToastMessage("Profile Edit Successfully");
                    return RedirectToAction(nameof(EditProfile));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

            }
            return View(UserEdit);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangePassword()
        {
            var userId = HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser user = await _userManager.FindByIdAsync(userId);

            return View(new VmChangePassword() { ImagePath = user.ImagePath });
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangePassword(VmChangePassword ChangeAdminPassword)
        {
            ApplicationUser UserAdmin = await _userManager.FindByIdAsync(_userManager.GetUserId(User));
            ChangeAdminPassword.ImagePath = UserAdmin.ImagePath;
            if (ModelState.IsValid)
            {
                try
                {
                    bool found = await _userManager.CheckPasswordAsync(UserAdmin, ChangeAdminPassword.OldPassword);
                    if (found)
                    {
                        IdentityResult result = await _userManager.ChangePasswordAsync(UserAdmin, ChangeAdminPassword.OldPassword, ChangeAdminPassword.NewPassword);
                        if (result.Succeeded)
                        {
                            _toastNotification.AddSuccessToastMessage("Change Password Successfully");
                            return RedirectToAction(nameof(EditProfile));
                        }
                        else
                        {
                            foreach (var erroe in result.Errors)
                            {
                                ModelState.AddModelError("", erroe.Description);
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "The Old Password is Wrong!");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View("ChangePassword", ChangeAdminPassword);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
