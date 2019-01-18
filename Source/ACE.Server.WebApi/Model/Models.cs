using ACE.Database.Models.Shard;
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
    public class CharacterBackupResponseModel
    {
        public byte[] SnapshotPackage { get; set; }
    }

    public class CharacterMigrationBeginRequestModel
    {
        public uint CharacterId { get; set; }
    }
    public class CharacterMigrationBeginResponseModel
    {
        public string Cookie { get; set; }
        public string BaseURL { get; set; }
    }
    public class CancelMigrationRequestModel
    {
        public string Cookie { get; set; }
    }
    public class CancelMigrationResponseModel
    {
    }
    public class CompleteMigrationRequestModel
    {
        public string Cookie { get; set; }
        public string CharacterName { get; set; }
        public string BaseURL { get; set; }
    }
    public class CompleteMigrationResponseModel
    {
        public bool MigrationSuccess { get; set; }
    }
}
