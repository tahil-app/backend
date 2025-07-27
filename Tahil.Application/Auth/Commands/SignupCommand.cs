using Tahil.Application.Auth.Mappings;
using Tahil.Application.Auth.Models;

namespace Tahil.Application.Auth.Commands;

public record SignupCommand(SignupModel Model) : ICommand<Result<bool>>;

public class SignupCommandHandler(IUnitOfWork unitOfWork, ITeacherRepository userRepository) : ICommandHandler<SignupCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        var user = request.Model.ToUser();

        //await userRepository.AddUserAsync(user);
        
        var result = await unitOfWork.SaveChangesAsync();

        return Result.Success(result);
    }
}
