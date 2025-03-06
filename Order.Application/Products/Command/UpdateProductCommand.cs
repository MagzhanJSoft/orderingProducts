using MediatR;
using Order.Domain.Interface;

namespace Order.Application.Products.Command;
public record UpdateProductCommand(Guid Id, string Name, int Stock) : IRequest<Result<UpdateProductCommand>>
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
