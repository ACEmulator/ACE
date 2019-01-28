using FluentValidation;

namespace ACE.WebApiServer.Model.Character.Migration
{
    public class CharacterMigrationBeginRequestModel
    {
        public uint CharacterId { get; set; }
    }
    public class CharacterMigrationBeginRequestModelValidator : AbstractValidator<CharacterMigrationBeginRequestModel>
    {
        public CharacterMigrationBeginRequestModelValidator()
        {
            RuleFor(request => request.CharacterId).NotEmpty().WithMessage("You must specify the character Id.");
            RuleFor(request => request.CharacterId).GreaterThan((uint)0).WithMessage("You must specify a valid character Id.");
        }
    }
    public class CharacterMigrationBeginResponseModel
    {
        public bool Success { get; set; }
        public uint CharacterId { get; set; }
        public string Cookie { get; set; }
        public string BaseURL { get; set; }
    }
}
