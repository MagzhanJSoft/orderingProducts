using Microsoft.EntityFrameworkCore;
using Order.Application.Common.Interfaces;
using Order.Domain.Interface;
using Order.Domain.Models;

namespace Order.Infrastructure.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;

    public OrderRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Result<string>> AddAsync(Orders order)
    {
        try
        {
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
            return Result<string>.Success("Заказ успешно создан");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message);
        }
    }

    public async Task<Result<string>> RemoveAsync(Orders order)
    {
        try
        {
            _db.Orders.Update(order);
            await _db.SaveChangesAsync();
            return Result<string>.Success("Заказ успешно отменен");
        }
        catch (Exception ex)
        {
            return Result<string>.Failure(ex.Message);
        }
    }

    public async Task<Result<Orders>> GetByIdAsync(Guid orderId)
    {
        try
        {
            var orders = await _db.Orders
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .FirstOrDefaultAsync(o => o.Id == orderId);

            return orders == null ? Result<Orders>.Failure("Заказ не найден.") : Result<Orders>.Success(orders);
        }
        catch (Exception ex)
        {
            return Result<Orders>.Failure($"Ошибка при получении продукта: {ex.Message}");
        }
    }

    public async Task<List<Orders>> GetAllAsync()
    {
        return await _db.Orders.ToListAsync();
        
    }
}
