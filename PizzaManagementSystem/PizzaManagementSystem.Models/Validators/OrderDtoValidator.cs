using FluentValidation;
// ReSharper disable StringLiteralTypo

namespace PizzaManagementSystem.Models.Validators;

public class OrderDtoValidator : AbstractValidator<OrderDto>
{
    public OrderDtoValidator()
    {
        RuleFor(dto => dto.FirstName).Length(3, 60);
        RuleFor(dto => dto.Items).NotEmpty().WithMessage("Selezionare almeno una pizza dal menu");
        RuleForEach(dto => dto.Items).GreaterThanOrEqualTo(1).LessThanOrEqualTo(4)
            .WithMessage("L'Id della pizza deve essere >= 1 e <= 4");
    }
}