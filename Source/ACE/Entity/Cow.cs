using System.Threading.Tasks;

using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;
using ACE.Common;

namespace ACE.Entity
{
    public class Cow : Creature
    {
        private static readonly UniversalMotion motionTipRight = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.TippedRight));

        public Cow()
        {
        }

        protected override async Task Init(AceObject aceO)
        {
            await base.Init(aceO);
            UseRadius = 1;
            IsAlive = true;
            SetupVitals();
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

        public override async Task ActOnUse(ObjectGuid playerId)
        {
            Player player = await CurrentLandblock.GetObject(playerId) as Player;
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
                await player.DoMoveTo(this);
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
