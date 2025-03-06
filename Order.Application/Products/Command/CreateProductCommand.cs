using MediatR;
using Order.Domain.Interface;

namespace Order.Application.Products.Command;

public record CreateProductCommand(string Name, int Stock) : IRequest<Result<Guid>>
{
    public bool AreEmptyName()
    {
        return string.IsNullOrWhiteSpace(Name);
    }

    public bool QuantityCheck()
    {
        return Stock >= 0;
    }
};
