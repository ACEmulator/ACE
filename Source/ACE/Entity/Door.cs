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
    public class Door : UsableObject
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
            var weenie = Database.DatabaseManager.World.GetAceObjectByWeenie(AceObject.WeenieClassId);

            PhysicsState |= PhysicsState.HasPhysicsBsp | PhysicsState.ReportCollision;

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

            // If we had the base weenies this would be the way to go
            ////if (DefaultLocked)
            ////    IsLocked = true;
            ////else
            ////    IsLocked = false;

            // But since we don't know what doors were DefaultLocked, let's assume for now that any door that starts Locked should default as such.
            if (IsLocked)
                DefaultLocked = true;

            movementOpen.ForwardCommand = (ushort)MotionCommand.On;
            movementClosed.ForwardCommand = (ushort)MotionCommand.Off;

            if (AceObject.Use == null)
                Use = weenie.Use;
            if (AceObject.UseMessage == null)
                UseMessage = weenie.UseMessage;
            if (AceObject.LongDesc == null)
                LongDesc = weenie.LongDesc;
            if (AceObject.ShortDesc == null)
                ShortDesc = weenie.ShortDesc;
        }

        public bool Ethereal
        {
            get;
            private set;
        }

        public bool IsOpen
        {
            get;
            private set;
        }

        public bool IsLocked
        {
            get;
            private set;
        }

        public bool DefaultLocked
        {
            get;
            private set;
        }

        public bool DefaultOpen
        {
            get;
            private set;
        }

        public float ResetInterval
        {
            get;
            private set;
        }

        private double? resetTimestamp;
        public double? ResetTimestamp
        {
            get { return resetTimestamp; }
            private set { resetTimestamp = Time.GetTimestamp(); }
        }

        private double? useTimestamp;
        public double? UseTimestamp
        {
            get { return useTimestamp; }
            private set { useTimestamp = Time.GetTimestamp(); }
        }

        private double? useLockTimestamp;
        public double? UseLockTimestamp
        {
            get { return useLockTimestamp; }
            private set { useLockTimestamp = Time.GetTimestamp(); }
        }

        public uint? LastUnlocker
        {
            get;
            private set;
        }

        public string KeyCode
        {
            get;
            private set;
        }

        public string LockCode
        {
            get;
            private set;
        }

        public string ShortDesc
        {
            get;
            private set;
        }

        public string LongDesc
        {
            get;
            private set;
        }

        public string Use
        {
            get;
            private set;
        }

        public string UseMessage
        {
            get;
            private set;
        }

        public uint? ResistLockpick
        {
            get;
            private set;
        }

        public uint? AppraisalLockpickSuccessPercent
        {
            get;
            private set;
        }

        public override void OnUse(ObjectGuid playerId)
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
                    ActionChain moveToDoorChain = new ActionChain();

                    moveToDoorChain.AddChain(player.CreateMoveToChain(Guid, 0.2f));
                    moveToDoorChain.AddDelaySeconds(0.50);

                    moveToDoorChain.AddAction(this, () => OnUse(playerId));

                    moveToDoorChain.EnqueueChain();
                }
                else
                {
                    bool isLocked;
                    bool isOpen;

                    ActionChain checkDoorChain = new ActionChain();
                    CurrentLandblock.ChainOnObject(checkDoorChain, Guid, (WorldObject dwo) =>
                    {
                        Door door = dwo as Door;
                        if (door == null)
                        {
                            return;
                        }
                        isLocked = door.IsLocked;
                        isOpen = door.IsOpen;

                        if (!isLocked)
                        {
                            if (!isOpen)
                            {
                                door.Open(playerId);
                            }
                            else
                            {
                                door.Close(playerId);
                            }

                            // Create Door auto close timer
                            ActionChain autoCloseTimer = new ActionChain();
                            autoCloseTimer.AddDelaySeconds(ResetInterval);
                            autoCloseTimer.AddAction(this, () => door.Reset());
                            autoCloseTimer.EnqueueChain();
                        }
                        else
                        {
                            CurrentLandblock.EnqueueBroadcastSound(this, Sound.OpenFailDueToLock);
                        }
                    });
                    checkDoorChain.EnqueueChain();

                    ////if (!IsLocked)
                    ////{
                    ////    if (!IsOpen)
                    ////    {
                    ////        Open(playerId);
                    ////    }
                    ////    else
                    ////    {
                    ////        Close(playerId);
                    ////    }

                    ////    // Create Door auto close timer
                    ////    ActionChain autoCloseTimer = new ActionChain();
                    ////    autoCloseTimer.AddDelaySeconds(ResetInterval);
                    ////    autoCloseTimer.AddAction(this, () => Reset());
                    ////    autoCloseTimer.EnqueueChain();
                    ////}
                    ////else
                    ////{
                    ////    CurrentLandblock.EnqueueBroadcastSound(this, Sound.OpenFailDueToLock);
                    ////}

                    var sendUseDoneEvent = new GameEventUseDone(player.Session);
                    player.Session.Network.EnqueueSend(sendUseDoneEvent);
                }
            });
            chain.EnqueueChain();
        }

        private void Open(ObjectGuid opener = new ObjectGuid())
        {
            ActionChain chain = new ActionChain();
            CurrentLandblock.ChainOnObject(chain, Guid, (WorldObject wo) =>
            {
                if (CurrentMotionState == motionStateOpen)
                    return;

                CurrentLandblock.EnqueueBroadcastMotion(this, motionOpen);
                CurrentMotionState = motionStateOpen;
                PhysicsState |= PhysicsState.Ethereal;
                Ethereal = true;
                IsOpen = true;
                if (opener.Full > 0)
                    UseTimestamp++;
            });
            chain.EnqueueChain();
        }

        private void Close(ObjectGuid closer = new ObjectGuid())
        {
            ActionChain chain = new ActionChain();
            CurrentLandblock.ChainOnObject(chain, Guid, (WorldObject wo) =>
            {
                if (CurrentMotionState == motionStateClosed)
                    return;

                CurrentLandblock.EnqueueBroadcastMotion(this, motionClosed);
                CurrentMotionState = motionStateClosed;
                PhysicsState ^= PhysicsState.Ethereal;
                Ethereal = false;
                IsOpen = false;
                if (closer.Full > 0)
                    UseTimestamp++;
            });
            chain.EnqueueChain();
        }

        private void Reset()
        {
            if ((Time.GetTimestamp() - UseTimestamp) < ResetInterval)
                return;

            if (!DefaultOpen)
                Close(new ObjectGuid(0));
            else
                Open(new ObjectGuid(0));

            ResetTimestamp++;
        }
    }
}
