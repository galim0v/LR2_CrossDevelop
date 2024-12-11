using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Galimov_CrossPlatform_LR2_BackEnd.Data;
using Galimov_CrossPlatform_LR2_BackEnd.Models;

namespace Galimov_CrossPlatform_LR2_BackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        // GET: api/Products/BySeller/5
        [HttpGet("BySeller/{sellerId}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsBySeller(int sellerId)
        {
            var products = await _context.Products
                .Where(p => p.SellerId == sellerId)
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound();
            }

            return products;
        }

        [HttpGet("ByAveragePrice/{averagePrice}")]
        public async Task<ActionResult<IEnumerable<object>>> GetProductsByAveragePriceAsync(decimal averagePrice)
        {
            var sellers = await _context.Sellers
                .Include(s => s.Products)
                .ToListAsync();

            var result = sellers
                .Where(s => s.Products.Any())
                .Select(s => new
                {
                    SellerName = s.Name,
                    SellerEmail = s.Email,
                    Products = s.Products,
                    AveragePrice = s.Products.Average(p => p.Price)
                })
                .Where(s => Math.Abs(s.AveragePrice - averagePrice) < 0.01m)
                .ToList();

            if (!result.Any())
            {
                return NotFound();
            }

            return Ok(result);
        }




        // GET: api/Products/ByPrice/{minPrice}
        [HttpGet("ByPrice/{minPrice}")]
        public async Task<ActionResult<IEnumerable<object>>> GetProductsByPrice(decimal minPrice)
        {
            var products = await _context.Products
                .Where(p => p.Price > minPrice)
                .Select(p => new
                {
                    ProductName = p.Name,
                    ProductPrice = p.Price,
                    SellerName = _context.Sellers
                        .Where(s => s.Id == p.SellerId)
                        .Select(s => s.Name)
                        .FirstOrDefault()
                })
                .ToListAsync();

            if (!products.Any())
            {
                return NotFound();
            }

            return Ok(products);
        }

    }
}
