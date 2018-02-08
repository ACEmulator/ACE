using ACE.Entity;

namespace ACE.Server.Entity
{
    public class Bindstone : WorldObject
    {
        public Bindstone(AceObject aceObject)
            : base(aceObject)
        {
            Stuck = true; Attackable = true;

            SetObjectDescriptionBools();

            RadarBehavior = global::ACE.Entity.Enum.RadarBehavior.ShowAlways;
            RadarColor = global::ACE.Entity.Enum.RadarColor.LifeStone;
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
