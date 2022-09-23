using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Product")]
        public int ProductId { get; set; }
        [Required]
        [Display(Name = "User")]
        public string?   UserId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? ApplicationUser { get; set; }
    }
}
