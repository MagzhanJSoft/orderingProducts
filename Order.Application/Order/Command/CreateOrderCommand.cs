using MediatR;
using Order.Domain.Interface;
using System.Xml.Linq;

namespace Order.Application.Order.Command;

public record OrderProductDto(Guid ProductId, int Quantity);
public record CreateOrderCommand(List<OrderProductDto> Products) : IRequest<Result<string>>;

