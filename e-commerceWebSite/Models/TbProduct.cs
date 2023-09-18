using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerceWebSite.Models
{
    public class TbProduct
    {
        [Key]
        public int Id { get; set;}
        [Required]
        public string Name { get; set;}
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set;}
        [Required]
        public double Price { get; set; }
        public string? ImagePath { get; set;}
        public bool IsActive { get; set;}
        public bool IsDeleted { get; set; }
        public int StockQuantity { get; set; }
        public DateTime? ExipirationDate { get; set;}
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        [ForeignKey(nameof(Seller))]
        public string SellerId { get; set; }


        public virtual TbCategory? Category { get; set; }
        public virtual ApplicationUser? Seller { get; set; }
        public virtual List<TbCartProduct>? TbCartProducts { get; set; }


    }
}
