using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Order")]
        public int OrderId { get; set; }
        [Required]
        [Display(Name ="Product")]
        public int ProductId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; }
    }   
}
