using Microsoft.AspNetCore.Mvc;
using MicroservicioDapper.Interfaces;
using MicroservicioDapper.Models;

namespace MicroservicioDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/products
        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _productRepository.GetAllProductsAsync();
            return Ok(products);
        }

        // GET api/products/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            
            if (product == null)
                return NotFound($"Product with ID {id} not found");
            
            return Ok(product);
        }

        // POST api/products
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Stock = request.Stock
            };

            var id = await _productRepository.CreateProductAsync(product);
            product.Id = id;
            return CreatedAtAction(nameof(GetProductById), new { id }, product);
        }

        // PUT api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct == null)
                return NotFound($"Product with ID {id} not found");

            existingProduct.Name = request.Name;
            existingProduct.Description = request.Description;
            existingProduct.Price = request.Price;
            existingProduct.Stock = request.Stock;

            var updated = await _productRepository.UpdateProductAsync(existingProduct);
            
            if (!updated)
                return StatusCode(500, "Failed to update product");
            
            return NoContent();
        }

        // DELETE api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct == null)
                return NotFound($"Product with ID {id} not found");

            var deleted = await _productRepository.DeleteProductAsync(id);
            
            if (!deleted)
                return StatusCode(500, "Failed to delete product");
            
            return NoContent();
        }
    }
}