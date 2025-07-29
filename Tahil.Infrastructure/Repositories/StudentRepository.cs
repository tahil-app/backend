using Tahil.Common.Exceptions;
using Tahil.Domain.Dtos;

namespace Tahil.Infrastructure.Repositories;

public class StudentRepository : Repository<Student>, IStudentRepository
{
    public StudentRepository(BEContext context) : base(context.Set<Student>())
    {
    }

    public async Task<StudentDto> GetStudentAsync(int id)
    {
        var query = _dbSet.Where(r => r.User.IsActive && r.Id == id)
            .Select(r => new StudentDto
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
                Qualification = r.Qualification,
                ImagePath = r.User.ImagePath,
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

    public async Task<string> GetAttachmentDisplayNameAsync(string attachmentName)
    {
        return await _dbSet
            .SelectMany(r => r.StudentAttachments)
            .Where(a => a.Attachment.FileName == attachmentName)
            .Select(a => a.DisplayName)
            .FirstOrDefaultAsync() ?? string.Empty;
    }

    public async Task AddStudentAsync(Student student)
    {
        await CheckExistsAsync(student);

        student.User.IsActive = true;
        Add(student);
    }

    private async Task CheckExistsAsync(Student student)
    {
        var existStudent = await GetAsync(u => u.User.Email.Value == student.User.Email.Value || u.User.PhoneNumber == student.User.PhoneNumber);

        if (existStudent is not null && existStudent.User.Email.Value == student.User.Email.Value)
            throw new DuplicateException("Email");

        if (existStudent is not null && existStudent.User.PhoneNumber == student.User.PhoneNumber)
            throw new DuplicateException("Phone Number");
    }

}