using System.ComponentModel.DataAnnotations;

namespace e_commerceWebSite.ViewModel
{
    public class VmResetPassword
    {
        public string UserId { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter New Password")]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Compare("NewPassword", ErrorMessage = "must be match with password")]
        public string ConfirmPassword { get; set; }
    }
}
