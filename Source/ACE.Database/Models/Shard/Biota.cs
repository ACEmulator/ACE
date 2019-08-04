using System;
using System.Collections.Generic;

namespace ACE.Database.Models.Shard
{
    public partial class Biota
    {
        public Biota()
        {
            BiotaPropertiesAllegiance = new HashSet<BiotaPropertiesAllegiance>();
            BiotaPropertiesAnimPart = new HashSet<BiotaPropertiesAnimPart>();
            BiotaPropertiesAttribute = new HashSet<BiotaPropertiesAttribute>();
            BiotaPropertiesAttribute2nd = new HashSet<BiotaPropertiesAttribute2nd>();
            BiotaPropertiesBodyPart = new HashSet<BiotaPropertiesBodyPart>();
            BiotaPropertiesBookPageData = new HashSet<BiotaPropertiesBookPageData>();
            BiotaPropertiesBool = new HashSet<BiotaPropertiesBool>();
            BiotaPropertiesCreateList = new HashSet<BiotaPropertiesCreateList>();
            BiotaPropertiesDID = new HashSet<BiotaPropertiesDID>();
            BiotaPropertiesEmote = new HashSet<BiotaPropertiesEmote>();
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
            HousePermission = new HashSet<HousePermission>();
        }

        public uint Id { get; set; }
        public uint WeenieClassId { get; set; }
        public int WeenieType { get; set; }
        public uint PopulatedCollectionFlags { get; set; }

        public virtual BiotaPropertiesBook BiotaPropertiesBook { get; set; }
        public virtual ICollection<BiotaPropertiesAllegiance> BiotaPropertiesAllegiance { get; set; }
        public virtual ICollection<BiotaPropertiesAnimPart> BiotaPropertiesAnimPart { get; set; }
        public virtual ICollection<BiotaPropertiesAttribute> BiotaPropertiesAttribute { get; set; }
        public virtual ICollection<BiotaPropertiesAttribute2nd> BiotaPropertiesAttribute2nd { get; set; }
        public virtual ICollection<BiotaPropertiesBodyPart> BiotaPropertiesBodyPart { get; set; }
        public virtual ICollection<BiotaPropertiesBookPageData> BiotaPropertiesBookPageData { get; set; }
        public virtual ICollection<BiotaPropertiesBool> BiotaPropertiesBool { get; set; }
        public virtual ICollection<BiotaPropertiesCreateList> BiotaPropertiesCreateList { get; set; }
        public virtual ICollection<BiotaPropertiesDID> BiotaPropertiesDID { get; set; }
        public virtual ICollection<BiotaPropertiesEmote> BiotaPropertiesEmote { get; set; }
        public virtual ICollection<BiotaPropertiesEnchantmentRegistry> BiotaPropertiesEnchantmentRegistry { get; set; }
        public virtual ICollection<BiotaPropertiesEventFilter> BiotaPropertiesEventFilter { get; set; }
        public virtual ICollection<BiotaPropertiesFloat> BiotaPropertiesFloat { get; set; }
        public virtual ICollection<BiotaPropertiesGenerator> BiotaPropertiesGenerator { get; set; }
        public virtual ICollection<BiotaPropertiesIID> BiotaPropertiesIID { get; set; }
        public virtual ICollection<BiotaPropertiesInt> BiotaPropertiesInt { get; set; }
        public virtual ICollection<BiotaPropertiesInt64> BiotaPropertiesInt64 { get; set; }
        public virtual ICollection<BiotaPropertiesPalette> BiotaPropertiesPalette { get; set; }
        public virtual ICollection<BiotaPropertiesPosition> BiotaPropertiesPosition { get; set; }
        public virtual ICollection<BiotaPropertiesSkill> BiotaPropertiesSkill { get; set; }
        public virtual ICollection<BiotaPropertiesSpellBook> BiotaPropertiesSpellBook { get; set; }
        public virtual ICollection<BiotaPropertiesString> BiotaPropertiesString { get; set; }
        public virtual ICollection<BiotaPropertiesTextureMap> BiotaPropertiesTextureMap { get; set; }
        public virtual ICollection<HousePermission> HousePermission { get; set; }
    }
}
