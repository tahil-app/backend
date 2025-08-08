using Tahil.Domain.Dtos;
using Tahil.Domain.Localization;

namespace Tahil.Infrastructure.Repositories;

public class StudentRepository : Repository<Student>, IStudentRepository
{
    private readonly LocalizedStrings _localizedStrings;
    public StudentRepository(BEContext context, LocalizedStrings localizedStrings) : base(context.Set<Student>())
    {
        _localizedStrings = localizedStrings;
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
                Groups = r.StudentGroups.Select(g => new GroupDto 
                {
                    Id = g.Group.Id,
                    Name = g.Group.Name
                }).ToList(),
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
        var student = await GetAsync(s => s.Id == id && s.User.TenantId == tenantId, [s => s.StudentGroups, s => s.StudentAttachments]);

        if (student is null)
            return Result<bool>.Failure(_localizedStrings.NotAvailableStudent);

        // Check if student has child relationships
        if (student.StudentGroups.Any())
            return Result<bool>.Failure(_localizedStrings.StudentHasGroups);

        if (student.StudentAttachments.Any())
            return Result<bool>.Failure(_localizedStrings.StudentHasAttachments);

        // If no child relationships exist, proceed with deletion
        HardDelete(student);
        return Result<bool>.Success(true);
    }

    public async Task<bool> ExistsInTenantAsync(int? id, Guid? tenantId)
    {
        if (!id.HasValue || !tenantId.HasValue)
            return false;

        return await _dbSet.AnyAsync(c => c.Id == id.Value && c.User.TenantId == tenantId.Value);
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