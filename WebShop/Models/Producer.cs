using System.ComponentModel.DataAnnotations;
namespace WebShop.Models
{
    public class Producer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Producer")]
        public string? ProducerName { get; set; }

        public virtual List<Product>? Products { get; set; }
    }
}
