namespace ACE.Entity
{
    public abstract class CollidableObject : WorldObject
    {
        internal CollidableObject(AceObject aceO)
            : base(aceO)
        {
        }

        public abstract void OnCollide(ObjectGuid playerId);
    }
}
