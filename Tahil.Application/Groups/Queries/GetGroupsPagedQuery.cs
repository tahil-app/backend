using System.Linq.Expressions;
using Tahil.Domain.Entities;

namespace Tahil.Application.Groups.Queries;

public record GetGroupsPagedQuery(QueryParams QueryParams) : IQuery<Result<PagedList<GroupDto>>>;

public class GetGroupsPagedQueryHandler(IGroupRepository groupRepository, IApplicationContext applicationContext) : IQueryHandler<GetGroupsPagedQuery, Result<PagedList<GroupDto>>>
{
    public async Task<Result<PagedList<GroupDto>>> Handle(GetGroupsPagedQuery request, CancellationToken cancellationToken)
    {
        Expression<Func<Group, bool>> filter = group => group.TenantId == applicationContext.TenantId;

        if (applicationContext.UserRole == UserRole.Teacher)
        {
            filter = group => group.TeacherId == applicationContext.UserId && group.TenantId == applicationContext.TenantId;
        }

        var groups = await groupRepository.GetPagedProjectionAsync(
            queryParams: request.QueryParams,
            projection: g => new GroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Capacity = g.Capacity,
                NumberOfStudents = g.StudentGroups.Count,
                Course = new LookupDto
                {
                    Id = g.Course!.Id,
                    Name = g.Course.Name
                },
                Teacher = new LookupDto
                {
                    Id = g.Teacher!.Id,
                    Name = g.Teacher.User!.Name
                }
            },
            predicate: filter);

        return Result.Success(groups);
    }
}