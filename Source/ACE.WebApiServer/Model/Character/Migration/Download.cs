using ACE.Server.Managers.TransferManager;
using ACE.Server.Managers.TransferManager.Responses;
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
                if (TransferManagerUtil.StringContainsInvalidChars(TransferManagerConstants.CookieChars, str))
                {
                    _.AddFailure("The cookie contains invalid characters.");
                }
            });
            RuleFor(request => request.Cookie).Length(TransferManagerConstants.CookieLength).WithMessage($"Cookie must be {TransferManagerConstants.CookieLength} characters in length.");
        }
    }
    public class WebApiCharacterMigrationDownloadResponseModel : CharacterMigrationDownloadResponseModel { }
}
