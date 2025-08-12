using Tahil.Application.Courses.Commands;

public class DeleteClassScheduleCommandValidator : AbstractValidator<DeleteCourseCommand>
{
    public DeleteClassScheduleCommandValidator(LocalizedStrings locale)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);
    }
}