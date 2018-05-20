using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class WeeniePropertiesBool
    {
        public uint ObjectId { get; set; }
        public ushort Type { get; set; }
        public bool Value { get; set; }

        public Weenie Object { get; set; }
    }
}
