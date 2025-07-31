using Tahil.Common.Exceptions;
using Tahil.Common.Helpers;
using Tahil.Domain.Enums;
using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class LessonScheduleRepository : Repository<LessonSchedule>, ILessonScheduleRepository
{
    private readonly DbSet<LessonSession> lessonSessionDbSet;
    private readonly DbSet<Room> roomDbSet;
    private readonly DbSet<Course> courseDbSet;
    private readonly DbSet<Teacher> teacherDbSet;
    private readonly LocalizedStrings _localizedStrings;

    public LessonScheduleRepository(BEContext context, LocalizedStrings localizedStrings) : base(context.Set<LessonSchedule>()) 
    {
        lessonSessionDbSet = context.Set<LessonSession>();
        roomDbSet = context.Set<Room>();
        courseDbSet = context.Set<Course>();
        teacherDbSet = context.Set<Teacher>();
        _localizedStrings = localizedStrings;
    }

    public async Task AddLessonScheduleAsync(LessonSchedule schedule)
    {
        await CheckConflictAsync(schedule);
        
        schedule.CreatedAt = Date.Now;
        
        Add(schedule);
    }

    public async Task UpdateLessonScheduleAsync(LessonSchedule schedule)
    {
        await CheckConflictAsync(schedule);

        var hasSessions = await lessonSessionDbSet.AnyAsync(r => r.ScheduleId == schedule.Id);
        if (hasSessions) 
        {
            var oldSchedule = await GetAsync(r => r.Id == schedule.Id);
            oldSchedule!.Status = LessonScheduleStatus.Archived;
         
            await RemoveComingSessionsAsync(oldSchedule);
            
            Update(oldSchedule);
        }

        schedule.ReferenceId = schedule.Id;
        schedule.CreatedAt = Date.Now;
        schedule.Id = 0;
        
        Add(schedule);
    }

    public async Task DeleteLessonScheduleAsync(int id)
    {
        var oldSchedule = await GetAsync(r => r.Id == id);

        var hasSessions = await lessonSessionDbSet.AnyAsync(r => r.ScheduleId == id);
        if (hasSessions)
        {
            oldSchedule!.Status = LessonScheduleStatus.Canceled;

            await RemoveComingSessionsAsync(oldSchedule);

            Update(oldSchedule);
        } 
        else
        {
            HardDelete(oldSchedule!);
        }
    }

    private async Task CheckConflictAsync(LessonSchedule schedule)
    {
        var roomIsValid = await roomDbSet.AnyAsync(r => r.IsActive && r.Id == schedule.RoomId);
        if (!roomIsValid)
            throw new DomainException(_localizedStrings.Room);

        var courseIsValid = await courseDbSet.AnyAsync(r => r.IsActive && r.Id == schedule.CourseId);
        if (!courseIsValid)
            throw new DomainException(_localizedStrings.NotAvailableCourse);

        var teacherIsValid = await teacherDbSet.AnyAsync(r => r.User.IsActive && r.Id == schedule.TeacherId);
        if (!teacherIsValid)
            throw new DomainException(_localizedStrings.NotAvailableTeacher);

        var hasConflict = await AnyAsync(e => 
                e.Day == schedule.Day &&
                e.Id != schedule.Id && // to avoid self-conflict during update
                (
                    e.RoomId == schedule.RoomId ||
                    e.TeacherId == schedule.TeacherId ||
                    e.GroupId == schedule.GroupId
                ) &&
                e.EndTime > schedule.StartTime &&
                e.StartTime < schedule.EndTime
            );

        if (hasConflict)
            throw new DomainException(_localizedStrings.ConflictBusyTime);
    }

    private async Task RemoveComingSessionsAsync(LessonSchedule schedule)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var sessions = await lessonSessionDbSet
            .Where(s => s.ScheduleId == schedule.Id && s.Date >= today)
            .ToListAsync();

        if (sessions.Any())
            lessonSessionDbSet.RemoveRange(sessions);
    }
}