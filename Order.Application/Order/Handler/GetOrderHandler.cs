using MediatR;
using Order.Application.Common.Interfaces;
using Order.Application.Order.DTOs;
using Order.Application.Order.Queries;
using Order.Domain.Interface;

namespace Order.Application.Order.Handler;

public class GetOrderHandler : IRequestHandler<GetOrdersQuery, Result<List<OrderDto>>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<List<OrderDto>>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var orders = await _orderRepository.GetAllAsync();
            var orderDtos = orders.Select(o => new OrderDto(o.Id, o.CreatedAt)).ToList();

            return Result<List<OrderDto>>.Success(orderDtos);
        }
        catch (Exception ex)
        {
            return Result<List<OrderDto>>.Failure(ex.Message);
        }
    }
}
