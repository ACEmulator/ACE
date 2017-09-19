﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ACE.Entity
{
    public class StarterItem
    {
        public StarterItem()
        {
            StackSize = 1;
        }

        [JsonProperty("weenieId")]
        public uint WeenieId { get; set; }

        [JsonProperty("stacksize")]
        public ushort StackSize { get; set; }
    }
}
