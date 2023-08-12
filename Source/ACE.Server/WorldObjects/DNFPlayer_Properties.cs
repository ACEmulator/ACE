using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public bool? YouAreFrozen
        {
            get => GetProperty(PropertyBool.YouAreFrozen);
            set => SetPhysicsPropertyState(PropertyBool.IsFrozen, PhysicsState.Frozen, value);
        }

        public bool? YouAreJailed
        {
            get => GetProperty(PropertyBool.YouAreJailed);
            set => SetProperty(PropertyBool.YouAreJailed, (bool)value);
        }

        public bool? YouAreAttackable
        {
            get => GetProperty(PropertyBool.YouAreAttackable);
            set => SetProperty(PropertyBool.Attackable, (bool)value);
        }
    }
}
