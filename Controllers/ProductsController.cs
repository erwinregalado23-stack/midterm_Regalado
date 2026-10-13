
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using midterm_Regalado.Data;
using midterm_Regalado.Models;

namespace midterm_Regalado.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // READ + AUTO-SEED
        public async Task<IActionResult> Index()
        {
            if (!await _context.Products.AnyAsync())
            {
                _context.Products.AddRange(
                    new Product
                    {
                        Name = "Dior Sauvage",
                        Category = "woody",
                        Description = "Fresh and woody fragrance",
                        Price = 4500
                    },
                    new Product
                    {
                        Name = "Eros",
                        Category = "mint, vanilla",
                        Description = "date night scent",
                        Price = 6000
                    },
                    new Product
                    {
                        Name = "tobacco vanilla",
                        Category = "vanilla, tobacco, mint",
                        Description = "formal event",
                        Price = 8000
                    },
                    new Product
                    {
                        Name = "Ultra Male",
                        Category = "Vanilla, Lavender, Bergamot, and Lemon",
                        Description = "pear bubblegum scent",
                        Price = 7500
                    },
                    new Product
                    {
                        Name = "Bleu de Chanel",
                        Category = "woody, citrus",
                        Description = "Fresh and elegant fragrance",
                        Price = 6500
                    }
                );

                await _context.SaveChangesAsync();
            }

            return View(await _context.Products.ToListAsync());
        }

        // CREATE - GET
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Name,Description,Price,Category")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // UPDATE - GET
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Name,Description,Price,Category")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // DELETE - GET: Show confirmation page
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // DELETE - POST: Delete the selected product
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

