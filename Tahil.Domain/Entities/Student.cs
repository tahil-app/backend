using Tahil.Common.Exceptions;

namespace Tahil.Domain.Entities;

public class Student : Base
{
    public string Qualification { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public ICollection<StudentAttachment> StudentAttachments { get; set; } = new List<StudentAttachment>();
    public ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();

    public void Activate() => User.IsActive = true;
    public void DeActivate() => User.IsActive = false;
    public void Update(StudentDto studentDto)
    {
        var userDto = new UserDto
        {
            Name = studentDto.Name,
            PhoneNumber = studentDto.PhoneNumber,
            Email = studentDto.Email,
            Gender = studentDto.Gender,
            BirthDate = studentDto.BirthDate,
            JoinedDate = studentDto.JoinedDate,
        };

        User.Update(userDto);
        Qualification = studentDto.Qualification;
    }

    public void AddAttachment(Attachment attachment, string displayName) 
    {
        StudentAttachments.Add(new StudentAttachment 
        {
            Attachment = attachment,
            StudentId = Id,
            DisplayName = displayName
        });
    }
    public void RemoveStudentAttachment(int attachmentId) 
    {
        var deletedStudentAttach = StudentAttachments.FirstOrDefault(r => r.AttachmentId == attachmentId);
        if (deletedStudentAttach is null)
            throw new NotFoundException("Attachment");

        StudentAttachments.Remove(deletedStudentAttach!);
    }

    public void AddToGroups(List<Group> groups)
    {
        groups.ForEach(group => StudentGroups.Add(new StudentGroup 
        {
            Group = group,
            StudentId = Id
        }));
    }
    public void RemoveFromGroup(List<int> groupIds)
    {
       var deletedFromGroups = StudentGroups.Where(r => groupIds.Contains(r.GroupId)).ToList();
        deletedFromGroups.ForEach(stGroup => 
        {
            StudentGroups.Remove(stGroup);
        });
    }
}