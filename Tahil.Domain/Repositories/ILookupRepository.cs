namespace Tahil.Domain.Repositories;

public interface ILookupRepository
{
    Task<LessonScheduleLookupsDto> GetLessonScheduleLookupsAsync();
}