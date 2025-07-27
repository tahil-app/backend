using Tahil.Application.Auth.Commands;

namespace Tahil.Application.Auth.Validators;

public class SignupCommandValidator : AbstractValidator<SignupCommand>
{
    public SignupCommandValidator()
    {
        RuleFor(x => x.Model)
            .NotNull();

        RuleFor(x => x.Model.Name).NotNull();
        RuleFor(x => x.Model.Email).NotNull();
        RuleFor(x => x.Model.PhoneNumber).NotNull();
        RuleFor(x => x.Model.Password).NotNull();
    }
}