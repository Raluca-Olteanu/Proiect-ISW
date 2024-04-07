using Backend.Data;
using Backend.Dtos;
using Backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;

        public UsersController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("id/{id}")]
        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("username/{username}")]
        public async Task<ActionResult<User>> GetUserByUsername(string username)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Username == username);
        }

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("purchases")]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetAllPurchases()
        {
            var purchases = await _context.Purchases.Include(p => p.Products).ToListAsync();
            return purchases;
        }

        [HttpPost("products")]
        [Authorize(Roles = "Admin")]
        public async Task<Product> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        [HttpPost("purchases")]
        public async Task<ActionResult<Purchase>> CreatePurchase(PurchaseDto purchaseDto)
        {
            try
            {
                // Create a new Purchase entity
                var purchase = new Purchase
                {
                    UserId = purchaseDto.UserId,
                    PurchaseDate = DateTime.Now,
                    Products = new List<Product>() // Initialize the Products collection
                };

                // Retrieve products from the database based on the received product IDs
                foreach (var productId in purchaseDto.ProductIds)
                {
                    var product = await _context.Products.FindAsync(productId);
                    if (product != null)
                    {
                        // Associate the product with the purchase
                        purchase.Products.Add(product);
                    }
                    else
                    {
                        // Product with the given ID not found
                        return BadRequest($"Product with ID {productId} not found.");
                    }
                }

                // Add the purchase to the database
                _context.Purchases.Add(purchase);
                await _context.SaveChangesAsync();

                // Return the created purchase
                return CreatedAtAction(nameof(GetPurchase), new { id = purchase.Id }, purchase);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the purchase: {ex.Message}");
            }
        }

        [HttpGet("purchases/id/{id}")]
        public async Task<Purchase> GetPurchase(int id)
        {
            var purchase = await _context.Purchases
                .Include(p => p.Products) // Eager loading the Products navigation property
                .FirstOrDefaultAsync(p => p.Id == id);

            return purchase;
        }
    }
}