using ACE.Server.Managers;
using FluentValidation;

namespace ACE.WebApiServer.Model.Character.Migration
{
    public class CharacterMigrationCancelRequestModel
    {
        public string Cookie { get; set; }
    }
    public class CharacterMigrationCancelRequestModelValidator : AbstractValidator<CharacterMigrationCancelRequestModel>
    {
        public CharacterMigrationCancelRequestModelValidator()
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
    public class CharacterMigrationCancelResponseModel
    {
        public bool Success { get; set; }
        public string Cookie { get; set; }
    }
}
