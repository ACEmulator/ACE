using FluentValidation;

namespace ACE.WebApiServer.Model.Character
{
    public class CharacterBackupRequestModel
    {
        public uint CharacterId { get; set; }
    }
    public class CharacterBackupRequestModelValidator : AbstractValidator<CharacterBackupRequestModel>
    {
        public CharacterBackupRequestModelValidator()
        {
            RuleFor(request => request.CharacterId).NotEmpty().WithMessage("You must specify the character Id.");
            RuleFor(request => request.CharacterId).GreaterThan((uint)0).WithMessage("You must specify a valid character Id.");
        }
    }
    public class CharacterBackupResponseModel
    {
        public bool Success { get; set; }
        public uint CharacterId { get; set; }
        public byte[] SnapshotPackage { get; set; }
    }
}
