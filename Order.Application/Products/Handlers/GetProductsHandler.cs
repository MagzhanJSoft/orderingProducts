using MediatR;
using Order.Application.Common.Interfaces;
using Order.Application.Products.DTOs;
using Order.Domain.Interface;
using Order.Domain.Models;
using Order.Application.Products.Queries;

namespace Order.Application.Products.Handlers;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, Result<List<ProductDto>>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<List<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var products = await _productRepository.GetAllAsync();
            var productDtos = products.Select(p => new ProductDto(p.Id, p.Name, p.Stock)).ToList();

            return Result<List<ProductDto>>.Success(productDtos);
        }
        catch (Exception ex)
        {
            return Result<List<ProductDto>>.Failure(ex.Message);
        }

    }
}

