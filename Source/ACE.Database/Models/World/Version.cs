using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class Version
    {
        public uint Id { get; set; }
        public string BaseVersion { get; set; }
        public string PatchVersion { get; set; }
        public DateTime LastModified { get; set; }
    }
}
