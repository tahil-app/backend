namespace Tahil.Application.Employees.Commands;

public record DeleteEmployeeCommand(int Id) : ICommand<Result<bool>>;

public class DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IApplicationContext applicationContext) : ICommandHandler<DeleteEmployeeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var result = await userRepository.DeleteUserAsync(request.Id, applicationContext.TenantId);

        if (result.IsSuccess)
        {
            var saveResult = await unitOfWork.SaveChangesAsync();
            return Result.Success(saveResult);
        }

        return result;
    }
} 