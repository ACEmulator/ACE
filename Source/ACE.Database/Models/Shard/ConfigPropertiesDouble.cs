using System;
using System.Collections.Generic;

#nullable disable

namespace ACE.Database.Models.Shard
{
    public partial class ConfigPropertiesDouble
    {
        public string Key { get; set; }
        public double Value { get; set; }
        public string Description { get; set; }
    }
}
