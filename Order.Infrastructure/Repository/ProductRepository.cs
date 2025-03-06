using Microsoft.EntityFrameworkCore;
using Order.Application.Common.Interfaces;
using Order.Domain.Interface;
using Order.Domain.Models;

namespace Order.Infrastructure.Repository;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _db;
    public ProductRepository(AppDbContext db) => _db = db;

    public async Task<Result<List<Product>>> GetProductsByIdsAsync(IEnumerable<Guid> productIds)
    {
        try
        {
            var product = await _db.Products
                        .Where(p => productIds.Contains(p.Id))
                        .ToListAsync();

            return Result<List<Product>>.Success(product);

        }
        catch (Exception ex)
        {
            return Result<List<Product>>.Failure($"Ошибка обновления продуктов: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateRangeAsync(IEnumerable<Product> products)
    {
        try
        {
            _db.Products.UpdateRange(products);
            await _db.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Ошибка обновления продуктов: {ex.Message}");
        }
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _db.Products.ToListAsync();
    }

    public async Task<Result<Product>> GetByIdAsync(Guid id)
    {
        try
        {
            var product = await _db.Products.FindAsync(id);
            return product == null ? Result<Product>.Failure("Продукт не найден.") : Result<Product>.Success(product);
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure($"Ошибка при получении продукта: {ex.Message}");
        }
    }

    public async Task<Result<List<Product>>> GetByNameAsync(string name)
    {
        try
        {
            var products = await _db.Products.Where(p => p.Name == name).ToListAsync();
            return Result<List<Product>>.Success(products);
        }
        catch (Exception ex)
        {
            return Result<List<Product>>.Failure($"Ошибка при поиске продуктов: {ex.Message}");
        }
    }

    public async Task<Result<Guid>> AddAsync(Product product)
    {
        try
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return Result<Guid>.Success(product.Id);
        }
        catch (DbUpdateException dbEx) // Ошибка обновления БД
        {
            return Result<Guid>.Failure($"Ошибка сохранения в БД: {dbEx.Message}");
        }
        catch (Exception ex)
        {
            return Result<Guid>.Failure($"Ошибка при добавлении продукта: {ex.Message}");
        }
    }

    public async Task<Result<bool>> UpdateAsync(Product product)
    {
        try
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return Result<bool>.Success(true);
        }
        catch (DbUpdateException dbEx)
        {
            return Result<bool>.Failure($"Ошибка обновления БД: {dbEx.Message}");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure($"Ошибка при обновлении продукта: {ex.Message}");
        }
    }
}
