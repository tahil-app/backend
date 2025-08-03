namespace Tahil.Application.Courses.Validators;

public interface ICourseCommand
{
    CourseDto Course { get; }
}

public abstract class CourseCommandValidator<T> : AbstractValidator<T>
    where T : ICourseCommand
{

    private readonly LocalizedStrings locale;
    protected CourseCommandValidator(LocalizedStrings locale)
    {
        this.locale = locale;
    }

    public void AddCommonRules()
    {
        RuleFor(x => x.Course.Id)
            .NotNull().WithMessage(locale.MustBePositive);

        RuleFor(x => x.Course.Name)
            .NotNull().WithMessage(locale.RequiredGroup)
            .MinimumLength(2).WithMessage(locale.CourseNameTooShort)
            .MaximumLength(100).WithMessage(locale.CourseNameTooLong);
    }
}