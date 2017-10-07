using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;
using ACE.Common;

namespace ACE.Entity
{
    public class Door : WorldObject
    {
        private static readonly MovementData movementOpen = new MovementData();
        private static readonly MovementData movementClosed = new MovementData();

        private static readonly MotionState motionStateOpen = new UniversalMotion(MotionStance.Standing, movementOpen);
        private static readonly MotionState motionStateClosed = new UniversalMotion(MotionStance.Standing, movementClosed);

        private static readonly UniversalMotion motionOpen = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.On));
        private static readonly UniversalMotion motionClosed = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Off));

        public Door(AceObject aceO)
            : base(aceO)
        {
            // Set PhysicsState defaults.. Leaving commented for now to read in what was pcapped
            // PhysicsState = 0;
            // ReportCollision = true;
            // HasPhysicsBsp = true;

            if (!DefaultOpen)
            {
                CurrentMotionState = motionStateClosed;
                IsOpen = false;
                Ethereal = false;
            }
            else
            {
                CurrentMotionState = motionStateOpen;
                IsOpen = true;
                Ethereal = true;
            }

            IsLocked = AceObject.Locked ?? false;
            ResetInterval = AceObject.ResetInterval ?? 30.0f;
            ResistLockpick = AceObject.ResistLockpick ?? 0;
            LockCode = AceObject.LockCode ?? "";

            // If we had the base weenies this would be the way to go
            ////if (DefaultLocked)
            ////    IsLocked = true;
            ////else
            ////    IsLocked = false;

            // But since we don't know what doors were DefaultLocked, let's assume for now that any door that starts Locked should default as such.
            if (IsLocked)
                DefaultLocked = true;

            movementOpen.ForwardCommand = (uint)MotionCommand.On;
            movementClosed.ForwardCommand = (uint)MotionCommand.Off;

            if (UseRadius < 2)
                UseRadius = 2;
        }

        private bool IsOpen
        {
            get;
            set;
        }

        private bool IsLocked
        {
            get;
            set;
        }

        private bool DefaultLocked
        {
            get;
            set;
        }

        private bool DefaultOpen
        {
            get;
            set;
        }

        private float ResetInterval
        {
            get;
            set;
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

        private double? useLockTimestamp;
        private double? UseLockTimestamp
        {
            get { return useLockTimestamp; }
            set { useLockTimestamp = Time.GetTimestamp(); }
        }

        private uint? LastUnlocker
        {
            get;
            set;
        }

        private string KeyCode
        {
            get;
            set;
        }

        private string LockCode
        {
            get;
            set;
        }

        private string ShortDesc
        {
            get;
            set;
        }

        private string UseMessage
        {
            get;
            set;
        }

        private uint? ResistLockpick
        {
            get;
            set;
        }

        private uint? AppraisalLockpickSuccessPercent
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
                    ActionChain checkDoorChain = new ActionChain();

                    checkDoorChain.AddAction(this, () =>
                    {
                        if (!IsLocked)
                        {
                            if (!IsOpen)
                            {
                                Open(playerId);
                            }
                            else
                            {
                                Close(playerId);
                            }

                            // Create Door auto close timer
                            ActionChain autoCloseTimer = new ActionChain();
                            autoCloseTimer.AddDelaySeconds(ResetInterval);
                            autoCloseTimer.AddAction(this, () => Reset());
                            autoCloseTimer.EnqueueChain();
                        }
                        else
                        {
                            CurrentLandblock.EnqueueBroadcastSound(this, Sound.OpenFailDueToLock);
                        }

                        var sendUseDoneEvent = new GameEventUseDone(player.Session);
                        player.Session.Network.EnqueueSend(sendUseDoneEvent);
                    });

                    checkDoorChain.EnqueueChain();
                }
            });
            chain.EnqueueChain();
        }

        private void Open(ObjectGuid opener = new ObjectGuid())
        {
            if (CurrentMotionState == motionStateOpen)
                return;

            CurrentLandblock.EnqueueBroadcastMotion(this, motionOpen);
            CurrentMotionState = motionStateOpen;
            Ethereal = true;
            IsOpen = true;
            if (opener.Full > 0)
                UseTimestamp++;
        }

        private void Close(ObjectGuid closer = new ObjectGuid())
        {
            if (CurrentMotionState == motionStateClosed)
                return;

            CurrentLandblock.EnqueueBroadcastMotion(this, motionClosed);
            CurrentMotionState = motionStateClosed;
            Ethereal = false;
            IsOpen = false;
            if (closer.Full > 0)
                UseTimestamp++;
        }

        private void Reset()
        {
            if ((Time.GetTimestamp() - UseTimestamp) < ResetInterval)
                return;

            if (!DefaultOpen)
            {
                Close(ObjectGuid.Invalid);
                if (DefaultLocked)
                {
                    IsLocked = true;
                }
            }
            else
                Open(ObjectGuid.Invalid);

            ResetTimestamp++;
        }

        /// <summary>
        /// Used for unlocking a door via lockpick, so contains a skill check
        /// player.Skills[Skill.Lockpick].ActiveValue should be sent for the skill check
        /// </summary>
        public bool UnlockDoor(uint playerLockpickSkillLvl)
        {
            if (playerLockpickSkillLvl >= ResistLockpick)
            {
                // LastUnlocker = 
                IsLocked = false;
                CurrentLandblock.EnqueueBroadcastSound(this, Sound.LockSuccess);
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Used for unlocking a door via a key
        /// </summary>
        public bool UnlockDoor(string keyCode)
        {
            if (keyCode == LockCode)
            {
                // LastUnlocker = 
                IsLocked = false;
                CurrentLandblock.EnqueueBroadcastSound(this, Sound.LockSuccess);
                return true;
            }
            else
                return false;
        }
    }
}
