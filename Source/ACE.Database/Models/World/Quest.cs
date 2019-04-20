using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class Quest
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public uint MinDelta { get; set; }
        public int MaxSolves { get; set; }
        public string Message { get; set; }
        public DateTime LastModified { get; set; }
    }
}
