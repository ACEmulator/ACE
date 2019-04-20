using System;
using ACE.Entity.Enum;

namespace ACE.Entity
{
    /// <summary>
    /// Emotes are similar to a data-driven event system
    /// </summary>
    public class Emote
    {
        public EmoteType Type;
        public float Delay;
        public float Extent;
        public UInt64 Amount;
        public UInt64 HeroXP;
        public UInt64 Min;
        public UInt64 Max;
        public double MinFloat;
        public double MaxFloat;
        public uint Stat;
        public uint Motion;
        public PlayScript PScript;
        public Sound Sound;
        public CreateProfile CreateProfile;
        public Frame Frame;
        public uint SpellId;
        public string TestString;
        public string Message;
        public double Percent;
        public int Display;
        public int Wealth;
        public int Loot;
        public int LootType;
        public Position Position;
    }
}
