//using Tahil.Application.Users.Commands;

//namespace Tahil.Application.Users.Validators;

//public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
//{
//    public CreateUserCommandValidator()
//    {
//        RuleFor(x => x.User)
//            .NotNull();

//        RuleFor(x => x.User.Name).NotNull();
//        RuleFor(x => x.User.Email).NotNull();
//        RuleFor(x => x.User.PhoneNumber).NotNull();
//        RuleFor(x => x.User.Password).NotNull();
//        RuleFor(x => x.User.Role).NotNull();
//    }
//}