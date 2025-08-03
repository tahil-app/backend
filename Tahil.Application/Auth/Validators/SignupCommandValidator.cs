using Tahil.Application.Auth.Commands;
using Tahil.Domain.Localization;

namespace Tahil.Application.Auth.Validators;

public class SignupCommandValidator : AbstractValidator<SignupCommand>
{
    public SignupCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Model).NotNull();

        RuleFor(x => x.Model.Name).NotNull().WithMessage(locale.RequiredName);
        RuleFor(x => x.Model.Email).NotNull().WithMessage(locale.RequiredEmail);
        RuleFor(x => x.Model.PhoneNumber).NotNull().WithMessage(locale.RequiredPhoneNumber);
        RuleFor(x => x.Model.Password).NotNull().WithMessage(locale.RequiredPassword);
    }
}