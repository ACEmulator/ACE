using System;
using System.Collections.Generic;

#nullable disable

namespace ACE.Database.Models.World
{
    public partial class WeeniePropertiesFloat
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public ushort Type { get; set; }
        public double Value { get; set; }

        public virtual Weenie Object { get; set; }
    }
}
