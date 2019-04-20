using System;
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

namespace ACE.Server.WorldObjects
{
    public class Door : WorldObject, Lock
    {
        private static readonly Motion motionOpen = new Motion(MotionStance.NonCombat, MotionCommand.On);
        private static readonly Motion motionClosed = new Motion(MotionStance.NonCombat, MotionCommand.Off);

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
            BaseDescriptionFlags |= ObjectDescriptionFlag.Door;

            if (!DefaultOpen)
            {
                CurrentMotionState = motionClosed;
                IsOpen = false;
                //Ethereal = false;
            }
            else
            {
                CurrentMotionState = motionOpen;
                IsOpen = true;
                //Ethereal = true;
            }

            ResetInterval = ResetInterval ?? 30.0f;
            ResistLockpick = ResistLockpick ?? 0;
            LockCode = LockCode ?? "";

            // If we had the base weenies this would be the way to go
            ////if (DefaultLocked)
            ////    IsLocked = true;
            ////else
            ////    IsLocked = false;

            // But since we don't know what doors were DefaultLocked, let's assume for now that any door that starts Locked should default as such.
            if (IsLocked)
                DefaultLocked = true;
        }

        private double? useLockTimestamp;
        private double? UseLockTimestamp
        {
            get { return useLockTimestamp; }
            set => useLockTimestamp = Time.GetUnixTime();
        }

        public string LockCode
        {
            get => GetProperty(PropertyString.LockCode);
            set { if (value == null) RemoveProperty(PropertyString.LockCode); else SetProperty(PropertyString.LockCode, value); }
        }

        public override void ActOnUse(WorldObject worldObject)
        {
            var player = worldObject as Player;
            var behind = player != null && player.GetSplatterDir(this).Contains("Back");

            if (!IsLocked || behind)
            {
                if (!IsOpen)
                    Open(worldObject.Guid);
                else if (!(worldObject is Switch) && !(worldObject is PressurePlate))
                    Close(worldObject.Guid);

                // Create Door auto close timer
                ActionChain autoCloseTimer = new ActionChain();
                autoCloseTimer.AddDelaySeconds(ResetInterval ?? 0);
                autoCloseTimer.AddAction(this, () => Reset());
                autoCloseTimer.EnqueueChain();
            }
            else
            {
                if (player != null)
                {
                    var doorIsLocked = new GameEventCommunicationTransientString(player.Session, "The door is locked!");
                    player.Session.Network.EnqueueSend(doorIsLocked);
                    EnqueueBroadcast(new GameMessageSound(Guid, Sound.OpenFailDueToLock, 1.0f));
                }
            }
        }

        public void Open(ObjectGuid opener = new ObjectGuid())
        {
            if (CurrentMotionState == motionOpen)
                return;

            EnqueueBroadcastMotion(motionOpen);
            CurrentMotionState = motionOpen;
            Ethereal = true;
            IsOpen = true;
            //CurrentLandblock?.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(Sequences, Guid, PropertyBool.Ethereal, Ethereal ?? true));
            //CurrentLandblock?.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(Sequences, Guid, PropertyBool.Open, IsOpen ?? true));
            if (opener.Full > 0)
                UseTimestamp++;
        }

        private void Close(ObjectGuid closer = new ObjectGuid())
        {
            if (CurrentMotionState == motionClosed)
                return;

            EnqueueBroadcastMotion(motionClosed);
            CurrentMotionState = motionClosed;
            Ethereal = false;
            IsOpen = false;
            //CurrentLandblock?.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(Sequences, Guid, PropertyBool.Ethereal, Ethereal ?? false));
            //CurrentLandblock?.EnqueueBroadcast(Location, Landblock.MaxObjectRange, new GameMessagePublicUpdatePropertyBool(Sequences, Guid, PropertyBool.Open, IsOpen ?? false));
            if (closer.Full > 0)
                UseTimestamp++;
        }

        private void Reset()
        {
            if ((Time.GetUnixTime() - UseTimestamp) < ResetInterval)
                return;

            if (!DefaultOpen)
            {
                Close(ObjectGuid.Invalid);
                if (DefaultLocked)
                {
                    IsLocked = true;
                    var updateProperty = new GameMessagePublicUpdatePropertyBool(this, PropertyBool.Locked, IsLocked);
                    var sound = new GameMessageSound(Guid, Sound.OpenFailDueToLock, 1.0f); // TODO: This should probably come 1.5 seconds after the door closes so that sounds don't overlap
                    EnqueueBroadcast(updateProperty, sound);
                }
            }
            else
                Open(ObjectGuid.Invalid);

            ResetTimestamp++;
        }

        /// <summary>
        /// Used for unlocking a door via lockpick, so contains a skill check
        /// player.Skills[Skill.Lockpick].Current should be sent for the skill check
        /// </summary>
        public UnlockResults Unlock(uint unlockerGuid, uint playerLockpickSkillLvl, ref int difficulty)
        {
            return LockHelper.Unlock(this, playerLockpickSkillLvl, ref difficulty);
        }

        /// <summary>
        /// Used for unlocking a door via a key
        /// </summary>
        public UnlockResults Unlock(uint unlockerGuid, string keyCode)
        {
            return LockHelper.Unlock(this, keyCode);
        }

        public override void SetLinkProperties(WorldObject wo)
        {
            wo.ActivationTarget = Guid.Full;
        }

        public override void OnCollideObject(WorldObject target)
        {
            if (IsOpen) return;

            // currently the only AI options appear to be 0 or 1,
            // 1 meaning able to open doors?
            var creature = target as Creature;
            if (creature == null || creature.AiOptions == 0)
                return;

            ActOnUse(target);
        }
    }
}
