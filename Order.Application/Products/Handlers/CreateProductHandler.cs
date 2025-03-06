using MediatR;
using Order.Application.Common.Interfaces;
using Order.Application.Products.Command;
using Order.Domain.Interface;
using Order.Domain.Models;
using System;

namespace Order.Application.Products.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var productCreateResult = Product.Create(Guid.NewGuid(), request.Name, request.Stock);
        if (!productCreateResult.IsSuccess)
        {
            return Result<Guid>.Failure(productCreateResult.Error);
        }

        var existingProduct = await _productRepository.GetByNameAsync(request.Name);
        if (!existingProduct.IsSuccess)
            return Result<Guid>.Failure(existingProduct.Error);
        
        if (existingProduct.Value.Count != 0)
            return Result<Guid>.Failure("Продукт с таким именем уже существует.");
        

        return await _productRepository.AddAsync(productCreateResult.Value);
                

    }
}
