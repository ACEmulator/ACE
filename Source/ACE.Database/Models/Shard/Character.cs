using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class Character
    {
        public uint Id { get; set; }
        public uint AccountId { get; set; }
        public uint BiotaId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public ulong DeleteTime { get; set; }
        public double LastLoginTimestamp { get; set; }
        public int TotalLogins { get; set; }
        public int CharacterOptions1 { get; set; }
        public int CharacterOptions2 { get; set; }
        public byte[] GameplayOptions { get; set; }
        public uint HairTexture { get; set; }
        public uint DefaultHairTexture { get; set; }

        public Biota Biota { get; set; }
    }
}
