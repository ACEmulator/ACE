using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class WeeniePropertiesAnimPart
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public byte Index { get; set; }
        public uint AnimationId { get; set; }

        public virtual Weenie Object { get; set; }
    }
}
