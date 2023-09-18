using System.ComponentModel.DataAnnotations;

namespace e_commerceWebSite.ViewModel
{
    public class VmEditProfile
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "Enter UserName")]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter your PhoneNumber")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public string? ImagePath { get; set; }
        
    }
}
