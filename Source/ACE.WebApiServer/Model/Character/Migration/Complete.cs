using ACE.Common;
using ACE.Server.Managers.TransferManager;
using FluentValidation;

namespace ACE.WebApiServer.Model.Character.Migration
{
    public class CharacterMigrationCompleteRequestModel
    {
        public string Cookie { get; set; }
        public string NewCharacterName { get; set; }
        public string BaseURL { get; set; }
    }
    public class CharacterMigrationCompleteRequestModelValidator : AbstractValidator<CharacterMigrationCompleteRequestModel>
    {
        public CharacterMigrationCompleteRequestModelValidator()
        {
            RuleFor(request => request.BaseURL).NotEmpty().WithMessage("You must specify the base URL.");
            RuleFor(request => request.NewCharacterName).NotEmpty().WithMessage("You must specify the character name to use.");
            RuleFor(request => request.NewCharacterName).Custom((str, _) =>
            {
                if (TransferManagerUtil.StringContainsInvalidChars(GameConfiguration.AllowedCharacterNameCharacters, str))
                {
                    _.AddFailure("The new character name contains invalid characters.");
                }
            });
            RuleFor(request => request.NewCharacterName.Trim())
                .Length(GameConfiguration.CharacterNameMinimumLength, GameConfiguration.CharacterNameMaximumLength)
                .WithMessage("The new character name must be 1 to 32 characters in length.");
            RuleFor(request => request.Cookie).NotEmpty().WithMessage("You must specify the character migration cookie.");
            RuleFor(request => request.Cookie).Custom((str, _) =>
            {
                if (TransferManagerUtil.StringContainsInvalidChars(TransferManagerConstants.CookieChars, str))
                {
                    _.AddFailure("The cookie contains invalid characters.");
                }
            });
            RuleFor(request => request.Cookie).Length(TransferManagerConstants.CookieLength)
                .WithMessage($"Cookie must be {TransferManagerConstants.CookieLength} characters in length.");
        }
    }
    public class CharacterMigrationCompleteResponseModel
    {
        public bool Success { get; set; }
        public string FailureReason { get; set; }
        public string Cookie { get; set; }
        public string CharacterName { get; set; }
        public uint CharacterId { get; set; }
    }
}
