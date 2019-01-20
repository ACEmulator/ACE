using ACE.Server.Managers;
using FluentValidation;

namespace ACE.WebApiServer.Model.Character.Migration
{
    public class CharacterMigrationDownloadRequestModel
    {
        public string Cookie { get; set; }
    }
    public class CharacterMigrationDownloadRequestModelValidator : AbstractValidator<CharacterMigrationDownloadRequestModel>
    {
        public CharacterMigrationDownloadRequestModelValidator()
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
        }
    }
    public class CharacterMigrationDownloadResponseModel : TransferManager.TransferManagerCharacterMigrationDownloadResponseModel { }
}
