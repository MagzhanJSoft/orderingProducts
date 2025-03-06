using Order.Domain.Interface;
using Order.Domain.Models;

namespace Order.Application.Common.Interfaces;

public interface IProductRepository
{
    Task<Result<Guid>> AddAsync(Product product);
    Task<List<Product>> GetAllAsync();
    Task<Result<Product>> GetByIdAsync(Guid id);
    Task<Result<List<Product>>> GetByNameAsync(string name);
    Task<Result<List<Product>>> GetProductsByIdsAsync(IEnumerable<Guid> productIds);
    Task<Result<bool>> UpdateAsync(Product product);
    Task<Result<bool>> UpdateRangeAsync(IEnumerable<Product> products);
}
