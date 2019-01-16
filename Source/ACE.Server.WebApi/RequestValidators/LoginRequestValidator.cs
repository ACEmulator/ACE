using ACE.Server.WebApi.Requests;
using FluentValidation;

namespace ACE.Server.WebApi.RequestValidators
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
