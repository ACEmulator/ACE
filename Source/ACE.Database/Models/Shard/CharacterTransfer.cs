namespace ACE.Database.Models.Shard
{
    public partial class CharacterTransfer
    {
        public CharacterTransfer() { }
        public uint Id { get; set; }
        public uint TransferType { get; set; }
        public uint AccountId { get; set; }
        public uint? PackageSourceId { get; set; }
        public uint? SourceId { get; set; }
        public uint? TargetId { get; set; }
        public ulong TransferTime { get; set; }
        public ulong? CancelTime { get; set; }
        public ulong? DownloadTime { get; set; }
        public string Cookie { get; set; }
        public string SourceThumbprint { get; set; }
        public string SourceBaseUrl { get; set; }
    }
}
