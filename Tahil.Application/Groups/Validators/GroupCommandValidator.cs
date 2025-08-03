namespace Tahil.Application.Groups.Validators;

public interface IGroupCommand
{
    GroupDto Group { get; }
}

public abstract class GroupCommandValidator<T> : AbstractValidator<T>
    where T : IGroupCommand
{

    private readonly LocalizedStrings locale;
    protected GroupCommandValidator(LocalizedStrings locale)
    {
        this.locale = locale;
    }

    public void AddCommonRules()
    {
        RuleFor(x => x.Group)
            .NotNull()
            .WithMessage(locale.CannotBeNull);

        When(x => x.Group != null, () =>
        {
            RuleFor(x => x.Group.Name)
                .NotEmpty()
                .WithMessage(locale.RequiredGroup)
                .MinimumLength(2)
                .WithMessage(locale.GroupNameTooShort)
                .MaximumLength(100)
                .WithMessage(locale.GroupNameTooLong);

            RuleFor(x => x.Group.CourseId)
                .GreaterThan(0)
                .WithMessage(locale.MustBePositive);

            RuleFor(x => x.Group.TeacherId)
                .GreaterThan(0)
                .WithMessage(locale.MustBePositive);
        });
    }
}