using MediatR;
using Order.Domain.Interface;

namespace Order.Application.Products.Command;
public record DeleteProductsCommand(Guid Id) : IRequest<Result<Guid>>;