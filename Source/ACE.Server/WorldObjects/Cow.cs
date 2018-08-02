using ACE.Common;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    public class Cow : Creature
    {
        private static readonly UniversalMotion motionTipRight = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.TippedRight));

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Cow(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Cow(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            // TODO: we shouldn't be auto setting properties that come from our weenie by default
            UseRadius = 1;
        }

        private double? resetTimestamp;
        private double? ResetTimestamp
        {
            get { return resetTimestamp; }
            set { resetTimestamp = Time.GetTimestamp(); }
        }

        private double? useTimestamp;
        private double? UseTimestamp
        {
            get { return useTimestamp; }
            set { useTimestamp = Time.GetTimestamp(); }
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem, and is wrapped in ActionChain.<para />
        /// The actor of the ActionChain is the item being used.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(WorldObject worldObject)
        {
            if (worldObject is Player)
            {
                var player = worldObject as Player;
                ////if (playerDistanceTo >= 2500)
                ////{
                ////    var sendTooFarMsg = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.Enum_0037);
                ////    player.Session.Network.EnqueueSend(sendTooFarMsg, sendUseDoneEvent);
                ////    return;
                ////}

                if (AllowedActivator == null)
                {
                    Activate(player.Guid);
                }

                var sendUseDoneEvent = new GameEventUseDone(player.Session);
                player.Session.Network.EnqueueSend(sendUseDoneEvent);
            }
        }

        private void Activate(ObjectGuid activator = new ObjectGuid())
        {       
            AllowedActivator = activator.Full;

            CurrentLandblock?.EnqueueBroadcastMotion(this, motionTipRight);
            
            // Stamp Cow tipping quest here;

            ActionChain autoResetTimer = new ActionChain();
            autoResetTimer.AddDelaySeconds(4);
            autoResetTimer.AddAction(this, () => ResetCow());
            autoResetTimer.EnqueueChain();

            if (activator.Full > 0)
                UseTimestamp++;
        }

        private void ResetCow()
        {
            AllowedActivator = null;

            ResetTimestamp++;
        }
    }
}
