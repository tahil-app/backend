using Tahil.Common.Helpers;
using Tahil.Domain.Dtos;
using Tahil.Domain.Enums;
using Tahil.Domain.Localization;
using Tahil.Domain.Services;

namespace Tahil.Infrastructure.Repositories;

public class ClassScheduleRepository : Repository<ClassSchedule>, IClassScheduleRepository
{
    private readonly DbSet<Group> groupDbSet;
    private readonly LocalizedStrings _localizedStrings;
    private readonly IApplicationContext _applicationContext;
    private readonly DbSet<ClassSession> sessionDbSet;

    public ClassScheduleRepository(BEContext context, LocalizedStrings localizedStrings, IApplicationContext applicationContext) : base(context.Set<ClassSchedule>())
    {
        sessionDbSet = context.Set<ClassSession>();
        groupDbSet = context.Set<Group>();
        _localizedStrings = localizedStrings;
        _applicationContext = applicationContext;
    }

    public async Task<List<ClassScheduleDto>> GetMonthySchedulesAsync(int month, int year)
    {
        var monthStart = new DateOnly(year, month, 1);
        var monthEnd = monthStart.AddMonths(1).AddDays(-1);

        Expression<Func<ClassSchedule, bool>> expression = r =>
            r.TenantId == _applicationContext.TenantId &&
            r.StartDate <= monthEnd &&
            (r.EndDate == null || r.EndDate >= monthStart);

        if (_applicationContext.UserRole == UserRole.Teacher)
            expression = r =>
                r.TenantId == _applicationContext.TenantId &&
                r.Group!.TeacherId == _applicationContext.UserId &&
                r.StartDate <= monthEnd &&
                (r.EndDate == null || r.EndDate >= monthStart);

        if (_applicationContext.UserRole == UserRole.Student)
            expression = r =>
                r.TenantId == _applicationContext.TenantId &&
                r.Group!.StudentGroups.Any(r => r.StudentId == _applicationContext.UserId) &&
                r.StartDate <= monthEnd &&
                (r.EndDate == null || r.EndDate >= monthStart);

        return await _dbSet.Where(expression).Select(r => new ClassScheduleDto
        {
            Id = r.Id,
            Day = r.Day,
            GroupId = r.GroupId,
            RoomId = r.RoomId,
            StartDate = r.StartDate,
            EndDate = r.EndDate,
            StartTime = r.StartTime,
            EndTime = r.EndTime,
            Status = r.Status,
            Color = r.Color,
            GroupName = r.Group!.Name,
            CourseName = r.Group.Course!.Name,
            RoomName = r.Room!.Name,
            TeacherName = r.Group.Teacher!.User.Name
        }).AsNoTracking().ToListAsync();
    }

    public async Task<Result<bool>> AddScheduleAsync(ClassSchedule schedule, Guid tenatId)
    {
        var conflictResult = await CheckConflictAsync(schedule);
        if (!conflictResult.IsSuccess)
            return conflictResult;

        schedule.CreatedAt = Date.Now;
        schedule.CreatedBy = _applicationContext.UserName;

        schedule.UpdatedAt = Date.Now;
        schedule.UpdatedBy = _applicationContext.UserName;

        schedule.Status = ClassScheduleStatus.New;
        schedule.TenantId = tenatId;

        Add(schedule);

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> UpdateScheduleAsync(ClassSchedule schedule, Guid tenatId)
    {
        var conflictResult = await CheckConflictAsync(schedule);
        if (!conflictResult.IsSuccess)
            return conflictResult;

        var scheduleToUpdate = await GetAsync(r => r.Id == schedule.Id);
        if (scheduleToUpdate == null)
            return Result<bool>.Failure(_localizedStrings.NotFoundClassSchedule);

        scheduleToUpdate?.Update(schedule, _applicationContext.UserName);

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteScheduleAsync(int id, Guid tenatId)
    {
        var scheduleToDelete = await GetAsync(r => r.Id == id && r.TenantId == tenatId, [s => s.Sessions]);
        if (scheduleToDelete == null)
            return Result<bool>.Failure(_localizedStrings.NotFoundClassSchedule);

        if (scheduleToDelete.Sessions.Any())
            return Result<bool>.Failure(_localizedStrings.ScheduleHasSessions);

        HardDelete(scheduleToDelete);

        return Result<bool>.Success(true);
    }

    public async Task<bool> ExistsInTenantAsync(int id, Guid tenantId)
    {
        return await _dbSet.AnyAsync(c => c.Id == id && c.TenantId == tenantId);
    }

    private async Task<Result<bool>> CheckConflictAsync(ClassSchedule schedule)
    {
        var teacherConflictResult = await CheckTeacherScheduleConflictAsync(schedule);
        if (!teacherConflictResult.IsSuccess)
            return teacherConflictResult;

        var roomOrGroupConflictResult = await CheckRoomOrGroupConflictAsync(schedule);
        if (!roomOrGroupConflictResult.IsSuccess)
            return roomOrGroupConflictResult;

        return Result<bool>.Success(true);
    }
    private async Task<Result<bool>> CheckTeacherScheduleConflictAsync(ClassSchedule schedule)
    {
        var teacherId = await groupDbSet.Where(r => r.Id == schedule.Id).Select(r => r.TeacherId).FirstOrDefaultAsync();

        var hasConflict = await AnyAsync(r =>
            r.Id != schedule.Id &&
            r.Group!.TeacherId == teacherId &&
            r.Day == schedule.Day &&
            r.EndTime > schedule.StartTime &&
            r.StartTime < schedule.EndTime);

        return hasConflict
            ? Result<bool>.Failure(_localizedStrings.TeacherHasAnotherSchedule)
            : Result<bool>.Success(true);
    }
    private async Task<Result<bool>> CheckRoomOrGroupConflictAsync(ClassSchedule schedule)
    {
        var conflict = await GetAsync(e =>
            e.Id != schedule.Id &&
             e.Day == schedule.Day &&
            (
                e.RoomId == schedule.RoomId ||
                e.GroupId == schedule.GroupId
            ) &&
            e.EndTime > schedule.StartTime &&
            e.StartTime < schedule.EndTime);

        if (conflict != null && conflict.RoomId == schedule.RoomId)
        {
            return Result<bool>.Failure(_localizedStrings.RoomIsBusy);
        }
        else if (conflict != null && conflict.GroupId == schedule.GroupId)
        {
            return Result<bool>.Failure(_localizedStrings.GroupIsBusy);
        }
        else if (conflict != null)
        {
            return Result<bool>.Failure(_localizedStrings.ConflictBusyTime);
        }

        return Result<bool>.Success(true);
    }


    private async Task RemoveComingSessionsAsync(ClassSchedule schedule)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var sessions = await sessionDbSet
            .Where(s => s.ScheduleId == schedule.Id && s.Date >= today)
            .ToListAsync();

        if (sessions.Any())
            sessionDbSet.RemoveRange(sessions);
    }


}