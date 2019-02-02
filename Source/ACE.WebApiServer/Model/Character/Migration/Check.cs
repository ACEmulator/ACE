using ACE.Server.Managers;
using FluentValidation;

namespace ACE.WebApiServer.Model.Character.Migration
{
    public class TransferManagerMigrationCheckRequestModelValidator : AbstractValidator<TransferManager.TransferManagerMigrationCheckRequestModel>
    {
        public TransferManagerMigrationCheckRequestModelValidator()
        {
            RuleFor(request => request.Cookie).NotEmpty().WithMessage("You must specify the character migration cookie.");
            RuleFor(request => request.Cookie).Custom((str, _) =>
            {
                if (TransferManager.StringContainsInvalidChars(TransferManager.CookieChars, str))
                {
                    _.AddFailure("The cookie contains invalid characters.");
                }
            });
            RuleFor(request => request.Cookie).Length(TransferManager.CookieLength).WithMessage($"Cookie must be {TransferManager.CookieLength} characters in length.");
            RuleFor(request => request.Nonce).NotEmpty().WithMessage("You must specify the nonce.");
            RuleFor(request => request.Nonce).Custom((str, _) =>
            {
                if (TransferManager.StringContainsInvalidChars(TransferManager.NonceChars, str))
                {
                    _.AddFailure("The nonce contains invalid characters.");
                }
            });
            RuleFor(request => request.Nonce).Length(TransferManager.NonceLength).WithMessage($"Nonce must be {TransferManager.NonceLength} characters in length.");
        }
    }
}
