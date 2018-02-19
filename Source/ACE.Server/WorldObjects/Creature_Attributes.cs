using System.Collections.Generic;

using ACE.Entity.Enum;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public readonly Dictionary<Ability, CreatureAttribute> Attributes = new Dictionary<Ability, CreatureAttribute>();

        public CreatureAttribute Strength => Attributes[Ability.Strength];
        public CreatureAttribute Endurance => Attributes[Ability.Endurance];
        public CreatureAttribute Coordination => Attributes[Ability.Coordination];
        public CreatureAttribute Quickness => Attributes[Ability.Quickness];
        public CreatureAttribute Focus => Attributes[Ability.Focus];
        public CreatureAttribute Self => Attributes[Ability.Self];
    }
}
