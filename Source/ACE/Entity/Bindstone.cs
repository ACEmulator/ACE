using ACE.Entity.Actions;
using ACE.Factories;
using System;
using System.Collections.Generic;

namespace ACE.Entity
{
    public class Bindstone : WorldObject
    {
        public Bindstone(AceObject aceObject)
            : base(aceObject)
        {
            Stuck = true; Attackable = true;

            SetObjectDescriptionBools();

            RadarBehavior = Enum.RadarBehavior.ShowAlways;
            RadarColor = Enum.RadarColor.LifeStone;
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
