using Tahil.Domain.Dtos;
using Tahil.Domain.Helpers;
using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class StudentRepository : Repository<Student>, IStudentRepository
{
    private readonly LocalizedStrings _localizedStrings;
    private readonly BEContext _context;
    public StudentRepository(BEContext context, LocalizedStrings localizedStrings) : base(context.Set<Student>())
    {
        _localizedStrings = localizedStrings;
        _context = context;
    }

    public async Task<StudentDto> GetStudentAsync(int id, Guid tenantId)
    {
        var query = _dbSet.Where(r => r.User.IsActive && r.Id == id && r.User.TenantId == tenantId)
            .Select(r => new StudentDto
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
                Qualification = r.Qualification,
                ImagePath = r.User.ImagePath,
                Code = r.Id.GetStudentCode(),
                Attachments = r.StudentAttachments.Select(at => new AttachmentDto
                {
                    Id = at.Attachment.Id,
                    FileName = at.Attachment.FileName,
                    DisplayName = at.DisplayName,
                    CreatedOn = at.Attachment.CreatedOn
                }).ToList(),
            });

        return await query.FirstOrDefaultAsync() ?? new();
    }

    public async Task<List<GroupDto>> GetStudentGroupsAsync(int id, Guid tenantId)
    {
        return await _dbSet.Where(s => s.User.IsActive && s.Id == id && s.User.TenantId == tenantId)
            .SelectMany(s => s.StudentGroups)
            .Select(gr => new GroupDto
            {
                Id = gr.GroupId,
                Name = gr.Group.Name,
                CourseName = gr.Group.Course!.Name,
                NumberOfStudents = gr.Group.StudentGroups.Count
            })
            .ToListAsync();
    }

    public async Task<List<DailyScheduleDto>> GetStudentSchedulesAsync(int id, Guid tenantId)
    {
        var result = await _dbSet.Where(s => s.User.IsActive && s.Id == id && s.User.TenantId == tenantId)
            .SelectMany(s => s.StudentGroups)
            .SelectMany(sg => sg.Group.Schedules)
            .Select(sc => new DailyScheduleDto
            {
                Day = sc.Day,
                StartTime = sc.StartTime,
                EndTime = sc.EndTime,
                RoomName = sc.Room!.Name,
                GroupName = sc.Group!.Name,
                CourseName = sc.Group!.Course!.Name,
                TeacherName = sc.Group!.Teacher!.User!.Name,
            })
            .OrderBy(sc => sc.StartTime)
            .ToListAsync();

        return result.OrderBy(r => r.Day).ToList();
    }

    public async Task<List<FeedbackDto>> GetStudentFeedbacksAsync(int id, int year, int month, Guid tenantId)
    {
        var result = await _dbSet.Where(s => 
            s.User.IsActive && 
            s.Id == id && 
            s.StudentAttendances.Any(sa => sa.Session!.Date.Month == month) &&
            s.StudentAttendances.Any(sa => sa.Session!.Date.Year == year) &&
            s.User.TenantId == tenantId)
            .SelectMany(s => s.StudentAttendances
                .Where(a => !string.IsNullOrEmpty(a.Note))
                .Select(a => new FeedbackDto
                {
                    Name = a.CreatedBy,
                    Comment = a.Note,
                    Date = a.CreatedAt.ToString()
                }))
            .ToListAsync();

        return result.OrderByDescending(r => r.Date).ToList();
    }

    public async Task<string> GetAttachmentDisplayNameAsync(string attachmentName, Guid tenantId)
    {
        return await _dbSet
            .SelectMany(r => r.StudentAttachments)
            .Where(a => a.Attachment.FileName == attachmentName && a.Attachment.TenantId == tenantId)
            .Select(a => a.DisplayName)
            .FirstOrDefaultAsync() ?? string.Empty;
    }

    public async Task<Result<bool>> AddStudentAsync(Student student, Guid tenantId)
    {
        var result = await CheckDuplicateStudentAsync(student, tenantId);

        if (result.IsSuccess)
        {
            student.User.TenantId = tenantId;
            student.User.IsActive = true;
            Add(student);
        }

        return result;
    }

    public async Task<Result<bool>> DeleteStudentAsync(int id, Guid tenantId)
    {
        // Check if student exists and has any child relationships in a single query
        var studentWithRelationships = await _dbSet
            .Where(r => r.Id == id && r.User.TenantId == tenantId)
            .Select(r => new
            {
                Student = r,
                HasStudentGroups = r.StudentGroups.Any(),
                HasStudentAttachments = r.StudentAttachments.Any()
            })
            .FirstOrDefaultAsync();

        if (studentWithRelationships is null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableStudent);

        // Check if student has child relationships
        if (studentWithRelationships.HasStudentGroups)
            return Result<bool>.Failure(_localizedStrings.StudentHasGroups);

        if (studentWithRelationships.HasStudentAttachments)
            return Result<bool>.Failure(_localizedStrings.StudentHasAttachments);

        // If no child relationships exist, proceed with deletion
        HardDelete(studentWithRelationships.Student);
        _context.Remove(studentWithRelationships.Student.User);

        return Result<bool>.Success(true);
    }

    public async Task<bool> ExistsInTenantAsync(int id, Guid tenantId)
    {
        return await _dbSet.AnyAsync(c => c.Id == id && c.User.TenantId == tenantId);
    }

    private async Task<Result<bool>> CheckDuplicateStudentAsync(Student student, Guid tenantId)
    {
        var existStudent = await GetAsync(s => (s.User.Email.Value == student.User.Email.Value || s.User.PhoneNumber == student.User.PhoneNumber) && s.User.TenantId == tenantId);

        // Check if email is duplicated
        if (existStudent is not null && existStudent.User.Email.Value == student.User.Email.Value)
            return Result<bool>.Failure(_localizedStrings.DuplicatedEmail);

        // Check if phone number is duplicated
        if (existStudent is not null && existStudent.User.PhoneNumber == student.User.PhoneNumber)
            return Result<bool>.Failure(_localizedStrings.DuplicatedPhoneNumber);

        return Result<bool>.Success(true);
    }
}