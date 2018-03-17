using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    public class Chest : Container
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Chest(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Chest(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            //BaseDescriptionFlags |= ObjectDescriptionFlag.Door;

            //if (!DefaultOpen)
            //{
            //    CurrentMotionState = motionStateClosed;
            //    IsOpen = false;
            //    //Ethereal = false;
            //}
            //else
            //{
            //    CurrentMotionState = motionStateOpen;
            //    IsOpen = true;
            //    //Ethereal = true;
            //}

            ContainerCapacity = 10;
            ItemCapacity = 120;

            CurrentMotionState = motionStateClosed; // What chest defaults to open?

            if (UseRadius < 2)
                UseRadius = 2; // Until DoMoveTo (Physics, Indoor/Outside range variance) is smarter, use 2 is safest.
        }

        private static readonly UniversalMotion motionOpen = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.On));
        private static readonly UniversalMotion motionClosed = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Off));

        private static readonly MotionState motionStateOpen = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.On));
        private static readonly MotionState motionStateClosed = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Off));

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

            if (!player.IsWithinUseRadiusOf(this) && Viewer != player.Guid.Full)
                player.DoMoveTo(this);
            else
            {
                if (!(IsLocked ?? false))
                {
                    if (!(IsOpen ?? false))
                    {
                        var turnToMotion = new UniversalMotion(MotionStance.Standing, Location, Guid);
                        turnToMotion.MovementTypes = MovementTypes.TurnToObject;

                        ActionChain turnToTimer = new ActionChain();
                        turnToTimer.AddAction(this, () => player.CurrentLandblock.EnqueueBroadcastMotion(player, turnToMotion)); ;
                        turnToTimer.AddDelaySeconds(1);
                        turnToTimer.AddAction(this, () => Open(player));
                        turnToTimer.EnqueueChain();

                        return;
                    }
                    else
                    {
                        if (Viewer == player.Guid.Full)
                        {
                            Close(player);
                        }
                        // else error msg?
                    }
                }
                else
                {
                    CurrentLandblock.EnqueueBroadcastSound(this, Sound.OpenFailDueToLock);
                }
                player.SendUseDoneEvent();
            }
        }

        public override void Open(Player player)
        {
            if (IsOpen ?? false)
                return;

            IsOpen = true;
            CurrentLandblock.EnqueueBroadcastMotion(this, motionOpen);
            CurrentMotionState = motionStateOpen;
            Viewer = player.Guid.Full;
            player.Session.Network.EnqueueSend(new GameEventViewContents(player.Session, this));

            // send createobject msgs to player for all chest inventory here

            player.SendUseDoneEvent();
        }

        public override void Close(Player player)
        {
            if (!(IsOpen ?? false))
                return;

            CurrentLandblock.EnqueueBroadcastMotion(this, motionClosed);
            CurrentMotionState = motionStateClosed;
            player.Session.Network.EnqueueSend(new GameEventCloseGroundContainer(player.Session, this));
            // send removeobject msgs to player for all chest inventory here
            Viewer = null;
            IsOpen = false;

            //player.SendUseDoneEvent();
        }
    }
}
