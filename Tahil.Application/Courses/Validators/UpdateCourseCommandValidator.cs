using Tahil.Application.Courses.Commands;

namespace Tahil.Application.Courses.Validators;

public class UpdateCourseCommandValidator : CourseCommandValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();
    }
} 