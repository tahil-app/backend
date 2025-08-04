using Tahil.Application.Employees.Validators;
using Tahil.Domain.Entities;

namespace Tahil.Application.Employees.Commands;

public record CreateEmployeeCommand(UserDto Employee) : ICommand<Result<bool>>, IEmployeeCommand;

public class CreateEmployeeCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, IApplicationContext applicationContext) : ICommandHandler<CreateEmployeeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var user = request.Employee.Adapt<User>();
        user.Role = UserRole.Employee;

        var result = await userRepository.AddUserAsync(user, applicationContext.TenantId);

        if (result.IsSuccess)
        {
            var saveResult = await unitOfWork.SaveChangesAsync();
            return Result.Success(saveResult);
        }

        return result;
    }
} 