using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class Biota
    {
        public Biota()
        {
            BiotaPropertiesAnimPart = new HashSet<BiotaPropertiesAnimPart>();
            BiotaPropertiesAttribute = new HashSet<BiotaPropertiesAttribute>();
            BiotaPropertiesAttribute2nd = new HashSet<BiotaPropertiesAttribute2nd>();
            BiotaPropertiesBodyPart = new HashSet<BiotaPropertiesBodyPart>();
            BiotaPropertiesBookPageData = new HashSet<BiotaPropertiesBookPageData>();
            BiotaPropertiesBool = new HashSet<BiotaPropertiesBool>();
            BiotaPropertiesCreateList = new HashSet<BiotaPropertiesCreateList>();
            BiotaPropertiesDID = new HashSet<BiotaPropertiesDID>();
            BiotaPropertiesEmote = new HashSet<BiotaPropertiesEmote>();
            BiotaPropertiesEmoteAction = new HashSet<BiotaPropertiesEmoteAction>();
            BiotaPropertiesEnchantmentRegistry = new HashSet<BiotaPropertiesEnchantmentRegistry>();
            BiotaPropertiesEventFilter = new HashSet<BiotaPropertiesEventFilter>();
            BiotaPropertiesFloat = new HashSet<BiotaPropertiesFloat>();
            BiotaPropertiesGenerator = new HashSet<BiotaPropertiesGenerator>();
            BiotaPropertiesIID = new HashSet<BiotaPropertiesIID>();
            BiotaPropertiesInt = new HashSet<BiotaPropertiesInt>();
            BiotaPropertiesInt64 = new HashSet<BiotaPropertiesInt64>();
            BiotaPropertiesPalette = new HashSet<BiotaPropertiesPalette>();
            BiotaPropertiesPosition = new HashSet<BiotaPropertiesPosition>();
            BiotaPropertiesSkill = new HashSet<BiotaPropertiesSkill>();
            BiotaPropertiesSpellBook = new HashSet<BiotaPropertiesSpellBook>();
            BiotaPropertiesString = new HashSet<BiotaPropertiesString>();
            BiotaPropertiesTextureMap = new HashSet<BiotaPropertiesTextureMap>();
            CharacterPropertiesContract = new HashSet<CharacterPropertiesContract>();
            CharacterPropertiesFillCompBook = new HashSet<CharacterPropertiesFillCompBook>();
            CharacterPropertiesFriendList = new HashSet<CharacterPropertiesFriendList>();
            CharacterPropertiesQuestRegistry = new HashSet<CharacterPropertiesQuestRegistry>();
            CharacterPropertiesShortcutBar = new HashSet<CharacterPropertiesShortcutBar>();
            CharacterPropertiesSpellBar = new HashSet<CharacterPropertiesSpellBar>();
            CharacterPropertiesTitleBook = new HashSet<CharacterPropertiesTitleBook>();
        }

        public uint Id { get; set; }
        public uint WeenieClassId { get; set; }
        public int WeenieType { get; set; }

        public BiotaPropertiesBook BiotaPropertiesBook { get; set; }
        public Character Character { get; set; }
        public ICollection<BiotaPropertiesAnimPart> BiotaPropertiesAnimPart { get; set; }
        public ICollection<BiotaPropertiesAttribute> BiotaPropertiesAttribute { get; set; }
        public ICollection<BiotaPropertiesAttribute2nd> BiotaPropertiesAttribute2nd { get; set; }
        public ICollection<BiotaPropertiesBodyPart> BiotaPropertiesBodyPart { get; set; }
        public ICollection<BiotaPropertiesBookPageData> BiotaPropertiesBookPageData { get; set; }
        public ICollection<BiotaPropertiesBool> BiotaPropertiesBool { get; set; }
        public ICollection<BiotaPropertiesCreateList> BiotaPropertiesCreateList { get; set; }
        public ICollection<BiotaPropertiesDID> BiotaPropertiesDID { get; set; }
        public ICollection<BiotaPropertiesEmote> BiotaPropertiesEmote { get; set; }
        public ICollection<BiotaPropertiesEmoteAction> BiotaPropertiesEmoteAction { get; set; }
        public ICollection<BiotaPropertiesEnchantmentRegistry> BiotaPropertiesEnchantmentRegistry { get; set; }
        public ICollection<BiotaPropertiesEventFilter> BiotaPropertiesEventFilter { get; set; }
        public ICollection<BiotaPropertiesFloat> BiotaPropertiesFloat { get; set; }
        public ICollection<BiotaPropertiesGenerator> BiotaPropertiesGenerator { get; set; }
        public ICollection<BiotaPropertiesIID> BiotaPropertiesIID { get; set; }
        public ICollection<BiotaPropertiesInt> BiotaPropertiesInt { get; set; }
        public ICollection<BiotaPropertiesInt64> BiotaPropertiesInt64 { get; set; }
        public ICollection<BiotaPropertiesPalette> BiotaPropertiesPalette { get; set; }
        public ICollection<BiotaPropertiesPosition> BiotaPropertiesPosition { get; set; }
        public ICollection<BiotaPropertiesSkill> BiotaPropertiesSkill { get; set; }
        public ICollection<BiotaPropertiesSpellBook> BiotaPropertiesSpellBook { get; set; }
        public ICollection<BiotaPropertiesString> BiotaPropertiesString { get; set; }
        public ICollection<BiotaPropertiesTextureMap> BiotaPropertiesTextureMap { get; set; }
        public ICollection<CharacterPropertiesContract> CharacterPropertiesContract { get; set; }
        public ICollection<CharacterPropertiesFillCompBook> CharacterPropertiesFillCompBook { get; set; }
        public ICollection<CharacterPropertiesFriendList> CharacterPropertiesFriendList { get; set; }
        public ICollection<CharacterPropertiesQuestRegistry> CharacterPropertiesQuestRegistry { get; set; }
        public ICollection<CharacterPropertiesShortcutBar> CharacterPropertiesShortcutBar { get; set; }
        public ICollection<CharacterPropertiesSpellBar> CharacterPropertiesSpellBar { get; set; }
        public ICollection<CharacterPropertiesTitleBook> CharacterPropertiesTitleBook { get; set; }
    }
}
