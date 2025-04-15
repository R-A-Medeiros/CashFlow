using CashFlow.Communication.Requests;
using FluentValidation;

namespace CashFlow.Application.UseCases.User.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage("Empty name.");
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("Empty email.")
            .EmailAddress()
            .When(user => string.IsNullOrWhiteSpace(user.Email) == false, ApplyConditionTo.CurrentValidator)
            .WithMessage("Invalid email.");
    }
}
