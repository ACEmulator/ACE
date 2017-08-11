// WeenieType.Lifestone

using ACE.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using System.Collections.Generic;
using System.IO;

namespace ACE.Entity
{
    public class Lifestone : WorldObject
    {
        private static readonly UniversalMotion motionSanctuary = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Sanctuary));

        private const IdentifyResponseFlags idFlags = IdentifyResponseFlags.StringStatsTable;

        public Lifestone(AceObject aceObject)
            : base(aceObject)
        {
            if (Use == null)
                Use = "Use this item to set your resurrection point.";

            var useString = new AceObjectPropertiesString();
            useString.AceObjectId = Guid.Full;
            useString.PropertyId = (ushort)PropertyString.Use;
            useString.PropertyValue = Use;
            lifestonePropertiesString.Add(useString);
        }

        private List<AceObjectPropertiesString> lifestonePropertiesString = new List<AceObjectPropertiesString>();
        
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

                    useObjectChain.AddAction(player, () =>
                    {
                        Activate(player);
                    });

                    useObjectChain.AddDelaySeconds(5); // TODO: get animation frames length and put that delay here

                    useObjectChain.AddAction(player, () =>
                    {
                        var sendUseDoneEvent = new GameEventUseDone(player.Session);
                        if (player.IsWithinUseRadiusOf(this))
                        {
                            player.SetCharacterPosition(PositionType.Sanctuary, player.Location);
                            var msg = new GameMessageSystemChat("You have attuned your spirit to this Lifestone. You will resurrect here after you die.", ChatMessageType.Magic);
                            player.Session.Network.EnqueueSend(msg, sendUseDoneEvent);
                        }
                        else
                        {
                            var failMsg = new GameMessageSystemChat("You wandered too far to attune with the Lifestone!", ChatMessageType.Magic);
                            player.Session.Network.EnqueueSend(failMsg, sendUseDoneEvent);
                        }
                    });

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

        public override void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        {
            WriteIdentifyObjectHeader(writer, idFlags, true); // Always succeed in assessing a lifestone.
            WriteIdentifyObjectStringsProperties(writer, idFlags, lifestonePropertiesString);
        }
    }
}
