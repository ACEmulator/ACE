using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class UsableObject : WorldObject
    {
        public UsableObject(ItemType type, ObjectGuid guid)
            : base(guid)
        {
        }

        public UsableObject(AceObject aceObject)
            : base(aceObject)
        {
        }

        public virtual void OnUse(ObjectGuid playerId)
        {
            // todo: implement.  default is probably to pick it up off the ground
        }
    }
}
