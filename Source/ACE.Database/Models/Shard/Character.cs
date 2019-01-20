using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class Character
    {
        public Character()
        {
            CharacterPropertiesContract = new HashSet<CharacterPropertiesContract>();
            CharacterPropertiesFillCompBook = new HashSet<CharacterPropertiesFillCompBook>();
            CharacterPropertiesFriendList = new HashSet<CharacterPropertiesFriendList>();
            CharacterPropertiesQuestRegistry = new HashSet<CharacterPropertiesQuestRegistry>();
            CharacterPropertiesShortcutBar = new HashSet<CharacterPropertiesShortcutBar>();
            CharacterPropertiesSpellBar = new HashSet<CharacterPropertiesSpellBar>();
            CharacterPropertiesTitleBook = new HashSet<CharacterPropertiesTitleBook>();
        }

        public uint Id { get; set; }
        public uint AccountId { get; set; }
        public string Name { get; set; }
        public bool IsPlussed { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsReadOnly { get; set; }
        public ulong DeleteTime { get; set; }
        public double LastLoginTimestamp { get; set; }
        public int TotalLogins { get; set; }
        public int CharacterOptions1 { get; set; }
        public int CharacterOptions2 { get; set; }
        public byte[] GameplayOptions { get; set; }
        public uint SpellbookFilters { get; set; }
        public uint HairTexture { get; set; }
        public uint DefaultHairTexture { get; set; }

        public ICollection<CharacterPropertiesContract> CharacterPropertiesContract { get; set; }
        public ICollection<CharacterPropertiesFillCompBook> CharacterPropertiesFillCompBook { get; set; }
        public ICollection<CharacterPropertiesFriendList> CharacterPropertiesFriendList { get; set; }
        public ICollection<CharacterPropertiesQuestRegistry> CharacterPropertiesQuestRegistry { get; set; }
        public ICollection<CharacterPropertiesShortcutBar> CharacterPropertiesShortcutBar { get; set; }
        public ICollection<CharacterPropertiesSpellBar> CharacterPropertiesSpellBar { get; set; }
        public ICollection<CharacterPropertiesTitleBook> CharacterPropertiesTitleBook { get; set; }
    }
}
