using ACE.Database.Models.Shard;
using ACE.Database.Models.World;

namespace ACE.Server.Entity.WorldObjects
{
    public class Clothing : WorldObject
    {
        public Clothing(Weenie weenie) : base(weenie)
        {
            return;
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
