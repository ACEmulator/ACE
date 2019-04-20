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

        public WeeniePropertiesBook WeeniePropertiesBook { get; set; }
        public ICollection<WeeniePropertiesAnimPart> WeeniePropertiesAnimPart { get; set; }
        public ICollection<WeeniePropertiesAttribute> WeeniePropertiesAttribute { get; set; }
        public ICollection<WeeniePropertiesAttribute2nd> WeeniePropertiesAttribute2nd { get; set; }
        public ICollection<WeeniePropertiesBodyPart> WeeniePropertiesBodyPart { get; set; }
        public ICollection<WeeniePropertiesBookPageData> WeeniePropertiesBookPageData { get; set; }
        public ICollection<WeeniePropertiesBool> WeeniePropertiesBool { get; set; }
        public ICollection<WeeniePropertiesCreateList> WeeniePropertiesCreateList { get; set; }
        public ICollection<WeeniePropertiesDID> WeeniePropertiesDID { get; set; }
        public ICollection<WeeniePropertiesEmote> WeeniePropertiesEmote { get; set; }
        public ICollection<WeeniePropertiesEventFilter> WeeniePropertiesEventFilter { get; set; }
        public ICollection<WeeniePropertiesFloat> WeeniePropertiesFloat { get; set; }
        public ICollection<WeeniePropertiesGenerator> WeeniePropertiesGenerator { get; set; }
        public ICollection<WeeniePropertiesIID> WeeniePropertiesIID { get; set; }
        public ICollection<WeeniePropertiesInt> WeeniePropertiesInt { get; set; }
        public ICollection<WeeniePropertiesInt64> WeeniePropertiesInt64 { get; set; }
        public ICollection<WeeniePropertiesPalette> WeeniePropertiesPalette { get; set; }
        public ICollection<WeeniePropertiesPosition> WeeniePropertiesPosition { get; set; }
        public ICollection<WeeniePropertiesSkill> WeeniePropertiesSkill { get; set; }
        public ICollection<WeeniePropertiesSpellBook> WeeniePropertiesSpellBook { get; set; }
        public ICollection<WeeniePropertiesString> WeeniePropertiesString { get; set; }
        public ICollection<WeeniePropertiesTextureMap> WeeniePropertiesTextureMap { get; set; }
    }
}
