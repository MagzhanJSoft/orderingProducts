using MediatR;
using Order.Application.Order.DTOs;
using Order.Domain.Interface;

namespace Order.Application.Order.Queries;

public record GetOrdersQuery() : IRequest<Result<List<OrderDto>>>;
