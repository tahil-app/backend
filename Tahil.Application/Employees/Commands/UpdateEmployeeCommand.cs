using Tahil.Application.Employees.Validators;

namespace Tahil.Application.Employees.Commands;

public record UpdateEmployeeCommand(UserDto Employee) : ICommand<Result<bool>>, IEmployeeCommand;

public class UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository, LocalizedStrings locale, IApplicationContext applicationContext) : ICommandHandler<UpdateEmployeeCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetAsync(u => u.Id == request.Employee.Id && u.Role == UserRole.Employee && u.TenantId == applicationContext.TenantId);
        if (user is null)
            return Result<bool>.Failure(locale.NotAvailableUser);

        user.Update(request.Employee);

        if (request.Employee.Password is not null && !string.IsNullOrEmpty(request.Employee.Password))
            user.UpdatePassword(request.Employee.Password);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
} 