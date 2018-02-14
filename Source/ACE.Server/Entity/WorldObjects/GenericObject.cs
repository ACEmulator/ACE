using ACE.Database.Models.World;

namespace ACE.Server.Entity.WorldObjects
{
    public class GenericObject : WorldObject
    {
        public GenericObject(Weenie weenie) : base(weenie)
        {
            Stuck = true;
            Attackable = true;
            
            SetObjectDescriptionBools();
        }

        ////public GenericObject(ObjectGuid guid, AceObject aceObject)
        ////    : base(guid, aceObject)
        ////{
        ////}

        ////public override void HandleActionOnCollide(ObjectGuid playerId)
        ////{
        ////    // TODO: Implement
        ////}

        ////public override void HandleActionOnUse(ObjectGuid playerId)
        ////{
        ////    // TODO: Implement
        ////}

        ////public override void OnUse(Session session)
        ////{
        ////    // TODO: Implement
        ////}        
    }
}
