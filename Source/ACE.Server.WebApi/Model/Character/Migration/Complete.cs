using ACE.Server.Managers;
using FluentValidation;

namespace ACE.Server.WebApi.Model.Character.Migration
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
            RuleFor(request => request.Cookie).NotEmpty().WithMessage("You must specify the character migration cookie.");
            RuleFor(request => request.Cookie).Custom((str, _) =>
            {
                if (TransferManager.CookieContainsInvalidChars(str))
                {
                    _.AddFailure("The cookie contains invalid characters.");
                }
            });
            RuleFor(request => request.Cookie).Length(TransferManager.CookieLength).WithMessage($"Cookie must be {TransferManager.CookieLength} characters in length.");
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
