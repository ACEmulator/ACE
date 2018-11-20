using System.Collections.Generic;

using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public readonly Dictionary<PropertyAttribute, CreatureAttribute> Attributes = new Dictionary<PropertyAttribute, CreatureAttribute>();

        public CreatureAttribute Strength => Attributes[PropertyAttribute.Strength];
        public CreatureAttribute Endurance => Attributes[PropertyAttribute.Endurance];
        public CreatureAttribute Coordination => Attributes[PropertyAttribute.Coordination];
        public CreatureAttribute Quickness => Attributes[PropertyAttribute.Quickness];
        public CreatureAttribute Focus => Attributes[PropertyAttribute.Focus];
        public CreatureAttribute Self => Attributes[PropertyAttribute.Self];
    }
}
