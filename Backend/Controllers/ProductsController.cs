using Backend.Data;
using Backend.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        [HttpGet("id/{id}")]
        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        [HttpGet("name/{name}")]
        public async Task<IEnumerable<Product>> GetProductsByName(string name)
        {
            return await _context.Products.Where(p => p.Name == name).ToListAsync();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<Product> CreateProduct(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}