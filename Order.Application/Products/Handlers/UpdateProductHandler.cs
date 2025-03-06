using MediatR;
using Order.Application.Common.Interfaces;
using Order.Application.Products.Command;
using Order.Domain.Interface;
using Order.Domain.Models;

namespace Order.Application.Products.Handlers;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Result<UpdateProductCommand>>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<UpdateProductCommand>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var productResult = await _productRepository.GetByIdAsync(request.Id);
        if (!productResult.IsSuccess)
        {
            return Result<UpdateProductCommand>.Failure(productResult.Error);
        }

        var existingProduct = await _productRepository.GetByNameAsync(request.Name);
        if (!existingProduct.IsSuccess)
            return Result<UpdateProductCommand>.Failure(existingProduct.Error);

        if (existingProduct.Value.Count != 0)
            return Result<UpdateProductCommand>.Failure("Продукт с таким именем уже существует.");

        var product = productResult.Value;
        var updateProductResult = product.Update(request.Name, request.Stock);
        if (!updateProductResult.IsSuccess) 
            return Result<UpdateProductCommand>.Failure(updateProductResult.Error);

        var updateResult = await _productRepository.UpdateAsync(product);
        if (!updateResult.IsSuccess)
            return Result<UpdateProductCommand>.Failure(updateResult.Error);
        

        return Result<UpdateProductCommand>.Success(request);

    }
}
