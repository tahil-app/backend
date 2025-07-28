using Tahil.Application.Models;

namespace Tahil.Application.Teachers.Commands;

public record UploadTeacherImageCommand(UserAttachmentModel AttachmentModel) : ICommand<Result<bool>>;

public class UploadTeacherLogoCommandHandler(
    IUnitOfWork unitOfWork,
    IUploadService uploadService,
    ITeacherRepository teacherRepository)
    : ICommandHandler<UploadTeacherImageCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UploadTeacherImageCommand request, CancellationToken cancellationToken)
    {
        var teacher = await teacherRepository.GetAsync(r => r.Id == request.AttachmentModel.UserId);
        if (teacher is null)
            throw new NotFoundException("Teacher");

        var deletedImage = teacher.User.ImagePath;

        var image = request.AttachmentModel.File;
        Guid guid = Guid.NewGuid();
        using var stream = image.OpenReadStream();
        var uploadFolderPath = Path.Combine("wwwroot", "attachments", "teachers", request.AttachmentModel.UserId.ToString());

        var uploadFileName = await uploadService.UploadAsync(uploadFolderPath, guid, $"LOGO-{image.FileName}", stream);

        teacher.User.ChangeImage(uploadFileName);

        var result = await unitOfWork.SaveChangesAsync();

        if (result && deletedImage is not null) 
        {
            var deletedPath = Path.Combine("wwwroot", "attachments", "teachers", teacher.Id.ToString(), deletedImage!);
            await uploadService.DeleteAsync(deletedPath);
        }

        return Result.Success(result);
    }

}