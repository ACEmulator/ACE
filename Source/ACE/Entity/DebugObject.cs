namespace ACE.Entity
{
    public class DebugObject : CollidableObject
    {
        public DebugObject(AceObject aceObject)
            : base(aceObject)
        {
        }

        public DebugObject(ObjectGuid guid, AceObject aceObject)
            : base(guid, aceObject)
        {
        }

        public override void OnCollide(ObjectGuid playerId)
        {
            // TODO: Implement
        }
    }
}
