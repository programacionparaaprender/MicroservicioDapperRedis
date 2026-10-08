using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using MicroservicioDapper.Controllers;
using MicroservicioDapper.Interfaces;
using MicroservicioDapper.Models;

namespace MicroservicioDapper.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _repoMock = new Mock<IProductRepository>();
            _controller = new ProductsController(_repoMock.Object);
        }

        // -------------------------------
        // GET ALL
        // -------------------------------
        [Fact]
        public async Task GetAllProducts_ReturnsOk_WithProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };

            _repoMock
                .Setup(r => r.GetAllProductsAsync())
                .ReturnsAsync(products);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal(2, value.Count());
        }

        // -------------------------------
        // GET BY ID - OK
        // -------------------------------
        [Fact]
        public async Task GetProductById_WhenExists_ReturnsOk()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Test" };

            _repoMock
                .Setup(r => r.GetProductByIdAsync(1))
                .ReturnsAsync(product);

            // Act
            var result = await _controller.GetProductById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(1, value.Id);
        }

        // -------------------------------
        // GET BY ID - NOT FOUND
        // -------------------------------
        [Fact]
        public async Task GetProductById_WhenNotExists_ReturnsNotFound()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetProductByIdAsync(99))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProductById(99);

            // Assert
            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("not found", notFound.Value.ToString());
        }

        // -------------------------------
        // POST
        // -------------------------------
        [Fact]
        public async Task CreateProduct_ReturnsCreated()
        {
            // Arrange
            var request = new CreateProductRequest
            {
                Name = "New",
                Description = "Desc",
                Price = 10,
                Stock = 5
            };

            _repoMock
                .Setup(r => r.CreateProductAsync(It.IsAny<Product>()))
                .ReturnsAsync(1);

            // Act
            var result = await _controller.CreateProduct(request);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            var product = Assert.IsType<Product>(created.Value);
            Assert.Equal("New", product.Name);
        }

        // -------------------------------
        // PUT
        // -------------------------------
        [Fact]
        public async Task UpdateProduct_WhenExists_ReturnsNoContent()
        {
            // Arrange
            var existing = new Product { Id = 1, Name = "Old" };

            _repoMock
                .Setup(r => r.GetProductByIdAsync(1))
                .ReturnsAsync(existing);

            _repoMock
                .Setup(r => r.UpdateProductAsync(existing))
                .ReturnsAsync(true);

            var request = new UpdateProductRequest
            {
                Name = "Updated",
                Description = "Updated",
                Price = 20,
                Stock = 10
            };

            // Act
            var result = await _controller.UpdateProduct(1, request);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        // -------------------------------
        // DELETE
        // -------------------------------
        [Fact]
        public async Task DeleteProduct_WhenExists_ReturnsNoContent()
        {
            // Arrange
            _repoMock
                .Setup(r => r.GetProductByIdAsync(1))
                .ReturnsAsync(new Product { Id = 1 });

            _repoMock
                .Setup(r => r.DeleteProductAsync(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProduct(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}
