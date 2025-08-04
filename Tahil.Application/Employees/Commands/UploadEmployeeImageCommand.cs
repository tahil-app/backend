using Tahil.Application.Models;

namespace Tahil.Application.Employees.Commands;

public record UploadEmployeeImageCommand(UserAttachmentModel AttachmentModel) : ICommand<Result<bool>>;

public class UploadEmployeeLogoCommandHandler(
    IUnitOfWork unitOfWork,
    IUploadService uploadService,
    IUserRepository userRepository,
    LocalizedStrings locale)
    : ICommandHandler<UploadEmployeeImageCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(UploadEmployeeImageCommand request, CancellationToken cancellationToken)
    {
        var employee = await userRepository.GetAsync(r => r.Id == request.AttachmentModel.UserId);
        if (employee is null)
            return Result<bool>.Failure(locale.NotAvailableEmployee);

        var deletedImage = employee.ImagePath;

        var image = request.AttachmentModel.File;
        Guid guid = Guid.NewGuid();
        using var stream = image.OpenReadStream();
        var uploadFolderPath = Path.Combine("wwwroot", "attachments", "employees", request.AttachmentModel.UserId.ToString());

        var uploadFileName = await uploadService.UploadAsync(uploadFolderPath, guid, $"LOGO-{image.FileName}", stream);

        employee.ChangeImage(uploadFileName);

        var result = await unitOfWork.SaveChangesAsync();

        if (result && deletedImage is not null)
        {
            var deletedPath = Path.Combine("wwwroot", "attachments", "employees", employee.Id.ToString(), deletedImage!);
            await uploadService.DeleteAsync(deletedPath);
        }

        return Result.Success(result);
    }

}