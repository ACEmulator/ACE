using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Network.Enum;
using ACE.Network.GameEvent.Events;
using ACE.Network.Motion;
using ACE.Entity.Enum.Properties;
using System;
using ACE.DatLoader.FileTypes;

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
        }

        public bool Ethereal
        {
            get { return AceObject.GetBoolProperty(PropertyBool.Ethereal) ?? false; }
            set { AceObject.SetBoolProperty(PropertyBool.Ethereal, value); }
        }

        public bool IsOpen
        {
            get { return AceObject.GetBoolProperty(PropertyBool.Open) ?? false; }
            set { AceObject.SetBoolProperty(PropertyBool.Open, value); }
        }

        public bool IsLocked
        {
            get { return AceObject.GetBoolProperty(PropertyBool.Locked) ?? false; }
            set { AceObject.SetBoolProperty(PropertyBool.Locked, value); }
        }

        public bool DefaultLocked
        {
            get { return AceObject.GetBoolProperty(PropertyBool.DefaultLocked) ?? false; }
            set { AceObject.SetBoolProperty(PropertyBool.DefaultLocked, value); }
        }

        public bool DefaultOpen
        {
            get { return AceObject.GetBoolProperty(PropertyBool.DefaultOpen) ?? false; }
            set { AceObject.SetBoolProperty(PropertyBool.DefaultOpen, value); }
        }

        public float ResetInterval
        {
            get { return (float?)AceObject.GetDoubleProperty(PropertyDouble.ResetInterval) ?? 30.0f; }
            set { AceObject.SetDoubleProperty(PropertyDouble.ResetInterval, value); }
        }

        public double? ResetTimestamp
        {
            get { return AceObject.GetDoubleProperty(PropertyDouble.ResetTimestamp); }
            set { AceObject.SetDoubleTimestamp(PropertyDouble.ResetTimestamp); }
        }

        public double? UseTimestamp
        {
            get { return AceObject.GetDoubleProperty(PropertyDouble.UseTimestamp); }
            set { AceObject.SetDoubleTimestamp(PropertyDouble.UseTimestamp); }
        }

        public uint? LastUnlocker
        {
            get { return AceObject.GetInstanceIdProperty(PropertyInstanceId.LastUnlocker); }
            set { AceObject.SetInstanceIdProperty(PropertyInstanceId.LastUnlocker, value); }
        }

        public string KeyCode
        {
            get { return AceObject.GetStringProperty(PropertyString.KeyCode); }
            set { AceObject.SetStringProperty(PropertyString.KeyCode, value); }
        }

        public string LockCode
        {
            get { return AceObject.GetStringProperty(PropertyString.LockCode); }
            set { AceObject.SetStringProperty(PropertyString.LockCode, value); }
        }

        public uint? ResistLockpick
        {
            get { return AceObject.GetIntProperty(PropertyInt.ResistLockpick); }
            set { AceObject.SetIntProperty(PropertyInt.ResistLockpick, value); }
        }

        public uint? AppraisalLockpickSuccessPercent
        {
            get { return AceObject.GetIntProperty(PropertyInt.AppraisalLockpickSuccessPercent); }
            set { AceObject.SetIntProperty(PropertyInt.AppraisalLockpickSuccessPercent, value); }
        }

        public override void OnUse(ObjectGuid playerId)
        {
            // TODO: check if door is locked, send locked soundfx if locked and fail to open.

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

                ////var sendUseDoneEvent = new GameEventUseDone(player.Session);                

                ////if (playerDistanceTo >= 2500)
                ////{
                ////    var sendTooFarMsg = new GameEventDisplayStatusMessage(player.Session, StatusMessageType1.Enum_0037);
                ////    player.Session.Network.EnqueueSend(sendTooFarMsg, sendUseDoneEvent);
                ////    return;
                ////}

                if (playerDistanceTo >= radiusSquared)
                {
                    ActionChain moveToDoorChain = new ActionChain();

                    moveToDoorChain.AddChain(player.CreateMoveToChain(Guid, 0.1f));
                    moveToDoorChain.AddDelaySeconds(0.50);

                    moveToDoorChain.AddAction(this, () => OnUse(playerId));

                    moveToDoorChain.EnqueueChain();
                }
                else
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

                    var sendUseDoneEvent = new GameEventUseDone(player.Session);
                    player.Session.Network.EnqueueSend(sendUseDoneEvent);
                }
            });
            chain.EnqueueChain();
        }

        private void Open(ObjectGuid opener = new ObjectGuid())
        {
            CurrentLandblock.EnqueueBroadcastMotion(this, motionOpen);
            CurrentMotionState = motionStateOpen;
            PhysicsState |= PhysicsState.Ethereal;
            Ethereal = true;
            IsOpen = true;
            if (opener.Full > 0)
                UseTimestamp++;
        }

        private void Close(ObjectGuid closer = new ObjectGuid())
        {
            if (this.CurrentMotionState == motionStateClosed)
                return;

            CurrentLandblock.EnqueueBroadcastMotion(this, motionClosed);
            CurrentMotionState = motionStateClosed;
            PhysicsState ^= PhysicsState.Ethereal;
            Ethereal = false;
            IsOpen = false;
            if (closer.Full > 0)
                UseTimestamp++;
        }

        private void Reset()
        {
            TimeSpan span = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc));
            double timestamp = span.TotalSeconds;

            if ((timestamp - ResetTimestamp) < ResetInterval)
                return;

            if (!DefaultOpen)
                Close(new ObjectGuid(0));
            else
                Open(new ObjectGuid(0));

            ResetTimestamp++;
        }
    }
}
