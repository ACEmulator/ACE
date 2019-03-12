namespace ACE.Server.Managers.TransferManager.Enums
{
    public enum MigrationReadyStatus
    {
        Unknown,
        Ready,
        NonExistant,
        NotMigration,
        AlreadyDownloaded,
        AlreadyCancelled,
        CharNotFound,
        InvalidCharState,
        PackageFileMissing
    }
}
