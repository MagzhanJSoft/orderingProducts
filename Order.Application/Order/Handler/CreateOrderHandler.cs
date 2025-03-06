using MediatR;
using Order.Application.Common.Interfaces;
using Order.Application.Order.Command;
using Order.Domain.Interface;
using Order.Domain.Models;

namespace Order.Application.Order.Handler;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Result<string>>
{
    private readonly IProductRepository _productRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;
    public CreateOrderHandler(IProductRepository productRepository,
        IOrderRepository orderRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<string>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        using var transaction = await _unitOfWork.BeginTransactionAsync();

        try 
        { 

            var order = Orders.Create();

            var getProductsResult = await _productRepository.GetProductsByIdsAsync(request.Products.Select(p => p.ProductId));
            if (!getProductsResult.IsSuccess)
                return Result<string>.Failure(getProductsResult.Error);

            var products = getProductsResult.Value;

            foreach (var (productId, quantity) in request.Products)
            {
                var product = products.FirstOrDefault(p => p.Id == productId);
                if (product == null)
                    return Result<string>.Failure($"Товар не найден: {productId}");

                if (product.Stock < quantity)
                    return Result<string>.Failure($"Недостаточно товаров: {productId} в количестве {quantity - product.Stock}");

                var addProductResult = order.AddProduct(product, quantity);
                if (!addProductResult.IsSuccess)
                    return Result<string>.Failure(addProductResult.Error);

            }

            // Обновляем запасы продуктов
            var updateResult = await _productRepository.UpdateRangeAsync(products);
            if (!updateResult.IsSuccess)
                return Result<string>.Failure(updateResult.Error);

            var addResult = await _orderRepository.AddAsync(order);
            if (!addResult.IsSuccess)
                return Result<string>.Failure(updateResult.Error);

            await _unitOfWork.CommitAsync();

            return Result<string>.Success(addResult.Value);
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync(); 
            return Result<string>.Failure($"Ошибка при создании заказа: {ex.Message}");
        }
    }
}
