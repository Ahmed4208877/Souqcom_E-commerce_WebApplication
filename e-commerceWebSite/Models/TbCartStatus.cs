using System.ComponentModel.DataAnnotations;

namespace e_commerceWebSite.Models
{
    public class TbCartStatus
    {
        [Key]
        public string Id { get; set; }
        public string Status { get; set; }

        public virtual List<TbCart>? Carts { get; set; }
    }
}
