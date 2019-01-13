using ACE.Server.Web.Requests;
using FluentValidation;

namespace ACE.Server.Web.RequestValidators
{
    public class LoginRequestValidator : AbstractValidator<LoginModel>
    {
        public LoginRequestValidator()
        {
            RuleFor(request => request.username).NotEmpty().WithMessage("You must specify your username.");
            RuleFor(request => request.password).NotEmpty().WithMessage("You must specify your password.");
        }
    }
}
