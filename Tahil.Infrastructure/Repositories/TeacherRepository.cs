using Mapster;
using MediatR;
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

    public async Task<TeacherDto> GetTeacherAsync(int id, Guid tenantId)
    {
        var query = _dbSet.Where(r => r.User.IsActive && r.Id == id && r.User.TenantId == tenantId)
            .Select(r => new TeacherDto
            {
                Id = r.Id,
                Name = r.User.Name,
                Email = r.User.Email.Value,
                PhoneNumber = r.User.PhoneNumber,
                IsActive = r.User.IsActive,
                BirthDate = r.User.BirthDate,
                JoinedDate = r.User.JoinedDate,
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
                Groups = r.Groups.Select(gr => new GroupDto 
                {
                    Id = gr.Id,
                    Name = gr.Name
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

    public async Task<string> GetAttachmentDisplayNameAsync(string attachmentName, Guid tenantId)
    {
        return await _dbSet
            .SelectMany(r => r.TeacherAttachments)
            .Where(a => a.Attachment.FileName == attachmentName && a.Attachment.TenantId == tenantId)
            .Select(a => a.DisplayName)
            .FirstOrDefaultAsync() ?? string.Empty;
    }

    public async Task<Result<bool>> AddTeacherAsync(Teacher teacher, Guid tenantId)
    {
        var result = await CheckDuplicateTeacherAsync(teacher.Id, teacher.User.Email.Value, teacher.User.PhoneNumber, teacher.User.Password, tenantId);

        if (result.IsSuccess)
        {
            teacher.User.TenantId = tenantId;
            teacher.User.IsActive = true;
            Add(teacher);
        }

        return result;
    }

    public async Task<Result<bool>> UpdateTeacherAsync(TeacherDto teacherDto, Guid tenantId)
    {
        var teacher = await GetAsync(r => r.Id == teacherDto.Id && r.User.TenantId == tenantId, [r => r.User, r => r.TeacherCourses, r => r.Groups]);
        if (teacher is null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableTeacher);


        if (teacher.User.Email.Value != teacherDto.Email || teacher.User.PhoneNumber != teacherDto.PhoneNumber) 
        {
            var existTeacher = await CheckDuplicateTeacherAsync(teacherDto.Id, teacherDto.Email, teacherDto.PhoneNumber, teacherDto.Password, tenantId);
            if (!existTeacher.IsSuccess)
                return existTeacher;
        }


        teacher.Update(teacherDto);

        if (teacherDto.Password is not null && !string.IsNullOrEmpty(teacherDto.Password))
            teacher.User.UpdatePassword(teacherDto.Password);

        if (teacherDto.Courses is not null)
            teacher.UpdateCourses(teacherDto.Courses.Adapt<List<Course>>());

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteTeacherAsync(int id, Guid tenantId)
    {
        var teacher = await GetAsync(t => t.Id == id && t.User.TenantId == tenantId, [t => t.TeacherCourses, t => t.TeacherAttachments]);

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

    public async Task<bool> ExistsInTenantAsync(int id, Guid tenantId)
    {
        return await _dbSet.AnyAsync(c => c.Id == id && c.User.TenantId == tenantId);
    }

    private async Task<Result<bool>> CheckDuplicateTeacherAsync(int teacherId, string email, string phone, string password, Guid tenantId)
    {
        var existTeacher = await GetAsync(t => t.User.TenantId == tenantId && t.User.Email.Value == email || t.User.PhoneNumber == phone, [r => r.User]);

        // Check if email is duplicated
        if (existTeacher is not null && existTeacher.User.Email.Value == email && (existTeacher.Id != teacherId || teacherId == 0))
            return Result<bool>.Failure(_localizedStrings.DuplicatedEmail);

        // Check if phone number is duplicated
        if (existTeacher is not null && existTeacher.User.PhoneNumber == phone && (existTeacher.Id != teacherId || teacherId == 0))
            return Result<bool>.Failure(_localizedStrings.DuplicatedPhoneNumber);

        return Result<bool>.Success(true);
    }
}