namespace Tahil.Domain.Entities;

public class Teacher : Base
{
    public string Experience { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public ICollection<TeacherAttachment> TeacherAttachments { get; set; } = new List<TeacherAttachment>();

    public void Activate() => User.IsActive = true;
    public void DeActivate() => User.IsActive = false;
    public void Update(TeacherDto teacherDto)
    {
        var userDto = new UserDto
        {
            Name = teacherDto.Name,
            PhoneNumber = teacherDto.PhoneNumber,
            Email = teacherDto.Email,
            Gender = teacherDto.Gender,
            BirthDate = teacherDto.BirthDate,
            JoinedDate = teacherDto.JoinedDate,
        };

        User.Update(userDto);
        Experience = teacherDto.Experience;
        Qualification = teacherDto.Qualification;
    }

    public void AddTeacherAttachment(Attachment attachment) 
    {
        TeacherAttachments.Add(new TeacherAttachment 
        {
            AttachmentId = attachment.Id,
            TeacherId = Id
        });
    }
    public void RemoveTeacherAttachment(Attachment attachment) 
    {
        var deletedAttach = TeacherAttachments.FirstOrDefault(r => r.AttachmentId == attachment.Id);
        TeacherAttachments.Remove(deletedAttach!);
    }
}