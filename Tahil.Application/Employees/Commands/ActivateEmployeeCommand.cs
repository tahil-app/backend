namespace Tahil.Application.Employees.Commands;

public record ActivateEmployeeCommand(int Id) : ICommand<Result<bool>>;

public class ActivateEmployeeCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, LocalizedStrings locale) : ICommandHandler<ActivateEmployeeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ActivateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(u => u.Id == request.Id && u.Role == UserRole.Employee);
        if (user is null)
            return Result<bool>.Failure(locale.NotAvailableUser);

        user.Activate();

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
} 