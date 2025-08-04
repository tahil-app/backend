using Tahil.Application.Teachers.Commands;

namespace Tahil.Application.Teachers.Validators;

public class UpdateTeacherPasswordCommandValidator : AbstractValidator<UpdateTeacherPasswordCommand>
{
    public UpdateTeacherPasswordCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Params)
            .NotNull()
            .WithMessage(locale.CannotBeNull);

        When(x => x.Params != null, () =>
        {
            RuleFor(x => x.Params.Password)
                .NotEmpty()
                .WithMessage(locale.RequiredPassword)
                .MinimumLength(6)
                .WithMessage(locale.PasswordTooShort);
        });
    }
} 