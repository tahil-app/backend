namespace Tahil.Application.Employees.Commands;

public record DeActivateEmployeeCommand(int Id) : ICommand<Result<bool>>;

public class DeActivateEmployeeCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, LocalizedStrings locale) : ICommandHandler<DeActivateEmployeeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeActivateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(u => u.Id == request.Id && u.Role == UserRole.Employee);
        if (user is null)
            return Result<bool>.Failure(locale.NotAvailableUser);

        user.DeActivate();

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
} 