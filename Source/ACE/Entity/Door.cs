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

        private string LongDesc
        {
            get;
            set;
        }

        private string Use
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
                Close(new ObjectGuid(0));
            else
                Open(new ObjectGuid(0));

            ResetTimestamp++;
        }
    }
}
