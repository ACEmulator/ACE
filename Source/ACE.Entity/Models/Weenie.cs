using System;
using System.Collections.Generic;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

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
        public WeenieType WeenieType { get; set; }

        public IDictionary<PropertyBool, bool> PropertiesBool { get; set; }
        public IDictionary<PropertyDataId, uint> PropertiesDID { get; set; }
        public IDictionary<PropertyFloat, double> PropertiesFloat { get; set; }
        public IDictionary<PropertyInstanceId, uint> PropertiesIID { get; set; }
        public IDictionary<PropertyInt, int> PropertiesInt { get; set; }
        public IDictionary<PropertyInt64, long> PropertiesInt64 { get; set; }
        public IDictionary<PropertyString, string> PropertiesString { get; set; }

        public IDictionary<PositionType, PropertiesPosition> PropertiesPosition { get; set; }

        public IDictionary<int, float /* probability */> PropertiesSpellBook { get; set; }

        public IList<PropertiesAnimPart> PropertiesAnimPart { get; set; }
        public IList<PropertiesPalette> PropertiesPalette { get; set; }
        public IList<PropertiesTextureMap> PropertiesTextureMap { get; set; }

        // Properties for all world objects that typically aren't modified over the original weenie
        public ICollection<PropertiesCreateList> PropertiesCreateList { get; set; }
        public ICollection<PropertiesEmote> PropertiesEmote { get; set; }
        public HashSet<int> PropertiesEventFilter { get; set; }
        public IList<PropertiesGenerator> PropertiesGenerator { get; set; }

        // Properties for creatures
        public IDictionary<PropertyAttribute, PropertiesAttribute> PropertiesAttribute { get; set; }
        public IDictionary<PropertyAttribute2nd, PropertiesAttribute2nd> PropertiesAttribute2nd { get; set; }
        public IDictionary<CombatBodyPart, PropertiesBodyPart> PropertiesBodyPart { get; set; }
        public IDictionary<Skill, PropertiesSkill> PropertiesSkill { get; set; }

        // Properties for books
        public PropertiesBook PropertiesBook { get; set; }
        public IList<PropertiesBookPageData> PropertiesBookPageData { get; set; }
    }
}
