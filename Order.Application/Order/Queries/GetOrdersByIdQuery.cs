using MediatR;
using Order.Application.Order.DTOs;
using Order.Application.Products.DTOs;
using Order.Domain.Interface;
using Order.Domain.Models;

namespace Order.Application.Order.Queries;
public record GetOrdersByIdQuery(Guid Id) : IRequest<Result<OrderWithProductsDto>>;
