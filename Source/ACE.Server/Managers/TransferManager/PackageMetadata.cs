using ACE.Server.Managers.TransferManager.Enums;
using System;

namespace ACE.Server.Managers.TransferManager
{
    public class PackageMetadata
    {
        public PackageType PackageType { get; set; }
        public string Cookie { get; set; }
        public uint CharacterId { get; set; }
        public uint AccountId { get; set; }
        public string FilePath { get; set; }
        public string NewCharacterName { get; set; }
        public Uri ImportUrl { get; set; }
    }
}
