using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Channels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using WebShop.Data;
using WebShop.Models;
using WebShop.ViewModels;

namespace WebShop.Controllers
{
    public class StatInfo
    {
        public int Count { get; set; }
        public decimal Amount { get; set; }
    }
    [Authorize]
    public class CartItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartItemsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public void DelUserProducts()
        {
            var currentUser = _context?.ApplicationUsers?.Where(u => u.UserName == User.Identity.Name).First();
            _context.CartItems.RemoveRange(_context.CartItems.Where(c => c.ApplicationUser == currentUser));
             _context.SaveChanges();
        }
        [HttpPost]
        public void PlaceUserOrder(string CustomerName, string CustomerPhoneNumber, string CustomerAddress)
        {
            var currentUser = _context?.ApplicationUsers?.Where(u => u.UserName == User.Identity.Name).First();
            Console.WriteLine($"\n\n------{CustomerName} {CustomerPhoneNumber} {CustomerAddress} ----\n\n");

            string orderNumber = currentUser?.Id + DateTime.Now;

            Order order = new Order()
            {
                PersonName = CustomerName,
                PhoneNumber = CustomerPhoneNumber,
                Address = CustomerAddress,
                Email = currentUser?.Email,
                OrderDate = DateTime.Now,
                UserId = currentUser?.Id,
                OrderNumber = orderNumber
            };


            var currentUserCartItems = _context.CartItems
                   .Include(x => x.ApplicationUser)
                   .Include(x => x.Product)
                   .Where(x => x.ApplicationUser.UserName == User.Identity.Name)
                   .ToList();

            List<OrderItem> items = new List<OrderItem>();
            foreach (var item in currentUserCartItems)
            {
                items.Add(new OrderItem { Order = order, Product = item.Product });
            }

            _context?.Orders?.Add(order);
            foreach(var item in items)
                _context?.OrderItems?.Add(item);

             _context.SaveChanges();

        }

        [HttpPost]
        public StatInfo AddProductToCart(int productId)
        {
            var currentUser = _context?.ApplicationUsers?.Where(u => u.UserName == User.Identity.Name).First();
            _context.CartItems.Add(new CartItem()
            {
                UserId = currentUser?.Id,
                ProductId = productId
            }) ;
            _context?.SaveChanges();
            return GetStatInfo(); 
        }
        [HttpPost]
        public StatInfo GetStatInfo()
        {
            var currentUser = _context?.ApplicationUsers?.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            if (currentUser == null)
                return new StatInfo() { Count = 0, Amount = 0.0M };
            else
            {
                int count = 0;
                decimal amount = 0.0M;
                var currentUserCartItems = _context.CartItems
                    .Include(x => x.ApplicationUser)
                    .Include(x => x.Product)
                    .Where(x => x.ApplicationUser.UserName == User.Identity.Name)
                    .ToList();

                foreach(var cartItem in currentUserCartItems)
                {
                    count++;
                    amount += cartItem.Product.Price;
                }

                return new StatInfo() { Count = count, Amount = amount };
            }
        }

        // GET: CartItems
        public async Task<IActionResult> Index()
        {
            var currentUserCartItems = _context.CartItems
                    .Include(x => x.ApplicationUser)
                    .Include(x => x.Product)
                    .Where(x => x.ApplicationUser.UserName == User.Identity.Name);
            // var applicationDbContext = _context.CartItems.Include(c => c.ApplicationUser).Include(c => c.Product);
            //CartViewModel cvm = new CartViewModel { CartItems = await currentUserCartItems.ToListAsync() };
            return View(await currentUserCartItems.ToListAsync());
        }

        // GET: CartItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CartItems == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems
                .Include(c => c.ApplicationUser)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // GET: CartItems/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        // POST: CartItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,UserId")] CartItem cartItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cartItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", cartItem.UserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cartItem.ProductId);
            return View(cartItem);
        }

        // GET: CartItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CartItems == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", cartItem.UserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cartItem.ProductId);
            return View(cartItem);
        }

        // POST: CartItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,UserId")] CartItem cartItem)
        {
            if (id != cartItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cartItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CartItemExists(cartItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.ApplicationUsers, "Id", "Id", cartItem.UserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", cartItem.ProductId);
            return View(cartItem);
        }

        // GET: CartItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CartItems == null)
            {
                return NotFound();
            }

            var cartItem = await _context.CartItems
                .Include(c => c.ApplicationUser)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cartItem == null)
            {
                return NotFound();
            }

            return View(cartItem);
        }

        // POST: CartItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CartItems == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CartItems'  is null.");
            }
            var cartItem = await _context.CartItems.FindAsync(id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CartItemExists(int id)
        {
          return _context.CartItems.Any(e => e.Id == id);
        }
    }
}
