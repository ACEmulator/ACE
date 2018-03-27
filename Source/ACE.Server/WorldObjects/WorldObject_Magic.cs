using System;
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
                var targetOutOfRangeMessage = new GameEventDisplayStatusMessage(session, StatusMessageType1.YourSpellTargetIsMissing);
                session.Network.EnqueueSend(targetOutOfRangeMessage);
                return;
            }

            string serverMessage = "Targeted SpellID " + spellId + " not yet implemented!";
            var failedUsePortalMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
            session.Network.EnqueueSend(failedUsePortalMessage);
        }

        public void HandleActionCastUntargetedSpell(uint spellId, Session session)
        {
            string serverMessage = "UnTargeted SpellID " + spellId + " not yet implemented!";
            var failedUsePortalMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
            session.Network.EnqueueSend(failedUsePortalMessage);
        }
    }
}
