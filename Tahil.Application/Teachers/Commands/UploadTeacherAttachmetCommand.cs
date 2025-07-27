using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.X509;

namespace Tahil.Application.Teachers.Commands;

public record UploadTeacherAttachmetCommand(int TeacherId, string Title, IFormFile File) : ICommand<Result<bool>>;

public class UploadTeacherAttachmetCommandHandler(
    IUnitOfWork unitOfWork,
    IUploadService uploadService,
    IAttachmentRepository attachmentRepository,
    IApplicationContext applicationContext,
    ITeacherRepository teacherRepository)
    : ICommandHandler<UploadTeacherAttachmetCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UploadTeacherAttachmetCommand request, CancellationToken cancellationToken)
    {
        var teacher = await teacherRepository.GetAsync(r => r.Id == request.TeacherId, [r => r.TeacherAttachments]);
        if (teacher is null)
            throw new NotFoundException("Teacher");

        Guid guid = Guid.NewGuid();
        using var stream = request.File.OpenReadStream();
        var uploadFolder = Path.Combine("wwwroot", "attachments", "teachers", request.TeacherId.ToString());
        var extension = Path.GetExtension(request.File.FileName);

        var fileName = await uploadService.UploadAsync(uploadFolder, guid, request.Title, extension, stream);

        var attachment = attachmentRepository.AddAttachment(new AttachmentDto
        {
            FileId = guid,
            FileName = $"{fileName}-{guid}{extension}",
            FileSize = stream.Length,
            MimeType = request.File.ContentType
        }, applicationContext.UserName);

        teacher.AddAttachment(attachment);

        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }

}