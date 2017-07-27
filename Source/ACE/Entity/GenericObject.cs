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

        public GenericObject(ObjectGuid guid, AceObject aceObject)
            : base(guid, aceObject)
        {
        }

        ////public override void OnCollide(ObjectGuid playerId)
        ////{
        ////    // TODO: Implement
        ////}

        public override void OnUse(ObjectGuid playerId)
        {
            // TODO: Implement

            if (CurrentLandblock != null)
            {
                ActionChain chain = new ActionChain();
                CurrentLandblock.ChainOnObject(chain, playerId, (WorldObject wo) =>
                {
                    Player player = wo as Player;
                    if (player == null)
                    {
                        return;
                    }

                    var errorMessage = new GameMessageSystemChat("Generic OnUse reached, this item not programmed yet", ChatMessageType.System);
                    var sendUseDoneEvent = new GameEventUseDone(player.Session);

                    player.Session.Network.EnqueueSend(errorMessage, sendUseDoneEvent);
                });
                chain.EnqueueChain();
            }
        }

        public override void InternalOnUse(Session session)
        {
            var errorMessage = new GameMessageSystemChat("Generic InternalOnUse reached, this item not programmed yet", ChatMessageType.System);
            var sendUseDoneEvent = new GameEventUseDone(session);

            session.Network.EnqueueSend(errorMessage, sendUseDoneEvent);
        }
    }
}
