﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ACE.Entity
{
    public class StarterGearSkill
    {
        [JsonProperty("id")]
        public ushort SkillId { get; set; }

        /// <summary>
        /// not used, but the file has it for readability
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("gear")]
        public List<StarterItem> Gear { get; set; } = new List<StarterItem>();

        [JsonProperty("heritage")]
        public List<StarterHeritage> Heritage { get; set; } = new List<StarterHeritage>();

        [JsonProperty("spells")]
        public List<StarterSpell> Spells { get; set; } = new List<StarterSpell>();
    }
}
