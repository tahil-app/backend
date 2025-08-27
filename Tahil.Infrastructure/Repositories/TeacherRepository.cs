using Mapster;
using MediatR;
using Tahil.Domain.Dtos;
using Tahil.Domain.Entities;
using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class TeacherRepository : Repository<Teacher>, ITeacherRepository
{
    private readonly LocalizedStrings _localizedStrings;
    private readonly BEContext _context;
    public TeacherRepository(BEContext context, LocalizedStrings localizedStrings) : base(context.Set<Teacher>())
    {
        _localizedStrings = localizedStrings;
        _context = context;
    }

    public async Task<TeacherDto> GetTeacherAsync(int id, Guid tenantId)
    {
        var query = _dbSet.Where(r => r.User.IsActive && r.Id == id && r.User.TenantId == tenantId)
            .Select(r => new TeacherDto
            {
                Id = r.Id,
                Code = $"T_{r.Id}",
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

    public async Task<List<GroupDto>> GetTeacherGroupsAsync(int id, Guid tenantId)
    {
        return await _dbSet.Where(r => r.User.IsActive && r.Id == id && r.User.TenantId == tenantId)
                .SelectMany(r => r.Groups.Select(gr => new GroupDto
                {
                    Id = gr.Id,
                    Name = gr.Name,
                    CourseName = gr.Course!.Name,
                    NumberOfStudents = gr.StudentGroups.Count
                }).ToList()).ToListAsync();
    }

    public async Task<List<DailyScheduleDto>> GetTeacherSchedulesAsync(int id, Guid tenantId)
    {
        var result = await _dbSet.Where(r => r.User.IsActive && r.Id == id && r.User.TenantId == tenantId)
            .SelectMany(r => r.Groups)
            .SelectMany(g => g.Schedules)
            .Select(s => new DailyScheduleDto
            {
                Day = s.Day,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                RoomName = s.Room!.Name,
                GroupName = s.Group!.Name,
                CourseName = s.Group!.Course!.Name,
            })
            .OrderBy(s => s.StartTime)
            .ToListAsync();

        return result.OrderBy(r => r.Day).ToList();
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
        {

            var groupCourseIds = teacher.Groups.Select(g => g.CourseId).Distinct().ToList();
            var missingCourses = groupCourseIds.Except(teacherDto.Courses.Select(r => r.Id)).ToList();
            if (missingCourses.Any())
                return Result<bool>.Failure(_localizedStrings.TeacherAndCourseHasGroup);

            teacher.UpdateCourses(teacherDto.Courses.Adapt<List<Course>>());
        }

        return Result<bool>.Success(true);
    }

    public async Task<Result<bool>> DeleteTeacherAsync(int id, Guid tenantId)
    {
        // Check if room exists and has any child relationships in a single query
        var teacherWithRelationships = await _dbSet
            .Where(r => r.Id == id && r.User.TenantId == tenantId)
            .Select(r => new
            {
                Teacher = r,
                HasTeacherAttachments = r.TeacherAttachments.Any(),
                HasTeacherCourses = r.TeacherCourses.Any()
            })
            .FirstOrDefaultAsync();

        if (teacherWithRelationships is null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableTeacher);

        // Check if teacher has child relationships
        if (teacherWithRelationships.HasTeacherCourses)
            return Result<bool>.Failure(_localizedStrings.TeacherHasCourses);

        if (teacherWithRelationships.HasTeacherAttachments)
            return Result<bool>.Failure(_localizedStrings.TeacherHasAttachments);

        // If no child relationships exist, proceed with deletion
        HardDelete(teacherWithRelationships.Teacher);
        _context.Remove(teacherWithRelationships.Teacher.User);
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