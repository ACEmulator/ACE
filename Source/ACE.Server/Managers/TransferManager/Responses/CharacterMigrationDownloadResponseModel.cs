namespace ACE.Server.Managers.TransferManager.Responses
{
    public class CharacterMigrationDownloadResponseModel
    {
        public bool Success { get; set; }
        public string Cookie { get; set; }
        public byte[] SnapshotPackage { get; set; }
    }
}
