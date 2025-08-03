using Tahil.Application.Groups.Commands;

namespace Tahil.Application.Groups.Validators;

public class UpdateGroupCommandValidator : GroupCommandValidator<UpdateGroupCommand>
{
    public UpdateGroupCommandValidator(LocalizedStrings locale) : base(locale)
    {
        AddCommonRules();
    }
}