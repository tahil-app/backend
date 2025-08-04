using Tahil.Application.Models;

namespace Tahil.Application.Students.Commands;

public record UploadStudentImageCommand(UserAttachmentModel AttachmentModel) : ICommand<Result<bool>>;

public class UploadStudentLogoCommandHandler(
    IUnitOfWork unitOfWork,
    IUploadService uploadService,
    IStudentRepository studentRepository,
    LocalizedStrings locale)
    : ICommandHandler<UploadStudentImageCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UploadStudentImageCommand request, CancellationToken cancellationToken)
    {
        var student = await studentRepository.GetAsync(r => r.Id == request.AttachmentModel.UserId);
        if (student is null)
            return Result<bool>.Failure(locale.NotAvailableStudent);

        var deletedImage = student.User.ImagePath;

        var image = request.AttachmentModel.File;
        Guid guid = Guid.NewGuid();
        using var stream = image.OpenReadStream();
        var uploadFolderPath = Path.Combine("wwwroot", "attachments", "students", request.AttachmentModel.UserId.ToString());

        var uploadFileName = await uploadService.UploadAsync(uploadFolderPath, guid, $"LOGO-{image.FileName}", stream);

        student.User.ChangeImage(uploadFileName);

        var result = await unitOfWork.SaveChangesAsync();

        if (result && deletedImage is not null)
        {
            var deletedPath = Path.Combine("wwwroot", "attachments", "students", student.Id.ToString(), deletedImage!);
            await uploadService.DeleteAsync(deletedPath);
        }

        return Result.Success(result);
    }

}