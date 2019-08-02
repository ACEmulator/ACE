﻿using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesCreateList
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public sbyte DestinationType { get; set; }
        public uint WeenieClassId { get; set; }
        public int StackSize { get; set; }
        public sbyte Palette { get; set; }
        public float Shade { get; set; }
        public bool TryToBond { get; set; }

        public virtual Biota Object { get; set; }
    }
}
