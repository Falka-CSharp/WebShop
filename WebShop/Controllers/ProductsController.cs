using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.IO;
using WebShop.Data;
using WebShop.Models;
using WebShop.ViewModels;

namespace WebShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductsController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? cid, int? pid, int page=1)
        {
            //*
            const int pageSize = 4;

            //*
            IQueryable<Product>? products = _context?.Products?.Include(p => p.Category).Include(p => p.Producer);
            if (cid != null && cid!=0)
                products = products?.Where(p => p.CategoryId == cid);
            if (pid != null && pid!=0)
                products = products?.Where(p => p.ProducerId == pid);
            //*
            List<Category>? categories = _context?.Categories?.ToList();
            List<Producer>? producers = _context?.Producers?.ToList();

            //*
            categories?.Insert(0, new Category() { Id = 0, CategoryName = "All categories" });
            producers?.Insert(0, new Producer() { Id = 0, ProducerName = "All producers" });

            //*
            //int count = await (products!=null ? products.CountAsync() : new Task<int>(()=> { return 0; }));
            int count = await products.CountAsync();
            var items = await products.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);

            //*
            FilterViewModel viewModel = new FilterViewModel()
            {
                Products = items,
                Categories = new SelectList(categories, "Id", "CategoryName"),
                Producers = new SelectList(producers, "Id", "ProducerName"),
                PageViewModel = pageViewModel
            };

            //var applicationDbContext = _context.Products.Include(p => p.Category).Include(p => p.Producer);
            return View(viewModel);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "ProducerName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Image,IsAvaible,CategoryId,ProducerId")] Product product,
            IFormFile image)
        {
            if (ModelState.IsValid)
            {
                //*
                if (image != null)
                {
                    var name = Path.Combine(_env.WebRootPath + "/Images", Path.GetFileName(image.FileName));
                    await image.CopyToAsync(new FileStream(name,FileMode.Create, FileAccess.Write));
                    product.Image = "Images/" + image.FileName;
                }
                else
                {
                    product.Image = "Images/default.png";
                }


                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", product.CategoryId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "ProducerName", product.ProducerId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", product.CategoryId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "ProducerName", product.ProducerId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Image,IsAvaible,CategoryId,ProducerId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", product.CategoryId);
            ViewData["ProducerId"] = new SelectList(_context.Producers, "Id", "ProducerName", product.ProducerId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Producer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.Id == id);
        }
    }
}
