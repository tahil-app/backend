namespace Tahil.Application.Employees.Queries;

public record GetEmployeesPagedQuery(QueryParams QueryParams) : IQuery<Result<PagedList<UserDto>>>;

public class GetEmployeesPagedQueryHandler(IUserRepository userRepository, IApplicationContext applicationContext) : IQueryHandler<GetEmployeesPagedQuery, Result<PagedList<UserDto>>>
{
    public async Task<Result<PagedList<UserDto>>> Handle(GetEmployeesPagedQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetPagedAsync(request.QueryParams, r => r.Role == UserRole.Employee && r.TenantId == applicationContext.TenantId);
        var pagedUsers = users.Adapt<PagedList<UserDto>>();

        return Result.Success(pagedUsers);
    }
}