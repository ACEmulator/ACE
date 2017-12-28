using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameEvent.Events;

namespace ACE.Entity
{
    public class Caster : WorldObject
    {
        public override int? Value
        {
            get { return AceObject.Value; }
            set { AceObject.Value = value; }
        }
        public Caster(AceObject aceObject)
            : base(aceObject)
        {
        }
    }
}
