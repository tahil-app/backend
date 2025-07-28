namespace Tahil.Application.Services;

public class AttachmentService(ITeacherRepository teacherRepository) : IAttachmentService
{
    public async Task<string> GetAttachmentDisplayNameAsync(string attachmentUserType, string attachmentName)
    {
        var displatName = "";

        switch (attachmentUserType)
        {
            case "Teachers":
                displatName = await teacherRepository.GetAttachmentDisplayNameAsync(attachmentName);
                break;
        }

        if (string.IsNullOrEmpty(displatName))
            throw new NotFoundException("Attachment");

        return displatName;
    }
}