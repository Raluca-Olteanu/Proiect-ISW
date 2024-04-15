using Backend.Data;
using Backend.Dtos;
using Backend.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly DataContext _context;

        public PurchasesController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetAllPurchases()
        {
            var purchases = await _context.Purchases.Include(p => p.Products).ToListAsync();
            return purchases;
        }

        [HttpPost]
        public async Task<ActionResult<Purchase>> CreatePurchase(PurchaseDto purchaseDto)
        {
            try
            {
                var purchase = new Purchase
                {
                    UserId = purchaseDto.UserId,
                    PurchaseDate = DateTime.Now,
                    Products = new List<Product>()
                };

                foreach (var productId in purchaseDto.ProductIds)
                {
                    var product = await _context.Products.FindAsync(productId);
                    if (product != null)
                    {
                        purchase.Products.Add(product);
                    }
                    else
                    {
                        return BadRequest($"Product with ID {productId} not found.");
                    }
                }
                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPurchase), new { id = purchase.Id }, purchase);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the purchase: {ex.Message}");
            }
        }

        [HttpGet("id/{id}")]
        public async Task<Purchase> GetPurchase(int id)
        {
            var purchase = await _context.Purchases
                .Include(p => p.Products)
                .FirstOrDefaultAsync(p => p.Id == id);

            return purchase;
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeletePurchase(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }
            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}