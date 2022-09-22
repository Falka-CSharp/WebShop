using Microsoft.AspNetCore.Identity;
namespace WebShop.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}
