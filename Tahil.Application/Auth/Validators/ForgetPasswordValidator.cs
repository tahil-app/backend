using Tahil.Application.Auth.Commands;
using Tahil.Domain.Localization;

namespace Tahil.Application.Auth.Validators;

public class ForgetPasswordCommandValidator : AbstractValidator<ForgetPasswordCommand>
{
    public ForgetPasswordCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Email).NotNull().WithMessage(locale.RequiredEmail);
    }
}