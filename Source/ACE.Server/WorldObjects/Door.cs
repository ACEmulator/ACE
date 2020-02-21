using System;
using log4net;
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
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            var player = worldObject as Player;
            var behind = player != null && player.GetRelativeDir(this).HasFlag(Quadrant.Back);

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

            IsOpen = true;

            if (opener.Full > 0)
                UseTimestamp++;

            var animTime = Physics.Animation.MotionTable.GetHookTime(MotionTableId, (uint)MotionStance.NonCombat, (uint)MotionCommand.Off, (uint)MotionCommand.On, AnimationHookType.Ethereal);

            if (animTime == -1)
            {
                log.Warn($"{Name}.Open() - couldn't find ethereal hook for wcid {WeenieClassId}, mtable {MotionTableId:X8}");
                animTime = 0.0f;
            }

            //Console.WriteLine($"{Name}.Open() - ethereal hook {animTime}");

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(animTime);
            actionChain.AddAction(this, () =>
            {
                Ethereal = true;

                EnqueueBroadcastPhysicsState();
            });
            actionChain.EnqueueChain();
        }

        public void Close(ObjectGuid closer = new ObjectGuid())
        {
            if (CurrentMotionState == motionClosed)
                return;

            EnqueueBroadcastMotion(motionClosed);
            CurrentMotionState = motionClosed;

            IsOpen = false;

            if (closer.Full > 0)
                UseTimestamp++;

            var animTime = Physics.Animation.MotionTable.GetHookTime(MotionTableId, (uint)MotionStance.NonCombat, (uint)MotionCommand.On, (uint)MotionCommand.Off, AnimationHookType.Ethereal);

            if (animTime == -1)
            {
                log.Warn($"{Name}.Close() - couldn't find ethereal hook for wcid {WeenieClassId}, mtable {MotionTableId:X8}");
                animTime = 0.0f;
            }

            //Console.WriteLine($"{Name}.Close() - ethereal hook {animTime}");

            var actionChain = new ActionChain();
            actionChain.AddDelaySeconds(animTime);
            actionChain.AddAction(this, () =>
            {
                Ethereal = false;

                EnqueueBroadcastPhysicsState();
            });
            actionChain.EnqueueChain();
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
                    EnqueueBroadcast(updateProperty);
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
