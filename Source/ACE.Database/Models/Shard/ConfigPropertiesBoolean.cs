using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class ConfigPropertiesBoolean
    {
        public string Key { get; set; }
        public bool Value { get; set; }
        public string Description { get; set; }
    }
}
