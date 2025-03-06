using Microsoft.EntityFrameworkCore;
using Order.Infrastructure;

namespace Order.Tests
{
    
    public static class DbContextFactory
    {
        public static AppDbContext Create()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:") // In-Memory база данных
                .Options;

            var context = new AppDbContext(options);
            context.Database.OpenConnection(); // Открываем соединение
            context.Database.EnsureCreated(); // Создаем схему

            return context;
        }
    }

}
