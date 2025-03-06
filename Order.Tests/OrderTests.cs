using Xunit;
using Order.Infrastructure.Persistence;

namespace Order.Tests
{    public class OrderTests
    {
        [Fact]
        public void CanCreateDbContext()
        {
            using var context = DbContextFactory.Create();

            Assert.NotNull(context);
            Assert.True(context.Database.CanConnect());
        }
    }

}
