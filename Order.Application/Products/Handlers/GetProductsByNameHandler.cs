using MediatR;
using Order.Application.Common.Interfaces;
using Order.Application.Products.Queries;
using Order.Application.Products.DTOs;
using Order.Domain.Interface;
using Order.Domain.Models;
using System.Collections.Generic;

namespace Order.Application.Products.Handlers;

public class GetProductsByNameHandler : IRequestHandler<GetProductsByNameQuery, Result<List<ProductDto>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsByNameHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<List<ProductDto>>> Handle(GetProductsByNameQuery request, CancellationToken cancellationToken)
    {
        var getProductsResult = await _productRepository.GetByNameAsync(request.Name);
        if (!getProductsResult.IsSuccess)
            return Result<List<ProductDto>>.Failure(getProductsResult.Error);

        var productDtos = getProductsResult.Value.Select(p => new ProductDto(p.Id, p.Name, p.Stock)).ToList();
        return Result<List<ProductDto>>.Success(productDtos);
    }
}

