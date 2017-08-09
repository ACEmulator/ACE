// WeenieType.Generic

using ACE.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;

namespace ACE.Entity
{
    public class GenericObject : WorldObject
    {
        public GenericObject(AceObject aceObject)
            : base(aceObject)
        {
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
