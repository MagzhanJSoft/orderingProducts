using Order.Domain.Models;
using Xunit;

namespace Order.Tests
{
    public class SoftDeleteTest
    {
        [Fact]
        public void SoftDelete_ShouldSetDeleteDate()
        {
            // Arrange
            var order = Orders.Create();

            // Act
            order.SoftDelete();

            // Assert
            Assert.NotNull(order.DeleteDate);
        }
    }
}
