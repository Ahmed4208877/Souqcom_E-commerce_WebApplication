using System.ComponentModel.DataAnnotations;

namespace e_commerceWebSite.ViewModel
{
    public class VmChangePassword
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter Old Password")]
        public string OldPassword { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter New Password")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Compare("NewPassword", ErrorMessage = "must be match with password")]
        public string ConfirmPassword { get; set; }
        public string? ImagePath { get; set; }
    }
}
