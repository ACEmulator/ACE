using System.Collections.Generic;

namespace ACE.Server.Managers.TransferManager.Responses
{
    public class TransferConfigResponseModel
    {
        public string MyThumbprint { get; set; }
        public List<string> AllowMigrationFrom { get; set; }
        public List<string> AllowImportFrom { get; set; }
        public bool AllowBackup { get; set; }
        public bool AllowImport { get; set; }
        public bool AllowMigrate { get; set; }
    }
}
