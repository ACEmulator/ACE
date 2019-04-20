using System.Collections.Generic;
using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class EmoteSet
    {
        public EmoteCategory Category;
        public float Probability;
        public uint ClassID;
        public string Quest;
        public uint Style;
        public uint Substate;
        public uint VendorType;
        public float MinHealth;
        public float MaxHealth;
        public List<Emote> Emotes;
    }
}
