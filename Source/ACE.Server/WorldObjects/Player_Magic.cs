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
        /// Handles player targeted casting message
        /// </summary>
        public void HandleActionCastTargetedSpell(ObjectGuid guidTarget, uint spellId)
        {
            CastResult result = CreatePlayerSpell(guidTarget, spellId);

            switch (result)
            {
                case CastResult.BusyCasting:
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, errorType: WeenieError.YoureTooBusy));
                    break;
                case CastResult.SpellTargetInvalid:
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YourSpellTargetIsMissing));
                    break;
                case CastResult.InvalidSpell:
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDontKnowThatSpell));
                    break;
                case CastResult.SpellCastCompleted:
                    break;
                default:
                    Session.Network.EnqueueSend(new GameMessageSystemChat("Targeted SpellID " + spellId + " not yet implemented!", ChatMessageType.System));
                    break;
            }
        }

        /// <summary>
        /// Handles player untargeted casting message
        /// </summary>
        public void HandleActionCastUntargetedSpell(uint spellId)
        {
            CastResult result = CreatePlayerSpell(spellId);

            switch (result)
            {
                case CastResult.InvalidSpell:
                    Session.Network.EnqueueSend(new GameEventWeenieError(Session, WeenieError.YouDontKnowThatSpell));
                    break;
                case CastResult.SpellCastCompleted:
                    break;
                default:
                    Session.Network.EnqueueSend(new GameMessageSystemChat("UnTargeted SpellID " + spellId + " not yet implemented!", ChatMessageType.System));
                    break;
            }
        }
    }
}
