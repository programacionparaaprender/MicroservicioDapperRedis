using Xunit;
using MicroservicioDapper.Models;

namespace MicroservicioDapper.Tests.Models
{
    public class UpdateProductRequestTests
    {
        [Fact]
        public void UpdateProductRequest_Should_Assign_Values()
        {
            // Act
            var request = new UpdateProductRequest
            {
                Name = "Updated",
                Description = "Updated",
                Price = 100,
                Stock = 3
            };

            // Assert
            Assert.Equal("Updated", request.Name);
            Assert.Equal("Updated", request.Description);
            Assert.Equal(100, request.Price);
            Assert.Equal(3, request.Stock);
        }
    }
}
