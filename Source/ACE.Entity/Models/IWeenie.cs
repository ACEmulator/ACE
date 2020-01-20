using System;
using System.Collections.Generic;

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
        int WeenieType { get; set; }

        IDictionary<ushort, bool> PropertiesBool { get; set; }
        IDictionary<ushort, uint> PropertiesDID { get; set; }
        IDictionary<ushort, double> PropertiesFloat { get; set; }
        IDictionary<ushort, uint> PropertiesIID { get; set; }
        IDictionary<ushort, int> PropertiesInt { get; set; }
        IDictionary<ushort, long> PropertiesInt64 { get; set; }
        IDictionary<ushort, string> PropertiesString { get; set; }

        IDictionary<ushort, PropertiesPosition> PropertiesPosition { get; set; }

        IDictionary<int, float /* probability */> PropertiesSpellBook { get; set; }

        IDictionary<byte, uint /* AnimationId */> PropertiesAnimPart { get; set; }
        ICollection<PropertiesPalette> PropertiesPalette { get; set; }
        ICollection<PropertiesTextureMap> PropertiesTextureMap { get; set; }

        // Properties for all world objects that typically aren't modified over the original weenie
        ICollection<PropertiesCreateList> PropertiesCreateList { get; set; }
        ICollection<PropertiesEmote> PropertiesEmote { get; set; }
        ICollection<int> PropertiesEventFilter { get; set; }
        ICollection<PropertiesGenerator> PropertiesGenerator { get; set; }

        // Properties for creatures
        IDictionary<ushort, PropertiesAttribute> PropertiesAttribute { get; set; }
        IDictionary<ushort, PropertiesAttribute2nd> PropertiesAttribute2nd { get; set; }
        IDictionary<ushort, PropertiesBodyPart> PropertiesBodyPart { get; set; }
        IDictionary<ushort, PropertiesSkill> PropertiesSkill { get; set; }

        // Properties for books
        PropertiesBook PropertiesBook { get; set; }
        IList<PropertiesBookPageData> PropertiesBookPageData { get; set; }
    }
}
