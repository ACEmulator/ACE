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
    public interface IWeenie
    {
        uint WeenieClassId { get; set; }
        WeenieType WeenieType { get; set; }

        IDictionary<PropertyBool, bool> PropertiesBool { get; set; }
        IDictionary<PropertyDataId, uint> PropertiesDID { get; set; }
        IDictionary<PropertyFloat, double> PropertiesFloat { get; set; }
        IDictionary<PropertyInstanceId, uint> PropertiesIID { get; set; }
        IDictionary<PropertyInt, int> PropertiesInt { get; set; }
        IDictionary<PropertyInt64, long> PropertiesInt64 { get; set; }
        IDictionary<PropertyString, string> PropertiesString { get; set; }

        IDictionary<PositionType, PropertiesPosition> PropertiesPosition { get; set; }

        IDictionary<int, float /* probability */> PropertiesSpellBook { get; set; }

        IList<PropertiesAnimPart> PropertiesAnimPart { get; set; }
        IList<PropertiesPalette> PropertiesPalette { get; set; }
        IList<PropertiesTextureMap> PropertiesTextureMap { get; set; }

        // Properties for all world objects that typically aren't modified over the original weenie
        ICollection<PropertiesCreateList> PropertiesCreateList { get; set; }
        ICollection<PropertiesEmote> PropertiesEmote { get; set; }
        HashSet<int> PropertiesEventFilter { get; set; }
        IList<PropertiesGenerator> PropertiesGenerator { get; set; } // Using a list per this: https://github.com/ACEmulator/ACE/pull/2616, however, no order is guaranteed for db records

        // Properties for creatures
        IDictionary<PropertyAttribute, PropertiesAttribute> PropertiesAttribute { get; set; }
        IDictionary<PropertyAttribute2nd, PropertiesAttribute2nd> PropertiesAttribute2nd { get; set; }
        IDictionary<CombatBodyPart, PropertiesBodyPart> PropertiesBodyPart { get; set; }
        IDictionary<Skill, PropertiesSkill> PropertiesSkill { get; set; }

        // Properties for books
        PropertiesBook PropertiesBook { get; set; }
        IList<PropertiesBookPageData> PropertiesBookPageData { get; set; }
    }
}
