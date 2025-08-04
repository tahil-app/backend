namespace Tahil.Application.Employees.Commands;

public record DeleteEmployeeCommand(int Id) : ICommand<Result<bool>>;

public class DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, LocalizedStrings locale) : ICommandHandler<DeleteEmployeeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(u => u.Id == request.Id && u.Role == UserRole.Employee);
        if (user is null)
            return Result<bool>.Failure(locale.NotAvailableUser);

        var result = await userRepository.DeleteUserAsync(request.Id);

        if (result.IsSuccess)
        {
            var saveResult = await unitOfWork.SaveChangesAsync();
            return Result.Success(saveResult);
        }

        return result;
    }
} 