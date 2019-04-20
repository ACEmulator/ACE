using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class ConfigPropertiesLong
    {
        public string Key { get; set; }
        public long Value { get; set; }
        public string Description { get; set; }
    }
}
