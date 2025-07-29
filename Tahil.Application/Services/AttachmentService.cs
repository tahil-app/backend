namespace Tahil.Application.Services;

public class AttachmentService(ITeacherRepository teacherRepository, IStudentRepository studentRepository) : IAttachmentService
{
    public async Task<string> GetAttachmentDisplayNameAsync(string attachmentUserType, string attachmentName)
    {
        var displatName = "";

        switch (attachmentUserType)
        {
            case "Teachers":
                displatName = await teacherRepository.GetAttachmentDisplayNameAsync(attachmentName);
                break;

            case "Students":
                displatName = await studentRepository.GetAttachmentDisplayNameAsync(attachmentName);
                break;
        }

        if (string.IsNullOrEmpty(displatName))
            throw new NotFoundException("Attachment");

        return displatName;
    }
}