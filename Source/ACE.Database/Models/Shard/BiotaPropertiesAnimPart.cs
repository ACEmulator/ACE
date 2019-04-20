﻿using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesAnimPart
    {
        public uint Id { get; set; }
        public uint ObjectId { get; set; }
        public byte Index { get; set; }
        public uint AnimationId { get; set; }
        public byte? Order { get; set; }

        public Biota Object { get; set; }
    }
}
