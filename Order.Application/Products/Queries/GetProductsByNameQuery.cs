using MediatR;
using Order.Application.Products.DTOs;
using Order.Domain.Interface;

namespace Order.Application.Products.Queries;
public record GetProductsByNameQuery(string Name) : IRequest<Result<List<ProductDto>>>;
