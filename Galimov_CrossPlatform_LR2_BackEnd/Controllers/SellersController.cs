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
    public class SellersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SellersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Sellers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seller>>> GetSellers()
        {
            return await _context.Sellers
                .Include(s => s.Products)
                .ToListAsync();
        }

        // GET: api/Sellers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Seller>> GetSeller(int id)
        {
            var seller = await _context.Sellers
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (seller == null)
            {
                return NotFound();
            }

            return seller;
        }

        // PUT: api/Sellers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeller(int id, Seller seller)
        {
            if (id != seller.Id)
            {
                return BadRequest();
            }

            _context.Entry(seller).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SellerExists(id))
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

        // POST: api/Sellers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Seller>> PostSeller(Seller seller)
        {
            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSeller", new { id = seller.Id }, seller);
        }

        // DELETE: api/Sellers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeller(int id)
        {
            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null)
            {
                return NotFound();
            }

            _context.Sellers.Remove(seller);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SellerExists(int id)
        {
            return _context.Sellers.Any(e => e.Id == id);
        }


        // GET: api/Sellers/WithProductCount/3
        [HttpGet("WithProductCount/{minProductCount}")]
        public async Task<ActionResult<IEnumerable<Seller>>> GetSellersWithProductCount(int minProductCount)
        {
            var sellers = await _context.Sellers
                .Include(s => s.Products)
                .Where(s => s.Products.Count >= minProductCount)
                .ToListAsync();

            if (!sellers.Any())
            {
                return NotFound();
            }

            return sellers;
        }

        // GET: api/Sellers/ByEmailDomain/{domain}
        [HttpGet("ByEmailDomain/{domain}")]
        public async Task<ActionResult<IEnumerable<Seller>>> GetSellersByEmailDomain(string domain)
        {
            var sellers = await _context.Sellers
                .Include(s => s.Products)
                .Where(s => s.Email.Contains(domain))
                .ToListAsync();

            if (!sellers.Any())
            {
                return NotFound();
            }

            return sellers;
        }


    }
}
