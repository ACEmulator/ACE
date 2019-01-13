using ACE.Server.Web.Requests;
using FluentValidation;

namespace ACE.Server.Web.RequestValidators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(request => request.User).NotEmpty().WithMessage("You must specify your username.");
            RuleFor(request => request.Pass).NotEmpty().WithMessage("You must specify your password.");
        }
    }
}
