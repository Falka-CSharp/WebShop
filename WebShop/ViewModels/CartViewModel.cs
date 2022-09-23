using WebShop.Models;
namespace WebShop.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<CartItem> CartItems{ get; set; } = Enumerable.Empty<CartItem>();
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhoneNumber { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
    }
}
