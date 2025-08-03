using Tahil.Application.Courses.Commands;

namespace Tahil.Application.Courses.Validators;

public class CreateCourseCommandValidator : CourseCommandValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();
    }
} 