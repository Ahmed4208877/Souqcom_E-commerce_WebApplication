using System.ComponentModel.DataAnnotations;

namespace e_commerceWebSite.ViewModel
{
    public class VmRegister
    {
        [Required(ErrorMessage ="Enter UserName")]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage= "Enter Password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Compare("Password", ErrorMessage = "must be match with password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }
        public bool RememberMe { get; set; }
    }
}
