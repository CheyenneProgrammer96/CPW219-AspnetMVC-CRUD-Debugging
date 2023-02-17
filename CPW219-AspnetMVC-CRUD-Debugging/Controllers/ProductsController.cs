using CPW219_AspnetMVC_CRUD_Debugging.Data;
using CPW219_AspnetMVC_CRUD_Debugging.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CPW219_AspnetMVC_CRUD_Debugging.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> products = await (from product in _context.Product
                                            select product).ToListAsync();

            return View(products);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Product.Add(product);
                await _context.SaveChangesAsync();
                
                ViewData["Message"] = $"{product.Name} was added successfully!";
                return View();
            }
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _context.Product.FindAsync(id);
            
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Product.Update(product);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            Product? productToDelete = await _context.Product.FindAsync(id);

            if (productToDelete == null) 
            {
                return NotFound();
            }

            return View(productToDelete);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product? productToDelete = await _context.Product.FindAsync(id);

            if (productToDelete == null) 
            {
                _context.Product.Remove(productToDelete);
                await _context.SaveChangesAsync();
                TempData["Message"] = productToDelete.Name + " was deleted successfully!";
                return RedirectToAction("Index");
            }

            TempData["Message"] = "This product was already deleted";
            return RedirectToAction("Index");
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
