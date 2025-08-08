using System.Linq.Expressions;
using Tahil.Domain.Entities;

namespace Tahil.Application.Groups.Queries;

public record GetGroupsPagedQuery(QueryParams QueryParams) : IQuery<Result<PagedList<GroupDto>>>;

public class GetGroupsPagedQueryHandler(IGroupRepository groupRepository, IApplicationContext applicationContext) : IQueryHandler<GetGroupsPagedQuery, Result<PagedList<GroupDto>>>
{
    public async Task<Result<PagedList<GroupDto>>> Handle(GetGroupsPagedQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Group, bool>> filter = applicationContext.UserRole == UserRole.Teacher
            ? group => group.TeacherId == applicationContext.UserId && group.TenantId == applicationContext.TenantId
            : group => group.TenantId == applicationContext.TenantId;

        var includes = new Expression<Func<Group, object>>[]
        {
            r => r.StudentGroups,
            r => r.Course!,
            r => r.Teacher!
        };

        //if (applicationContext.UserRole == UserRole.Teacher)
        //{
        //    includes = new Expression<Func<Group, object>>[]
        //    {
        //        r => r.StudentGroups,
        //        r => r.Course!,
        //    };
        //}

        var groups = await groupRepository.GetPagedAsync(
            queryParams: request.QueryParams,
            predicate: filter,
            includes: includes);

        var pagedGroups = groups.Adapt<PagedList<GroupDto>>();
        return Result.Success(pagedGroups);
    }
}