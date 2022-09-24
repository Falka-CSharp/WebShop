using WebShop.Models;
namespace WebShop.ViewModels
{
    public class OrderDetailsViewModel
    {
        public Order Order { get; set; } = new Order();
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
