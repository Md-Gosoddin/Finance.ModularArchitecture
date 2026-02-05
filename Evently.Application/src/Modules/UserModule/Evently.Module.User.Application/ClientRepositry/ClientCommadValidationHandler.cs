using FluentValidation;

namespace Evently.Module.User.Application.ClientRepositry;
internal sealed class ClientCommadValidationHandler : AbstractValidator<CreateCommnadHandlerRequest>
{
    public ClientCommadValidationHandler()
    {
        RuleFor(c => c.UserName).NotEmpty().MinimumLength(5);
        RuleFor(c => c.Email).NotEmpty();
        RuleFor(c => c.Identity).NotEmpty();
    }
}
