using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameEvent.Events;

namespace ACE.Entity
{
    public class Ammunition : WorldObject
    {
        public override int? Value
        {
            get { return AceObject.Value; }
            set { AceObject.Value = value; }
        }
        public Ammunition(AceObject aceObject)
            : base(aceObject)
        {
        }
    }
}
