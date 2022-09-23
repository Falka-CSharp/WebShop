using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    
    public class Order
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="Order number")]
        public string? OrderNumber { get; set; }
        [Required]
        [Display(Name = "Customer name")]
        public string? PersonName { get; set; }
        [Required]
        [Display(Name = "Customer phone")]
        public string? PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Customer E-Mail")]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "Customer address")]
        public string? Address { get; set; }
        [Required]
        [Display(Name = "Order date")]
        public DateTime? OrderDate { get; set; }
        [Required]
        [Display(Name = "User")]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        public virtual List<OrderItem>? Items { get; set; }
    }
}
