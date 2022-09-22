using System.ComponentModel.DataAnnotations;
namespace WebShop.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string? CategoryName { get; set; }  

        public virtual List<Product>? Products { get; set; }
    }
}
