using MediatR;
using Order.Domain.Interface;

namespace Order.Application.Order.Command;
public record CancelOrderCommand(Guid Id) : IRequest<Result<string>>;
