using Microsoft.AspNetCore.Mvc.Rendering;
using WebShop.Models;
namespace WebShop.ViewModels
{
    public class FilterViewModel
    {
        public IEnumerable<Product>? Products { get; set; }
        public SelectList? Categories { get; set; }
        public SelectList? Producers { get; set; }
        public PageViewModel? PageViewModel { get; set; }
    }
}
