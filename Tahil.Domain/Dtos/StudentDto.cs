namespace Tahil.Domain.Dtos;

public class StudentDto : UserDto
{
    public string Qualification { get; set; } = string.Empty;

    public List<DailyScheduleDto> DailySchedules { get; set; } = new();
    public List<AttachmentDto> Attachments { get; set; } = new();
    public List<FeedbackDto> Feedbacks { get; set; } = new();
    public List<GroupDto>? Groups { get; set; }
}
