using System;
using System.Collections.Generic;

namespace ACE.Entity.Models
{
    /// <summary>
    /// Only populated collections and dictionaries are initialized.
    /// We do this to conserve memory in ACE.Server
    /// Be sure to check for null first.
    /// </summary>
    public class Weenie : IWeenie
    {
        public uint WeenieClassId { get; set; }
        public string ClassName { get; set; }
        public int WeenieType { get; set; }

        public IDictionary<ushort, bool> PropertiesBool { get; set; }
        public IDictionary<ushort, uint> PropertiesDID { get; set; }
        public IDictionary<ushort, double> PropertiesFloat { get; set; }
        public IDictionary<ushort, uint> PropertiesIID { get; set; }
        public IDictionary<ushort, int> PropertiesInt { get; set; }
        public IDictionary<ushort, long> PropertiesInt64 { get; set; }
        public IDictionary<ushort, string> PropertiesString { get; set; }

        public IDictionary<ushort, PropertiesPosition> PropertiesPosition { get; set; }

        public IDictionary<int, float /* probability */> PropertiesSpellBook { get; set; }

        public ICollection<PropertiesCreateList> PropertiesCreateList { get; set; }
        public ICollection<PropertiesEmote> PropertiesEmote { get; set; }
        public ICollection<int> PropertiesEventFilter { get; set; }
        public ICollection<PropertiesGenerator> PropertiesGenerator { get; set; }

        public IDictionary<byte, uint /* AnimationId */> PropertiesAnimPart { get; set; }
        public ICollection<PropertiesPalette> PropertiesPalette { get; set; }
        public ICollection<PropertiesTextureMap> PropertiesTextureMap { get; set; }

        // Properties for creatures
        public IDictionary<ushort, PropertiesAttribute> PropertiesAttribute { get; set; }
        public IDictionary<ushort, PropertiesAttribute2nd> PropertiesAttribute2nd { get; set; }
        public IDictionary<ushort, PropertiesBodyPart> PropertiesBodyPart { get; set; }
        public IDictionary<ushort, PropertiesSkill> PropertiesSkill { get; set; }

        // Properties for books
        public PropertiesBook PropertiesBook { get; set; }
        public IDictionary<uint, PropertiesBookPageData> PropertiesBookPageData { get; set; }
    }
}
