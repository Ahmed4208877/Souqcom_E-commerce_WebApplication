using System.ComponentModel.DataAnnotations;

namespace e_commerceWebSite.Models
{
    public class TbNotification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string msgContent { get; set; }
    }
}
