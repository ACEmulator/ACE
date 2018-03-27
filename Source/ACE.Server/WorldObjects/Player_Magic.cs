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
        public void HandleActionCastTargetedSpell(ObjectGuid guidTarget, uint spellId)
        {
            CastResult result = CreateSpell(Guid, guidTarget, spellId);

            switch (result)
            {
                case CastResult.SpellTargetInvalid:
                    var targetOutOfRangeMessage = new GameEventWeenieError(Session, WeenieError.YourSpellTargetIsMissing);
                    Session.Network.EnqueueSend(targetOutOfRangeMessage);
                    break;
                default:
                    string serverMessage = "Targeted SpellID " + spellId + " not yet implemented!";
                    var unImplementedMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                    Session.Network.EnqueueSend(unImplementedMessage);
                    break;
            }

            Session.Network.EnqueueSend(new GameEventUseDone(Session));
        }

        /// <summary>
        /// Method used for handling player untargeted casting
        /// </summary>
        public void HandleActionCastUntargetedSpell(uint spellId)
        {
            CastResult result = CreateSpell(Guid, null, spellId);
            switch (spellId)
            {
                default:
                    string serverMessage = "UnTargeted SpellID " + spellId + " not yet implemented!";
                    var unImplementedMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.System);
                    Session.Network.EnqueueSend(unImplementedMessage);
                    break;
            }

            Session.Network.EnqueueSend(new GameEventUseDone(Session));
        }
    }
}
