namespace Tahil.Application.Rooms.Validators;

public interface IRoomCommand
{
    RoomDto Room { get; }
}

public abstract class RoomCommandValidator<T> : AbstractValidator<T>
    where T : IRoomCommand
{

    private readonly LocalizedStrings locale;
    protected RoomCommandValidator(LocalizedStrings locale)
    {
        this.locale = locale;
    }

    public void AddCommonRules()
    {
        RuleFor(x => x.Room)
            .NotNull()
            .WithMessage(locale.CannotBeNull);

        When(x => x.Room != null, () =>
        {
            RuleFor(x => x.Room.Name)
                .NotEmpty()
                .WithMessage(locale.RequiredRoom)
                .MinimumLength(2)
                .WithMessage(locale.RoomNameTooShort)
                .MaximumLength(100)
                .WithMessage(locale.RoomNameTooLong);

            RuleFor(x => x.Room.Capacity)
                .GreaterThanOrEqualTo(0)
                .WithMessage(locale.MustBePositive);
        });
    }
} 