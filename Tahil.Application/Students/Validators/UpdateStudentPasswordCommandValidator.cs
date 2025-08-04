using Tahil.Application.Students.Commands;

namespace Tahil.Application.Students.Validators;

public class UpdateStudentPasswordCommandValidator : AbstractValidator<UpdateStudentPasswordCommand>
{
    public UpdateStudentPasswordCommandValidator(LocalizedStrings locale)
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