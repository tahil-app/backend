namespace Tahil.Domain.Repositories;

public interface ILessonScheduleRepository : IRepository<LessonSchedule>
{
    Task UpdateLessonScheduleAsync(LessonSchedule lessonSchedule);
    Task DeleteLessonScheduleAsync(int id);
}