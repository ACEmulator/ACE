using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Network.Enum;
using ACE.DatLoader.FileTypes;

namespace ACE.Entity
{
    public class Lifestone : WorldObject
    {
        public Lifestone(AceObject aceO)
            : base(aceO)
        {
            LifeStone = true;
            Stuck = true; Attackable = true;
            
            SetObjectDescriptionBools();
            
            RadarColor = Enum.RadarColor.LifeStone;
        }

        public override void ActOnUse(ObjectGuid playerId)
        {
            // All data on a lifestone is constant -- therefore we just run in context of player
            Player player = CurrentLandblock.GetObject(playerId) as Player;
            string serverMessage = null;
            // validate within use range, taking into account the radius of the model itself, as well
            SetupModel csetup = SetupModel.ReadFromDat(SetupTableId.Value);
            float radiusSquared = (UseRadius.Value + csetup.Radius) * (UseRadius.Value + csetup.Radius);

            // Run this animation...
            // Player Enqueue:
            var motionSanctuary = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Sanctuary));

            var animationEvent = new GameMessageUpdateMotion(player.Guid,
                                                             player.Sequences.GetCurrentSequence(Network.Sequence.SequenceType.ObjectInstance),
                                                             player.Sequences, motionSanctuary);

            // This event was present for a pcap in the training dungeon.. Why? The sound comes with animationEvent...
            var soundEvent = new GameMessageSound(Guid, Sound.LifestoneOn, 1);

            if (player.Location.SquaredDistanceTo(Location) >= radiusSquared)
            {
                serverMessage = "You wandered too far to attune with the Lifestone!";
            }
            else
            {
                player.SetCharacterPosition(PositionType.Sanctuary, player.Location);

                // create the outbound server message
                serverMessage = "You have attuned your spirit to this Lifestone. You will resurrect here after you die.";
                player.HandleActionMotion(motionSanctuary);
                player.Session.Network.EnqueueSend(soundEvent);
            }

            var lifestoneBindMessage = new GameMessageSystemChat(serverMessage, ChatMessageType.Magic);
            // always send useDone event
            var sendUseDoneEvent = new GameEventUseDone(player.Session);
            player.Session.Network.EnqueueSend(lifestoneBindMessage, sendUseDoneEvent);
        }
    }
}
