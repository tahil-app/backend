using Tahil.Application.Teachers.Commands;

namespace Tahil.Application.Teachers.Validators;

public class CreateTeacherCommandValidator : TeacherCommandValidator<CreateTeacherCommand>
{
    public CreateTeacherCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();


        RuleFor(x => x.Teacher.Password)
            .NotEmpty()
            .WithMessage(locale.RequiredPassword);
    }
} 