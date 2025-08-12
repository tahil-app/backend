namespace Tahil.Domain.Repositories;

public interface IClassScheduleRepository : IRepository<ClassSchedule>
{
    Task<Result<bool>> AddScheduleAsync(ClassSchedule schedule, Guid tenatId);
    Task<Result<bool>> UpdateScheduleAsync(ClassSchedule schedule, Guid tenatId);
    Task<Result<bool>> DeleteScheduleAsync(int id, Guid tenatId);
    Task<List<ClassScheduleDto>> GetMonthySchedulesAsync(int month, int year);
    Task<bool> ExistsInTenantAsync(int? id, Guid? tenantId);
}