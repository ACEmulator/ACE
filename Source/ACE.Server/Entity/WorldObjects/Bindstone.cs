using ACE.Database.Models.Shard;
using ACE.Database.Models.World;

namespace ACE.Server.Entity.WorldObjects
{
    public class Bindstone : WorldObject
    {
        public Bindstone(Weenie weenie) : base(weenie)
        {
            Stuck = true;
            Attackable = true;

            SetObjectDescriptionBools();

            RadarBehavior = ACE.Entity.Enum.RadarBehavior.ShowAlways;
            RadarColor = ACE.Entity.Enum.RadarColor.LifeStone;
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
