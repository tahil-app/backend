using Tahil.Common.Exceptions;
using Tahil.Domain.Dtos;

namespace Tahil.Infrastructure.Repositories;

public class TeacherRepository : Repository<Teacher>, ITeacherRepository
{
    public TeacherRepository(BEContext context) : base(context.Set<Teacher>())
    {
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
                BirthDate = r.User.BirthDate,
                JoinedDate = r.User.JoinedDate,
                Gender = r.User.Gender,
                Role = r.User.Role,
                Experience = r.Experience,
                Qualification = r.Qualification,
                ImagePath = r.User.ImagePath,
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

    public async Task AddTeacherAsync(Teacher teacher)
    {
        await CheckExistsAsync(teacher);

        teacher.User.IsActive = true;
        Add(teacher);
    }

    private async Task CheckExistsAsync(Teacher teacher)
    {
        var existTeacher = await GetAsync(u => u.User.Email.Value == teacher.User.Email.Value || u.User.PhoneNumber == teacher.User.PhoneNumber);

        if (existTeacher is not null && existTeacher.User.Email.Value == teacher.User.Email.Value)
            throw new DuplicateException("Email");

        if (existTeacher is not null && existTeacher.User.PhoneNumber == teacher.User.PhoneNumber)
            throw new DuplicateException("Phone Number");
    }

}