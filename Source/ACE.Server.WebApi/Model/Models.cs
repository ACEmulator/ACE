using ACE.Database.Models.Shard;
using ACE.Entity;
using ACE.Server.Managers;
using FluentValidation;
using System.Collections.Generic;

namespace ACE.Server.WebApi.Model
{
    public class CharacterListModel : BaseModel
    {
        public List<Character> Characters { get; set; }
    }
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
                if (TransferManager.CookieContainsInvalidChars(str))
                {
                    _.AddFailure("The cookie contains invalid characters.");
                }
            });
        }
    }
    public class CharacterMigrationCancelResponseModel
    {
        public bool Success { get; set; }
        public string Cookie { get; set; }
    }
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
                if (TransferManager.CookieContainsInvalidChars(str))
                {
                    _.AddFailure("The cookie contains invalid characters.");
                }
            });
        }
    }
    public class CharacterMigrationDownloadResponseModel : TransferManager.TransferManagerCharacterMigrationDownloadResponseModel { }
    public class CharacterImportRequestModel
    {
        public string SnapshotPackageBase64 { get; set; }
        public string NewCharacterName { get; set; }
    }
    public class CharacterImportRequestModelValidator : AbstractValidator<CharacterImportRequestModel>
    {
        public CharacterImportRequestModelValidator()
        {
            RuleFor(request => request.SnapshotPackageBase64).NotEmpty().WithMessage("You must specify the snapshot package.");
            RuleFor(request => request.NewCharacterName).NotEmpty().WithMessage("You must specify the character name to use.");
        }
    }
    public class CharacterImportResponseModel
    {
        public bool Success { get; set; }
        public string FailureReason { get; set; }
        public string CharacterName { get; set; }
        public uint CharacterId { get; set; }
    }

    public class TransferConfigResponseModel : TransferManager.TransferManagerTransferConfigResponseModel { }

    public class PlayerCountResponseModel
    {
        public int Online { get; set; }
        public int Offline { get; set; }
    }

    public class NetworkStatsResponseModel
    {
        public long C2S_RequestsForRetransmit_Aggregate { get; set; }
        public long S2C_RequestsForRetransmit_Aggregate { get; set; }
        public long C2S_CRCErrors_Aggregate { get; set; }
        public long C2S_Packets_Aggregate { get; set; }
        public long S2C_Packets_Aggregate { get; set; }
        public string Summary { get; set; }
    }

    public class PlayerNameAndLocation
    {
        public string Location { get; set; }
        public string Name { get; set; }
    }
    public class PlayerLocationsResponseModel
    {
        public List<PlayerNameAndLocation> Locations { get; set; }
    }

    public class LandblockStatus
    {
        public int Actions { get; set; }
        public int Objects { get; set; }
        public string Id { get; set; }
    }
    public class LandblockStatusResponseModel
    {
        public List<LandblockStatus> Active { get; set; }
    }
}
