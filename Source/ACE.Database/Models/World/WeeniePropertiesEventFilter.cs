using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class WeeniePropertiesEventFilter
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public int Event { get; set; }

        public virtual Weenie Object { get; set; }
    }
}
