using Tahil.Common.Helpers;
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

        //var hasSessions = await lessonSessionDbSet.AnyAsync(r => r.ScheduleId == schedule.Id);
        //if (hasSessions)
        //{
        //    var oldSchedule = await GetAsync(r => r.Id == schedule.Id);
        //    oldSchedule!.Status = ClassScheduleStatus.Archived;

        //    await RemoveComingSessionsAsync(oldSchedule);

        //    Update(oldSchedule);
        //}

        //schedule.ReferenceId = schedule.Id;
        //schedule.CreatedAt = Date.Now;
        //schedule.Id = 0;

        //Add(schedule);

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteScheduleAsync(int id, Guid tenatId)
    {
        //var oldSchedule = await GetAsync(r => r.Id == id);

        //var hasSessions = await lessonSessionDbSet.AnyAsync(r => r.ScheduleId == id);
        //if (hasSessions)
        //{
        //    oldSchedule!.Status = ClassScheduleStatus.Canceled;

        //    await RemoveComingSessionsAsync(oldSchedule);

        //    Update(oldSchedule);
        //}
        //else
        //{
        //    HardDelete(oldSchedule!);
        //}

        return Result<bool>.Success(true);
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
        else if(conflict != null)
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