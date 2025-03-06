
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Common.Interfaces;
using Order.Application.Products.Handlers;
using Order.Infrastructure;
using Order.Infrastructure.Persistence;
using Order.Infrastructure.Repository;
using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Order.API.Validators;

namespace Order.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddSwaggerGen();

            
            builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(builder.Configuration.GetConnectionString("AppDbConnection"))
                );

            
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetProductsHandler).Assembly));

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.MapControllers();

            app.UseRouting();
            app.Run();
        }
    }
}
