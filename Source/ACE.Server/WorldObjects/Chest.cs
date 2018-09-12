using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.Motion;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public class Chest : Container, Lock
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

            CurrentMotionState = motionClosed; // What chest defaults to open?

            if (UseRadius < 2)
                UseRadius = 2; // Until DoMoveTo (Physics, Indoor/Outside range variance) is smarter, use 2 is safest.
        }

        private static readonly UniversalMotion motionOpen = new UniversalMotion(MotionStance.NonCombat, new MotionItem(MotionCommand.On));
        private static readonly UniversalMotion motionClosed = new UniversalMotion(MotionStance.NonCombat, new MotionItem(MotionCommand.Off));

        //private static readonly MotionState motionStateOpen = new UniversalMotion(MotionStance.NonCombat, new MotionItem(MotionCommand.On));
        //private static readonly MotionState motionStateClosed = new UniversalMotion(MotionStance.NonCombat, new MotionItem(MotionCommand.Off));

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
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

                if (!(IsLocked ?? false))
                {
                    if (!(IsOpen ?? false))
                    {
                        var turnToMotion = new UniversalMotion(MotionStance.NonCombat, Location, Guid);
                        turnToMotion.MovementTypes = MovementTypes.TurnToObject;

                        ActionChain turnToTimer = new ActionChain();
                        turnToTimer.AddAction(this, () => player.EnqueueBroadcastMotion(turnToMotion));
                        turnToTimer.AddDelaySeconds(1);
                        turnToTimer.AddAction(this, () => Open(player));
                        turnToTimer.EnqueueChain();

                        return;
                    }

                    if (Viewer == player.Guid.Full)
                        Close(player);

                    // else error msg?
                }
                else
                {
                    player.Session.Network.EnqueueSend(new GameEventCommunicationTransientString(player.Session, $"The {Name} is locked!"));
                    EnqueueBroadcast(new GameMessageSound(Guid, Sound.OpenFailDueToLock, 1.0f));
                }

                player.SendUseDoneEvent();
            }
        }

        protected override void DoOnOpenMotionChanges()
        {
            EnqueueBroadcastMotion(motionOpen);
            CurrentMotionState = motionOpen;
        }

        protected override void DoOnCloseMotionChanges()
        {
            EnqueueBroadcastMotion(motionClosed);
            CurrentMotionState = motionClosed;
        }

        public string LockCode
        {
            get => GetProperty(PropertyString.LockCode);
            set { if (value == null) RemoveProperty(PropertyString.LockCode); else SetProperty(PropertyString.LockCode, value); }
        }

        public int? ResistLockpick
        {
            get => GetProperty(PropertyInt.ResistLockpick);
            set { if (!value.HasValue) RemoveProperty(PropertyInt.ResistLockpick); else SetProperty(PropertyInt.ResistLockpick, value.Value); }
        }

        /// <summary>
        /// Used for unlocking a chest via lockpick, so contains a skill check
        /// player.Skills[Skill.Lockpick].Current should be sent for the skill check
        /// </summary>
        public UnlockResults Unlock(uint playerLockpickSkillLvl, ref int difficulty)
        {
            return LockHelper.Unlock(this, playerLockpickSkillLvl, ref difficulty);
        }

        /// <summary>
        /// Used for unlocking a chest via a key
        /// </summary>
        public UnlockResults Unlock(string keyCode)
        {
            return LockHelper.Unlock(this, keyCode);
        }
    }
}
