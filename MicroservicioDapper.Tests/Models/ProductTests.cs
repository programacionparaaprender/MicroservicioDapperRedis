using Xunit;
using MicroservicioDapper.Models;

namespace MicroservicioDapper.Tests.Models
{
    public class ProductTests
    {
        [Fact]
        public void Product_Should_Set_Properties_Correctly()
        {
            // Arrange
            var createdDate = DateTime.UtcNow;

            // Act
            var product = new Product
            {
                Id = 1,
                Name = "Laptop",
                Description = "Gaming",
                Price = 2500.50m,
                Stock = 10,
                CreatedDate = createdDate
            };

            // Assert
            Assert.Equal(1, product.Id);
            Assert.Equal("Laptop", product.Name);
            Assert.Equal("Gaming", product.Description);
            Assert.Equal(2500.50m, product.Price);
            Assert.Equal(10, product.Stock);
            Assert.Equal(createdDate, product.CreatedDate);
        }
    }
}
