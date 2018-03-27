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
                var targetOutOfRangeMessage = new GameEventWeenieError(session, WeenieError.YourSpellTargetIsMissing);
                session.Network.EnqueueSend(targetOutOfRangeMessage);
                return;
            }

            switch (spellId)
            {
                default:
                    string serverMessage = "Targeted SpellID " + spellId + " not yet implemented!";
                    var failedUsePortalMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                    session.Network.EnqueueSend(failedUsePortalMessage);
                    break;
            }

            session.Network.EnqueueSend(new GameEventUseDone(session));
        }

        public void HandleActionCastUntargetedSpell(uint spellId, Session session)
        {
            switch (spellId)
            {
                default:
                    string serverMessage = "UnTargeted SpellID " + spellId + " not yet implemented!";
                    var failedUsePortalMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                    session.Network.EnqueueSend(failedUsePortalMessage);
                    break;
            }

            session.Network.EnqueueSend(new GameEventUseDone(session));
        }
    }
}
