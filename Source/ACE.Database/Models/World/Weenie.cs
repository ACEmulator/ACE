using System;
using System.Collections.Generic;

namespace ACE.Database.Models.World
{
    public partial class Weenie
    {
        public Weenie()
        {
            WeeniePropertiesAnimPart = new HashSet<WeeniePropertiesAnimPart>();
            WeeniePropertiesAttribute = new HashSet<WeeniePropertiesAttribute>();
            WeeniePropertiesAttribute2nd = new HashSet<WeeniePropertiesAttribute2nd>();
            WeeniePropertiesBodyPart = new HashSet<WeeniePropertiesBodyPart>();
            WeeniePropertiesBookPageData = new HashSet<WeeniePropertiesBookPageData>();
            WeeniePropertiesBool = new HashSet<WeeniePropertiesBool>();
            WeeniePropertiesCreateList = new HashSet<WeeniePropertiesCreateList>();
            WeeniePropertiesDID = new HashSet<WeeniePropertiesDID>();
            WeeniePropertiesEmote = new HashSet<WeeniePropertiesEmote>();
            WeeniePropertiesEventFilter = new HashSet<WeeniePropertiesEventFilter>();
            WeeniePropertiesFloat = new HashSet<WeeniePropertiesFloat>();
            WeeniePropertiesGenerator = new HashSet<WeeniePropertiesGenerator>();
            WeeniePropertiesIID = new HashSet<WeeniePropertiesIID>();
            WeeniePropertiesInt = new HashSet<WeeniePropertiesInt>();
            WeeniePropertiesInt64 = new HashSet<WeeniePropertiesInt64>();
            WeeniePropertiesPalette = new HashSet<WeeniePropertiesPalette>();
            WeeniePropertiesPosition = new HashSet<WeeniePropertiesPosition>();
            WeeniePropertiesSkill = new HashSet<WeeniePropertiesSkill>();
            WeeniePropertiesSpellBook = new HashSet<WeeniePropertiesSpellBook>();
            WeeniePropertiesString = new HashSet<WeeniePropertiesString>();
            WeeniePropertiesTextureMap = new HashSet<WeeniePropertiesTextureMap>();
        }

        public uint ClassId { get; set; }
        public string ClassName { get; set; }
        public int Type { get; set; }
        public DateTime LastModified { get; set; }

        public virtual WeeniePropertiesBook WeeniePropertiesBook { get; set; }
        public virtual ICollection<WeeniePropertiesAnimPart> WeeniePropertiesAnimPart { get; set; }
        public virtual ICollection<WeeniePropertiesAttribute> WeeniePropertiesAttribute { get; set; }
        public virtual ICollection<WeeniePropertiesAttribute2nd> WeeniePropertiesAttribute2nd { get; set; }
        public virtual ICollection<WeeniePropertiesBodyPart> WeeniePropertiesBodyPart { get; set; }
        public virtual ICollection<WeeniePropertiesBookPageData> WeeniePropertiesBookPageData { get; set; }
        public virtual ICollection<WeeniePropertiesBool> WeeniePropertiesBool { get; set; }
        public virtual ICollection<WeeniePropertiesCreateList> WeeniePropertiesCreateList { get; set; }
        public virtual ICollection<WeeniePropertiesDID> WeeniePropertiesDID { get; set; }
        public virtual ICollection<WeeniePropertiesEmote> WeeniePropertiesEmote { get; set; }
        public virtual ICollection<WeeniePropertiesEventFilter> WeeniePropertiesEventFilter { get; set; }
        public virtual ICollection<WeeniePropertiesFloat> WeeniePropertiesFloat { get; set; }
        public virtual ICollection<WeeniePropertiesGenerator> WeeniePropertiesGenerator { get; set; }
        public virtual ICollection<WeeniePropertiesIID> WeeniePropertiesIID { get; set; }
        public virtual ICollection<WeeniePropertiesInt> WeeniePropertiesInt { get; set; }
        public virtual ICollection<WeeniePropertiesInt64> WeeniePropertiesInt64 { get; set; }
        public virtual ICollection<WeeniePropertiesPalette> WeeniePropertiesPalette { get; set; }
        public virtual ICollection<WeeniePropertiesPosition> WeeniePropertiesPosition { get; set; }
        public virtual ICollection<WeeniePropertiesSkill> WeeniePropertiesSkill { get; set; }
        public virtual ICollection<WeeniePropertiesSpellBook> WeeniePropertiesSpellBook { get; set; }
        public virtual ICollection<WeeniePropertiesString> WeeniePropertiesString { get; set; }
        public virtual ICollection<WeeniePropertiesTextureMap> WeeniePropertiesTextureMap { get; set; }
    }
}
