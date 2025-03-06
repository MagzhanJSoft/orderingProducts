using MediatR;
using Order.Application.Common.Interfaces;
using Order.Application.Order.DTOs;
using Order.Application.Order.Queries;
using Order.Application.Products.DTOs;
using Order.Application.Products.Queries;
using Order.Domain.Interface;
using Order.Domain.Models;

namespace Order.Application.Order.Handler;

public class GetOrdersByIdHandler : IRequestHandler<GetOrdersByIdQuery, Result<OrderWithProductsDto>>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersByIdHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Result<OrderWithProductsDto>> Handle(GetOrdersByIdQuery request, CancellationToken cancellationToken)
    {
        var getOrderResult = await _orderRepository.GetByIdAsync(request.Id);
        if (!getOrderResult.IsSuccess)
            return Result<OrderWithProductsDto>.Failure(getOrderResult.Error);

        var order = getOrderResult.Value;

        var orderDto = new OrderWithProductsDto(
            order.Id,
            order.OrderProducts.Select(op => new ProductInOrderDto(
                op.Product.Id,
                op.Product.Name,
                op.Quantity
            )).ToList()
        );

        return Result<OrderWithProductsDto>.Success(orderDto);
    }
}