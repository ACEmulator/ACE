using ACE.Entity.Enum;

namespace ACE.Entity
{
    public class SummonedPortal : UsableObject
    {
        public SummonedPortal(ObjectType type, ObjectGuid guid) : base(type, guid)
        {
        }

        public override void OnUse(ObjectGuid playerId)
        {
            // TODO: Implement
        }
    }
}
