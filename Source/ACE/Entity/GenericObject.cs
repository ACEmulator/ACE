// WeenieType.Generic

namespace ACE.Entity
{
    public class GenericObject : WorldObject
    {
        public GenericObject(AceObject aceObject)
            : base(aceObject)
        {
        }

        public GenericObject(ObjectGuid guid, AceObject aceObject)
            : base(guid, aceObject)
        {
        }

        ////public override void OnCollide(ObjectGuid playerId)
        ////{
        ////    // TODO: Implement
        ////}

        ////public override void OnUse(ObjectGuid playerId)
        ////{
        ////    // TODO: Implement
        ////}
    }
}
