using ACE.Common;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.Motion;

namespace ACE.Server.Entity.WorldObjects
{
    public class Cow : Creature
    {
        private static readonly UniversalMotion motionTipRight = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.TippedRight));

        /// <summary>
        /// If biota is null, one will be created with default values for this WorldObject type.
        /// </summary>
        public Cow(Weenie weenie, Biota biota = null) : base(weenie, biota)
        {
            if (biota == null) // If no biota was passed our base will instantiate one, and we will initialize it with appropriate default values
            {
                // TODO we shouldn't be auto setting properties that come from our weenie by default
                Stuck = true;
                Attackable = true;

                UseRadius = 1;
                IsAlive = true;
                SetupVitals();
            }
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

        private uint? AllowedActivator
        {
            get;
            set;
        }

        public override void ActOnUse(ObjectGuid playerId)
        {
            Player player = CurrentLandblock.GetObject(playerId) as Player;
            if (player == null)
            {
                return;
            }

            ////if (playerDistanceTo >= 2500)
            ////{
            ////    var sendTooFarMsg = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.Enum_0037);
            ////    player.Session.Network.EnqueueSend(sendTooFarMsg, sendUseDoneEvent);
            ////    return;
            ////}

            if (!player.IsWithinUseRadiusOf(this))
                player.DoMoveTo(this);
            else
            {
                if (AllowedActivator == null)
                {
                    Activate(playerId);
                }

                var sendUseDoneEvent = new GameEventUseDone(player.Session);
                player.Session.Network.EnqueueSend(sendUseDoneEvent);
            }
        }

        private void Activate(ObjectGuid activator = new ObjectGuid())
        {       
            AllowedActivator = activator.Full;

            CurrentLandblock.EnqueueBroadcastMotion(this, motionTipRight);
            
            // Stamp Cow tipping quest here;

            ActionChain autoResetTimer = new ActionChain();
            autoResetTimer.AddDelaySeconds(4);
            autoResetTimer.AddAction(this, () => Reset());
            autoResetTimer.EnqueueChain();

            if (activator.Full > 0)
                UseTimestamp++;
        }

        private void Reset()
        {
            AllowedActivator = null;

            ResetTimestamp++;
        }
    }
}
