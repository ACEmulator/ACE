using ACE.Server.Managers;
using ACE.Server.WebApi.Requests;
using FluentValidation;

namespace ACE.Server.WebApi.RequestValidators
{
    public class DownloadCharacterValidator : AbstractValidator<DownloadCharacterRequest>
    {
        public DownloadCharacterValidator()
        {
            RuleFor(request => request.Cookie).NotEmpty().WithMessage("You must specify the cookie.");
            RuleFor(request => request.Cookie).Length(TransferManager.CookieLength).WithMessage($"The cookie must be {TransferManager.CookieLength} characters long.");
            RuleFor(request => request.Cookie).Custom((str, _) =>
            {
                if (TransferManager.CookieContainsInvalidChars(str))
                {
                    _.AddFailure("The cookie contains invalid characters.");
                }
            });
        }
    }
}
