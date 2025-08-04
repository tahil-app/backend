namespace Tahil.Application.Employees.Queries;

public record GetEmployeeQuery(int Id) : IQuery<Result<UserDto>>;

public class GetEmployeeQueryHandler(IUserRepository userRepository) : IQueryHandler<GetEmployeeQuery, Result<UserDto>>
{
    public async Task<Result<UserDto>> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
    {
        var employee = await userRepository.GetAsync(r => r.Id == request.Id);
        return Result.Success(employee.Adapt<UserDto>());
    }
}