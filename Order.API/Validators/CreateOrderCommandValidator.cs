using FluentValidation;
using Order.Application.Order.Command;

namespace Order.API.Validators;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleForEach(x => x.Products)
            .ChildRules(product =>
            {
                product.RuleFor(p => p.Quantity)
                    .GreaterThan(0).WithMessage("Количество должно быть больше нуля.");
            });
    }
}