using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;
using ACE.Entity.Enum.Properties;
using System;
using ACE.DatLoader.FileTypes;
using ACE.Network.GameMessages.Messages;
using ACE.Common;

namespace ACE.Entity
{
    public class Cow : Creature
    {
        private static readonly UniversalMotion motionTipRight = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.TippedRight));

        public Cow(AceObject aceO)
            : base(aceO)
        {
            var weenie = Database.DatabaseManager.World.GetAceObjectByWeenie(AceObject.WeenieClassId);

            // TODO: Loading creatures out of world database does not the data for their attributes. FIXME
            // Strength.Base = weenie.Strength;

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

                SetupModel csetup = SetupModel.ReadFromDat(SetupTableId.Value);
                float radiusSquared = (UseRadius.Value + csetup.Radius) * (UseRadius.Value + csetup.Radius);
                float playerDistanceTo = player.Location.SquaredDistanceTo(Location);

                ////if (playerDistanceTo >= 2500)
                ////{
                ////    var sendTooFarMsg = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.Enum_0037);
                ////    player.Session.Network.EnqueueSend(sendTooFarMsg, sendUseDoneEvent);
                ////    return;
                ////}

                if (playerDistanceTo >= radiusSquared)
                {
                    ActionChain moveToObjectChain = new ActionChain();

                    moveToObjectChain.AddChain(player.CreateMoveToChain(Guid, 0.2f));
                    moveToObjectChain.AddDelaySeconds(0.50);

                    moveToObjectChain.AddAction(this, () => HandleActionOnUse(playerId));

                    moveToObjectChain.EnqueueChain();
                }
                else
                {
                    ActionChain useObjectChain = new ActionChain();

                    useObjectChain.AddAction(this, () =>
                    {
                        if (AllowedActivator == null)
                        {
                            Activate(playerId);
                        }

                        var sendUseDoneEvent = new GameEventUseDone(player.Session);
                        player.Session.Network.EnqueueSend(sendUseDoneEvent);
                    });

                    useObjectChain.EnqueueChain();
                }
            });
            chain.EnqueueChain();
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
