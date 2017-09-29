using System;
using ACE.Common;
using MySql.Data.MySqlClient;

namespace ACE.Entity
{
    [DbTable("ace_object_properties_attribute")]
    public class AceObjectPropertiesAttribute : BaseAceProperty, ICloneable
    {
        private uint _xpSpent = 0;
        private ushort _ranks = 0;
        private ushort _base = 0;

        [DbField("attributeId", (int)MySqlDbType.UInt16, IsCriteria = true, Update = false)]
        public ushort AttributeId { get; set; }

        [DbField("attributeBase", (int)MySqlDbType.UInt16)]
        public ushort AttributeBase
        {
            get
            {
                return _base;
            }
            set
            {
                _base = value;
                IsDirty = true;
            }
        }

        [DbField("attributeRanks", (int)MySqlDbType.UInt16)]
        public ushort AttributeRanks
        {
            get
            {
                return _ranks;
            }
            set
            {
                _ranks = value;
                IsDirty = true;
            }
        }

        [DbField("attributeXpSpent", (int)MySqlDbType.UInt32)]
        public uint AttributeXpSpent
        {
            get
            {
                return _xpSpent;
            }
            set
            {
                _xpSpent = value;
                IsDirty = true;
            }
        }

        public uint ActiveValue
        {
            get
            {
                return (uint)AttributeBase + AttributeRanks;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
