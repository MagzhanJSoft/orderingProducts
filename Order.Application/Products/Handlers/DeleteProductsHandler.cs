using MediatR;
using Order.Application.Common.Interfaces;
using Order.Application.Products.Command;
using Order.Domain.Interface;
using Order.Domain.Models;

namespace Order.Application.Products.Handlers;

public class DeleteProductsHandler : IRequestHandler<DeleteProductsCommand, Result<Guid>>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<Guid>> Handle(DeleteProductsCommand request, CancellationToken cancellationToken)
    {
        var getProductsResult = await _productRepository.GetByIdAsync(request.Id);
        if (!getProductsResult.IsSuccess)
            return Result<Guid>.Failure(getProductsResult.Error);

        getProductsResult.Value.SoftDelete();

        var updateResult = await _productRepository.UpdateAsync(getProductsResult.Value);
        if (!updateResult.IsSuccess)
            return Result<Guid>.Failure(updateResult.Error);


        return Result<Guid>.Success(getProductsResult.Value.Id);
    }
}