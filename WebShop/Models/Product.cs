using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string? Name { get; set; }
        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }
        [Display(Name = "Image")]
        public string? Image { get; set; }
        [Required]
        [Display(Name = "Is avaible")]
        public bool IsAvaible { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        [Required]
        [Display(Name = "Producer")]
        public int ProducerId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [ForeignKey("ProducerId")]
        public virtual Producer? Producer { get; set; }

        public virtual List<OrderItem>? Items { get; set; }

        public virtual List<CartItem>? CartItems {get;set;}
    }
}
