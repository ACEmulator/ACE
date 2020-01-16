﻿using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class BiotaPropertiesAttribute2nd
    {
        public uint ObjectId { get; set; }
        public ushort Type { get; set; }
        public uint InitLevel { get; set; }
        public uint LevelFromCP { get; set; }
        public uint CPSpent { get; set; }
        public uint CurrentLevel { get; set; }

        public virtual Biota Object { get; set; }
    }
}
