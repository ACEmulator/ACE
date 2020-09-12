using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class WeeniePropertiesAttribute
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public ushort Type { get; set; }
        public uint InitLevel { get; set; }
        public uint LevelFromCP { get; set; }
        public uint CPSpent { get; set; }

        public virtual Weenie Object { get; set; }
    }
}
