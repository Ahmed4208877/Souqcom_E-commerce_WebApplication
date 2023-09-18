using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerceWebSite.Models
{
    public class TbCart
    {
        [Key]
        public string Id { get; set; }
        public double Total_Price { get; set; }
        public DateTime InsertionData { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        [ForeignKey(nameof(CartStatus))]
        public string CartStatusId { get; set; }



        public virtual ApplicationUser User { get; set; }
        public virtual TbCartStatus CartStatus { get; set; }
        public virtual List<TbCartProduct>? TbCartProducts { get; set; }
    }
}
