// WeenieType.Lifestone

using ACE.Common;
using ACE.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;

namespace ACE.Entity
{
    public class Lifestone : WorldObject
    {
        private static readonly UniversalMotion motionSanctuary = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Sanctuary));

        public Lifestone(AceObject aceO)
            : base(aceO)
        {
            if (Use == null)
                Use = "Use this item to set your resurrection point.";
        }
       
        public override void HandleActionOnUse(ObjectGuid playerId)
        {
            ActionChain chain = new ActionChain();
            CurrentLandblock.ChainOnObject(chain, playerId, (WorldObject wo) =>
            {
                Player player = wo as Player;
                if (player == null)
                {
                    return;
                }

                if (!player.IsWithinUseRadiusOf(this))
                    player.DoMoveTo(this);
                else
                {
                    ActionChain useObjectChain = new ActionChain();

                    useObjectChain.AddAction(this, () =>
                    {
                        Activate(player);
                    });

                    useObjectChain.AddDelaySeconds(5); // TODO: get animation frames length and put that delay here

                    useObjectChain.AddAction(this, () => player.SetCharacterPosition(PositionType.Sanctuary, player.Location));

                    useObjectChain.AddAction(this, () => player.Session.Network.EnqueueSend(new GameMessageSystemChat("You have attuned your spirit to this Lifestone. You will resurrect here after you die.", ChatMessageType.Magic)));

                    var sendUseDoneEvent = new GameEventUseDone(player.Session);
                    useObjectChain.AddAction(this, () => player.Session.Network.EnqueueSend(sendUseDoneEvent));

                    useObjectChain.EnqueueChain();
                }
            });
            chain.EnqueueChain();
        }

        private void Activate(Player activator)
        {
            CurrentLandblock.EnqueueBroadcastMotion(activator, motionSanctuary);

            CurrentLandblock.EnqueueBroadcastSound(this, Sound.LifestoneOn); // This event was present for a pcap in the training dungeon.. Why? The sound already comes with animationEvent...

            if (activator.Guid.Full > 0)
                UseTimestamp++;
        }
    }
}
