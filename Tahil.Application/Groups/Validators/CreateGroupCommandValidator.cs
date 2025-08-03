using Tahil.Application.Groups.Commands;

namespace Tahil.Application.Groups.Validators;

public class CreateGroupCommandValidator : GroupCommandValidator<CreateGroupCommand>
{
    public CreateGroupCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();
    }
}
