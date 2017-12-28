using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameEvent.Events;

namespace ACE.Entity
{
    public class MeleeWeapon : WorldObject
    {
        public override int? Value
        {
            get { return AceObject.Value; }
            set { AceObject.Value = value; }
        }
        public override ushort? Burden
        {
            get { return AceObject.EncumbranceVal; }
            set { AceObject.EncumbranceVal = value; }
        }
        public override int? DamageType
        {
            get { return AceObject.DamageType; }
            set { AceObject.DamageType = value; }
        }
        public MeleeWeapon(AceObject aceObject)
            : base(aceObject)
        {
        }
    }
}
