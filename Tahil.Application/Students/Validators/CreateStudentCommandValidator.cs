using Tahil.Application.Students.Commands;

namespace Tahil.Application.Students.Validators;

public class CreateStudentCommandValidator : StudentCommandValidator<CreateStudentCommand>
{
    public CreateStudentCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();


        RuleFor(x => x.Student.Password)
            .NotEmpty()
            .WithMessage(locale.RequiredPassword);
    }
} 