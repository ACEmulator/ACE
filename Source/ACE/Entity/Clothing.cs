using ACE.Entity.Actions;
using ACE.Factories;
using System;
using System.Collections.Generic;

namespace ACE.Entity
{
    public class Clothing : WorldObject
    {
        public Clothing(AceObject aceObject)
            : base(aceObject)
        {
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
