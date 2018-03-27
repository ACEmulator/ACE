using System;
using System.Collections.Generic;
using System.Text;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public void HandleActionCastTargetedSpell(ObjectGuid guid, uint spellId, Session session)
        {
            WeenieType? spellTarget = CurrentLandblock.GetObject(guid).WeenieType;

            if (spellTarget == null)
            {
                var targetOutOfRangeMessage = new GameEventDisplayStatusMessage(session.Player.Session, StatusMessageType1.YourSpellTargetIsMissing);
                session.Player.Session.Network.EnqueueSend(targetOutOfRangeMessage);
                return;
            }
        }
    }
}
