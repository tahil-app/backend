namespace Tahil.Domain.Repositories;

public interface ILessonScheduleRepository : IRepository<LessonSchedule>
{
    Task AddLessonScheduleAsync(LessonSchedule lessonSchedule);
    Task UpdateLessonScheduleAsync(LessonSchedule lessonSchedule);
    Task DeleteLessonScheduleAsync(int id);
}