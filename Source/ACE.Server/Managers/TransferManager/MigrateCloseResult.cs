namespace ACE.Server.Managers.TransferManager
{
    public class MigrateCloseResult
    {
        public bool Success { get; set; } = false;
        public byte[] SnapshotPackage { get; set; }
    }
}
