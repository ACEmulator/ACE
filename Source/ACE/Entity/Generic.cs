using ACE.Managers;

namespace ACE.Entity
{
    public class Generic : WorldObject
    {
        public Generic(AceObject aceObject)
            : base(aceObject)
        {
        }

        public Generic(ObjectGuid guid, AceObject aceObject)
            : base(guid, aceObject)
        {
        }

        ////public override void OnCollide(ObjectGuid playerId)
        ////{
        ////    // TODO: Implement
        ////}
    }
}
