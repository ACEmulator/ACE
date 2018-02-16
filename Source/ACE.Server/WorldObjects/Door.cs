using System.Collections.Generic;

using ACE.Common;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    public class Door : WorldObject
    {
        private static List<AceObjectPropertyId> _updateLocked = new List<AceObjectPropertyId>() { new AceObjectPropertyId((uint)PropertyBool.Locked, AceObjectPropertyType.PropertyBool) };

        public enum UnlockDoorResults : ushort
        {
            UnlockSuccess   = 0,
            PickLockFailed  = 1,
            IncorrectKey    = 2,
            AlreadyUnlocked = 3,
            CannotBePicked  = 4,
            DoorOpen        = 5
        }

        private static readonly MovementData movementOpen = new MovementData();
        private static readonly MovementData movementClosed = new MovementData();

        private static readonly MotionState motionStateOpen = new UniversalMotion(MotionStance.Standing, movementOpen);
        private static readonly MotionState motionStateClosed = new UniversalMotion(MotionStance.Standing, movementClosed);

        private static readonly UniversalMotion motionOpen = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.On));
        private static readonly UniversalMotion motionClosed = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Off));

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Door(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Door(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            // TODO we shouldn't be auto setting properties that come from our weenie by default

            Door = true;
            Stuck = true;
            Attackable = true;

            // This likely will change to be based on reading a dat file to determine if this exists...
            HasPhysicsBsp = true;

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
            get { return AceObject.Locked ?? false; }
            set { AceObject.Locked = value; }
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

        private int? ResistLockpick
        {
            get;
            set;
        }

        private int? AppraisalLockpickSuccessPercent
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
        }

        private void Open(ObjectGuid opener = new ObjectGuid())
        {
            if (CurrentMotionState == motionStateOpen)
                return;

            CurrentLandblock.EnqueueBroadcastMotion(this, motionOpen);
            CurrentMotionState = motionStateOpen;
            Ethereal = true;
            IsOpen = true;
            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(this.Sequences, PropertyBool.Ethereal, Ethereal));
            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(this.Sequences, PropertyBool.Open, IsOpen));
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
            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(this.Sequences, PropertyBool.Ethereal, Ethereal));
            CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(this.Sequences, PropertyBool.Open, IsOpen));
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
                    CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(this.Sequences, PropertyBool.Locked, IsLocked));
                    // CurrentLandblock.EnqueueBroadcastSound(this, Sound.LockSuccess); // TODO: need to find the lock sound
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
        public UnlockDoorResults UnlockDoor(uint playerLockpickSkillLvl)
        {
            if (ResistLockpick == 0)
                return UnlockDoorResults.CannotBePicked;

            if (playerLockpickSkillLvl >= ResistLockpick)
            {
                if (!IsLocked)
                    return UnlockDoorResults.AlreadyUnlocked;

                IsLocked = false;
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(this.Sequences, PropertyBool.Locked, IsLocked));
                CurrentLandblock.EnqueueBroadcastSound(this, Sound.LockSuccess);
                return UnlockDoorResults.UnlockSuccess;
            }

            return UnlockDoorResults.PickLockFailed;
        }

        /// <summary>
        /// Used for unlocking a door via a key
        /// </summary>
        public UnlockDoorResults UnlockDoor(string keyCode)
        {
            if (IsOpen)
                return UnlockDoorResults.DoorOpen;

            if (keyCode == LockCode)
            {
                if (!IsLocked)
                    return UnlockDoorResults.AlreadyUnlocked;

                IsLocked = false;
                CurrentLandblock.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(this.Sequences, PropertyBool.Locked, IsLocked));
                CurrentLandblock.EnqueueBroadcastSound(this, Sound.LockSuccess);
                return UnlockDoorResults.UnlockSuccess;
            }

            return UnlockDoorResults.IncorrectKey;
        }
    }
}
