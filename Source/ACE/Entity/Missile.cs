using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameEvent.Events;

namespace ACE.Entity
{
    public class Missile : WorldObject
    {
        public override int? Value
        {
            get { return AceObject.Value; }
            set { AceObject.Value = value; }
        }
        public override double? MaximumVelocity
        {
            get { return AceObject.MaximumVelocity; }
            set { AceObject.MaximumVelocity = value; }
        }
        public override int? DamageType
        {
            get { return AceObject.Damage; }
            set { AceObject.Damage = value; }
        }
        public override int? WeaponTime
        {
            get { return AceObject.WeaponTime; }
            set { AceObject.WeaponTime = value; }
        }
        public Missile(AceObject aceObject)
            : base(aceObject)
        {
        }
    }
}
