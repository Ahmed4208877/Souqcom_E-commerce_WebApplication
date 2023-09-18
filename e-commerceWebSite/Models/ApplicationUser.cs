using Microsoft.AspNetCore.Identity;

namespace e_commerceWebSite.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ImagePath { get; set; }
        public bool IsActive { get; set; }

        public virtual List<TbProduct>? Products { get; set; }
        public virtual List<TbCart>? Carts { get; set; }

    }
}
