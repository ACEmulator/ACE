using ACE.Database.Models.Shard;
using ACE.Database.Models.World;

namespace ACE.Server.Entity.WorldObjects
{
    public class Bindstone : WorldObject
    {
        /// <summary>
        /// If biota is null, one will be created with default values for this WorldObject type.
        /// </summary>
        public Bindstone(Weenie weenie, Biota biota = null) : base(weenie, biota)
        {
            // TODO we shouldn't be auto setting properties that come from our weenie by default

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
