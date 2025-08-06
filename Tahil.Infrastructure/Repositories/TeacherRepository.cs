using Tahil.Domain.Dtos;
using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class TeacherRepository : Repository<Teacher>, ITeacherRepository
{
    private readonly LocalizedStrings _localizedStrings;
    public TeacherRepository(BEContext context, LocalizedStrings localizedStrings) : base(context.Set<Teacher>())
    {
        _localizedStrings = localizedStrings;
    }

    public async Task<TeacherDto> GetTeacherAsync(int id)
    {
        var query = _dbSet.Where(r => r.User.IsActive && r.Id == id)
            .Select(r => new TeacherDto
            {
                Id = r.Id,
                Name = r.User.Name,
                Email = r.User.Email.Value,
                PhoneNumber = r.User.PhoneNumber,
                IsActive = r.User.IsActive,
                BirthDate = r.User.BirthDate.HasValue ? r.User.BirthDate.Value : default,
                JoinedDate = r.User.JoinedDate.HasValue ? r.User.JoinedDate.Value : default,
                Gender = r.User.Gender,
                Role = r.User.Role,
                Experience = r.Experience,
                Qualification = r.Qualification,
                ImagePath = r.User.ImagePath,
                Courses = r.TeacherCourses.Select(cr => new CourseDto 
                {
                    Id = cr.Course.Id,
                    Name = cr.Course.Name
                }).ToList(),
                Attachments = r.TeacherAttachments.Select(at => new AttachmentDto
                {
                    Id = at.Attachment.Id,
                    FileName = at.Attachment.FileName,
                    DisplayName = at.DisplayName,
                    CreatedOn = at.Attachment.CreatedOn
                }).ToList(),
            });

        return await query.FirstOrDefaultAsync() ?? new();
    }

    public async Task<string> GetAttachmentDisplayNameAsync(string attachmentName)
    {
        return await _dbSet
            .SelectMany(r => r.TeacherAttachments)
            .Where(a => a.Attachment.FileName == attachmentName)
            .Select(a => a.DisplayName)
            .FirstOrDefaultAsync() ?? string.Empty;
    }

    public async Task<Result<bool>> AddTeacherAsync(Teacher teacher, Guid tenantId)
    {
        var result = await CheckDuplicateTeacherAsync(teacher);

        if (result.IsSuccess)
        {
            teacher.User.TenantId = tenantId;
            teacher.User.IsActive = true;
            Add(teacher);
        }

        return result;
    }

    public async Task<Result<bool>> DeleteTeacherAsync(int id)
    {
        var teacher = await GetAsync(t => t.Id == id, [t => t.TeacherCourses, t => t.TeacherAttachments]);

        if (teacher is null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableTeacher);

        // Check if teacher has child relationships
        if (teacher.TeacherCourses.Any())
            return Result<bool>.Failure(_localizedStrings.TeacherHasCourses);

        if (teacher.TeacherAttachments.Any())
            return Result<bool>.Failure(_localizedStrings.TeacherHasAttachments);

        // If no child relationships exist, proceed with deletion
        HardDelete(teacher);
        return Result<bool>.Success(true);
    }

    private async Task<Result<bool>> CheckDuplicateTeacherAsync(Teacher teacher) 
    {
        var existTeacher = await GetAsync(t => t.User.Email.Value == teacher.User.Email.Value || t.User.PhoneNumber == teacher.User.PhoneNumber);

        // Check if email is duplicated
        if (existTeacher is not null && existTeacher.User.Email.Value == teacher.User.Email.Value)
            return Result<bool>.Failure(_localizedStrings.DuplicatedEmail);

        // Check if phone number is duplicated
        if (existTeacher is not null && existTeacher.User.PhoneNumber == teacher.User.PhoneNumber)
            return Result<bool>.Failure(_localizedStrings.DuplicatedPhoneNumber);

        return Result<bool>.Success(true);
    }
}