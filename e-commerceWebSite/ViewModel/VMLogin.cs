using System.ComponentModel.DataAnnotations;

namespace e_commerceWebSite.ViewModel
{
    public class VMLogin
    {
        [Required(ErrorMessage ="Enter Your E-mail!")]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Invalid Email Format")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage ="Enter the Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
        
    }
}
