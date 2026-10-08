using Xunit;
using MicroservicioDapper.Models;

namespace MicroservicioDapper.Tests.Models
{
    public class CreateProductRequestTests
    {
        [Fact]
        public void CreateProductRequest_Should_Have_Default_Values()
        {
            // Act
            var request = new CreateProductRequest();

            // Assert
            Assert.Equal(string.Empty, request.Name);
            Assert.Equal(string.Empty, request.Description);
            Assert.Equal(0, request.Price);
            Assert.Equal(0, request.Stock);
        }
    }
}
