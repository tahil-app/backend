using Tahil.Application.ClassSessions.Commands;

namespace Tahil.Application.ClassSessions.Validators;

public class UpdateSessionStatusCommandValidator : AbstractValidator<UpdateClassSessionStatusCommand>
{
    public UpdateSessionStatusCommandValidator(LocalizedStrings locale, IClassSessionRepository classSessionRepository, IApplicationContext applicationContext)
    {
        RuleFor(x => x.SessionId)
            .GreaterThan(0)
            .WithMessage(locale.MustBePositive);

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage(locale.InvalidStatus);

        RuleFor(x => x)
            .MustAsync(async (command, cancellation) =>
            {
                if (command.Status != ClassSessionStatus.Completed)
                    return true;

                var hasIncompleteAttendances = await classSessionRepository.HasIncompleteAttendancesAsync(command.SessionId, applicationContext.TenantId);
                return !hasIncompleteAttendances;
            })
            .WithMessage(locale.CannotCompleteSessionWithIncompleteAttendance);
    }
} 