using MediatR;
using Order.Application.Products.DTOs;
using Order.Domain.Interface;

namespace Order.Application.Products.Queries;
public record GetProductsQuery() : IRequest<Result<List<ProductDto>>>;
