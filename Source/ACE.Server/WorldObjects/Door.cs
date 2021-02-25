using System;

using ACE.Common;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
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
            ObjectDescriptionFlags |= ObjectDescriptionFlag.Door;

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
                Ethereal = true;
            }

            ResetInterval = ResetInterval ?? 30.0f;
            LockCode = LockCode ?? "";

            // Account for possible missing property from recreated weenies
            if (IsLocked && !DefaultLocked)
                DefaultLocked = true;

            if (DefaultLocked)
                IsLocked = true;
            else
                IsLocked = false;

            ActivationResponse |= ActivationResponse.Use;
        }

        public string LockCode
        {
            get => GetProperty(PropertyString.LockCode);
            set { if (value == null) RemoveProperty(PropertyString.LockCode); else SetProperty(PropertyString.LockCode, value); }
        }

        public override void OnTalk(WorldObject activator)
        {
            if (activator is Player player)
            {
                var behind = player != null && player.GetRelativeDir(this).HasFlag(Quadrant.Back);

                if (IsOpen && !behind) // not sure if retail made this distinction, but for the doors tested, it seemed more logical given the text shown
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat(ActivationTalk, ChatMessageType.Broadcast));
            }
        }

        public override void ActOnUse(WorldObject worldObject)
        {
            if (IsBusy) return;

            var player = worldObject as Player;
            var behind = player != null && player.GetRelativeDir(this).HasFlag(Quadrant.Back);

            if (!IsLocked || behind)
            {
                if (!IsOpen)
                    Open(worldObject.Guid);
                else if (!(worldObject is Switch) && !(worldObject is PressurePlate))
                    Close(worldObject.Guid);

                // Create Door auto close timer
                var useTimestamp = UseTimestamp ?? 0;

                var autoCloseTimer = new ActionChain();
                autoCloseTimer.AddDelaySeconds(ResetInterval ?? 0);
                autoCloseTimer.AddAction(this, () => Reset(useTimestamp));
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

            EnqueueBroadcastPhysicsState();

            if (opener.Full > 0)
                UseTimestamp = Time.GetUnixTime();

            IsBusy = true;

            var animTime = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, MotionStance.NonCombat, MotionCommand.Off, MotionCommand.On);

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(animTime);
            actionChain.AddAction(this, () => IsBusy = false);
            actionChain.EnqueueChain();
        }

        public double CloseTimestamp;

        public void Close(ObjectGuid closer = new ObjectGuid())
        {
            if (CurrentMotionState == motionClosed)
                return;

            EnqueueBroadcastMotion(motionClosed);
            CurrentMotionState = motionClosed;

            IsOpen = false;

            var animTime = Physics.Animation.MotionTable.GetAnimationLength(MotionTableId, MotionStance.NonCombat, MotionCommand.On, MotionCommand.Off);

            //Console.WriteLine($"AnimTime: {animTime}");

            IsBusy = true;

            CloseTimestamp = Time.GetUnixTime();

            var closeTimestamp = CloseTimestamp;

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(animTime);
            actionChain.AddAction(this, () =>
            {
                FinalizeClose(closeTimestamp);
                IsBusy = false;
            });
            actionChain.EnqueueChain();

            if (closer.Full > 0)
                UseTimestamp = Time.GetUnixTime();
        }

        private void FinalizeClose(double closeTimestamp)
        {
            if (IsOpen || closeTimestamp != CloseTimestamp)
                return;

            // ethereal must be set to false for ethereal_check_for_collisions
            Ethereal = false;

            if (PropertyManager.GetBool("allow_door_hold").Item && PhysicsObj.ethereal_check_for_collisions())
            {
                // the source of this bug is EtherealHook for the door
                // physics engine set_ethereal() -> ethereal_check_for_collisions() -> CheckEthereal state

                // if fix_door_holding == true, the player can still hold doors for other nearby players
                // who already know about the door / have not been far away from the door for > 25s

                // fix_door_holding == true only fixes 'long holding'
                //Console.WriteLine($"{Name} ({Guid}).FinalizeClose()");
                Ethereal = true;

                var holdChain = new ActionChain();
                holdChain.AddDelaySeconds(1.0f);    // poll every second
                holdChain.AddAction(this, () => FinalizeClose(closeTimestamp));
                holdChain.EnqueueChain();
                return;
            }

            EnqueueBroadcastPhysicsState();
        }

        private void Reset(double useTimestamp)
        {
            if (useTimestamp != UseTimestamp) return;

            if (!DefaultOpen)
            {
                Close(ObjectGuid.Invalid);
                if (DefaultLocked)
                {
                    IsLocked = true;
                    var updateProperty = new GameMessagePublicUpdatePropertyBool(this, PropertyBool.Locked, IsLocked);
                    EnqueueBroadcast(updateProperty);
                }
            }
            else
                Open(ObjectGuid.Invalid);

            ResetTimestamp = Time.GetUnixTime();
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
        public UnlockResults Unlock(uint unlockerGuid, Key key, string keyCode = null)
        {
            return LockHelper.Unlock(this, key, keyCode);
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
