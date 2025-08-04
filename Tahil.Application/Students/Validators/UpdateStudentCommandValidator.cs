using Tahil.Application.Students.Commands;

namespace Tahil.Application.Students.Validators;

public class UpdateStudentCommandValidator : StudentCommandValidator<UpdateStudentCommand>
{
    public UpdateStudentCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();
    }
} 