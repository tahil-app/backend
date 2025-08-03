using Tahil.Common.Exceptions;

namespace Tahil.Domain.Entities;

public class Teacher : Base
{
    public string Experience { get; set; } = string.Empty;
    public string Qualification { get; set; } = string.Empty;

    public int UserId { get; set; }
    public User User { get; set; } = default!;

    public ICollection<TeacherAttachment> TeacherAttachments { get; set; } = new List<TeacherAttachment>();
    public ICollection<LessonSchedule> Schedules { get; set; } = new List<LessonSchedule>();
    public ICollection<LessonSession> Sessions { get; set; } = new List<LessonSession>();
    public ICollection<TeacherCourse> TeacherCourses { get; set; } = new List<TeacherCourse>();

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

    public void AddAttachment(Attachment attachment, string displayName) 
    {
        TeacherAttachments.Add(new TeacherAttachment 
        {
            Attachment = attachment,
            TeacherId = Id,
            DisplayName = displayName
        });
    }
    public void RemoveTeacherAttachment(int attachmentId) 
    {
        var deletedTeacherAttach = TeacherAttachments.FirstOrDefault(r => r.AttachmentId == attachmentId);
        TeacherAttachments.Remove(deletedTeacherAttach!);
    }

    public void UpdateCourses(List<Course> courses)
    {
        var coursesToAdd = courses.Where(r => !TeacherCourses.Select(s => s.CourseId).Contains(r.Id)).ToList();
        foreach (var course in coursesToAdd)
            TeacherCourses.Add(new TeacherCourse
            {
                CourseId = course.Id,
                TeacherId = Id
            });

        var coursesToRemove = TeacherCourses.Where(sg => !courses.Select(g => g.Id).Contains(sg.CourseId)).ToList();
        foreach (var course in coursesToRemove)
            TeacherCourses.Remove(course);

        if (courses.Count == 0)
            TeacherCourses.Clear();
    }
}