using MediatR;
using Microsoft.EntityFrameworkCore;
using Order.Application.Common.Interfaces;
using Order.Application.Order.Command;
using Order.Domain.Interface;
using Order.Domain.Models;

namespace Order.Application.Order.Handler;

public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, Result<string>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public CancelOrderHandler(IUnitOfWork unitOfWork, IOrderRepository orderRepository, IProductRepository productRepository)
    {
        _unitOfWork = unitOfWork;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public async Task<Result<string>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var getOrderResult = await _orderRepository.GetByIdAsync(request.Id);
            if (!getOrderResult.IsSuccess)
                return Result<string>.Failure(getOrderResult.Error);

            Orders order = getOrderResult.Value;

            if (order.OrderProducts.Count == 0)
                return Result<string>.Failure("Заказ уже пуст или отменен");

            var productIds = order.OrderProducts.Select(op => op.ProductId).ToList();
            var getProductsResult = await _productRepository.GetProductsByIdsAsync(productIds);
            if (!getProductsResult.IsSuccess)
                return Result<string>.Failure(getProductsResult.Error);

            var products = getProductsResult.Value;

            if (products.Count != productIds.Count)
                return Result<string>.Failure("Некоторые продукты не найдены");

            foreach (var orderProduct in order.OrderProducts)
            {
                var product = products.FirstOrDefault(p => p.Id == orderProduct.ProductId);
                if (product == null)
                    return Result<string>.Failure($"Товар не найден: {orderProduct.ProductId}");

                var increaseResult = product.IncreaseStock(orderProduct.Quantity);
                if (!increaseResult.IsSuccess)
                    return Result<string>.Failure(increaseResult.Error);
            }

            // Обновляем продукты
            var updateResult = await _productRepository.UpdateRangeAsync(products);
            if (!updateResult.IsSuccess)
                return Result<string>.Failure(updateResult.Error);

            order.SoftDelete();

            // Сохраняем изменения
            var removeResult = await _orderRepository.RemoveAsync(order);
            if (!removeResult.IsSuccess)
                return Result<string>.Failure(removeResult.Error);

            // Фиксируем транзакцию
            await _unitOfWork.CommitAsync();
            return Result<string>.Success("Заказ успешно отменен");
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            return Result<string>.Failure($"Ошибка при отмене заказа: {ex.Message}");
        }
    }
}