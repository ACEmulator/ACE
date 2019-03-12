using ACE.Server.Managers.TransferManager.Enums;

namespace ACE.Server.Managers.TransferManager
{
    public class ImportAndMigrateResult
    {
        public ImportAndMigrateFailiureReason FailReason { get; set; } = ImportAndMigrateFailiureReason.Unknown;
        public bool Success { get; set; } = false;
        public string NewCharacterName { get; set; }
        public uint NewCharacterId { get; set; }
    }
}
