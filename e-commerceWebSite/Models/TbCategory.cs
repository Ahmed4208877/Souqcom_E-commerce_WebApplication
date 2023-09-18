using System.ComponentModel.DataAnnotations;

namespace e_commerceWebSite.Models
{
    public class TbCategory
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime InsertionDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        public virtual List<TbProduct>? Products { get; set; }


    }
}
