using Order.Domain.Interface;
using Order.Domain.Models;

namespace Order.Application.Common.Interfaces
{
    public interface IOrderRepository
    {
        Task<Result<string>> AddAsync(Orders order);
        Task<List<Orders>> GetAllAsync();
        Task<Result<Orders>> GetByIdAsync(Guid orderId);
        Task<Result<string>> RemoveAsync(Orders order);
    }
}