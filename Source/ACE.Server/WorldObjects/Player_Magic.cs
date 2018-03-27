using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// Method used for handling player targeted casting
        /// </summary>
        public void HandleActionCastTargetedSpell(ObjectGuid guid, uint spellId, Session session)
        {
            CastResult result = CreateSpell(guid, spellId);

            switch (result)
            {
                case CastResult.SpellTargetInvalid:
                    var targetOutOfRangeMessage = new GameEventWeenieError(session, WeenieError.YourSpellTargetIsMissing);
                    session.Network.EnqueueSend(targetOutOfRangeMessage);
                    break;
                default:
                    string serverMessage = "Targeted SpellID " + spellId + " not yet implemented!";
                    var unImplementedMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                    session.Network.EnqueueSend(unImplementedMessage);
                    break;
            }

            session.Network.EnqueueSend(new GameEventUseDone(session));
        }

        /// <summary>
        /// Method used for handling player untargeted casting
        /// </summary>
        public void HandleActionCastUntargetedSpell(uint spellId, Session session)
        {
            CastResult result = CreateSpell(null, spellId);
            switch (spellId)
            {
                default:
                    string serverMessage = "UnTargeted SpellID " + spellId + " not yet implemented!";
                    var unImplementedMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                    session.Network.EnqueueSend(unImplementedMessage);
                    break;
            }

            session.Network.EnqueueSend(new GameEventUseDone(session));
        }
    }
}
