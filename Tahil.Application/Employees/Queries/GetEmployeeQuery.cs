namespace Tahil.Application.Employees.Queries;

public record GetEmployeeQuery(int Id) : IQuery<Result<UserDto>>;

public class GetEmployeeQueryHandler(IUserRepository userRepository, IApplicationContext applicationContext) : IQueryHandler<GetEmployeeQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
    {
        var employee = await userRepository.GetAsync(r => r.Id == request.Id && r.TenantId == applicationContext.TenantId);
        return Result.Success(employee.Adapt<UserDto>());
    }
}