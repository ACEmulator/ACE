using System;
using System.Collections.Generic;

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
    public partial class Chest : Container, Lock
    {
        /// <summary>
        /// This is used for things like Mana Forge Chests
        /// </summary>
        public bool ChestRegenOnClose
        {
            get
            {
                if (ChestResetInterval <= 5)
                    return true;

                return GetProperty(PropertyBool.ChestRegenOnClose) ?? false;
            }
            set { if (!value) RemoveProperty(PropertyBool.ChestRegenOnClose); else SetProperty(PropertyBool.ChestRegenOnClose, value); }
        }

        /// <summary>
        /// This is used for things like Dirty Old Crate
        /// </summary>
        public bool ChestClearedWhenClosed
        {
            get => GetProperty(PropertyBool.ChestClearedWhenClosed) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.ChestClearedWhenClosed); else SetProperty(PropertyBool.ChestClearedWhenClosed, value); }
        }

        /// <summary>
        /// This is the default setup for resetting chests
        /// </summary>
        public double ChestResetInterval
        {
            get
            {
                var chestResetInterval = ResetInterval ?? Default_ChestResetInterval;

                if (chestResetInterval < 15)
                    chestResetInterval = Default_ChestResetInterval;

                return chestResetInterval;
            }
        }

        public virtual double Default_ChestResetInterval => 120;

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
            ContainerCapacity = ContainerCapacity ?? 10;
            ItemCapacity = ItemCapacity ?? 120;

            ActivationResponse |= ActivationResponse.Use;   // todo: fix broken data

            CurrentMotionState = motionClosed;              // do any chests default to open?

            if (IsLocked)
                DefaultLocked = true;

            if (DefaultLocked) // ignore regen interval, only regen on relock
                NextGeneratorRegenerationTime = double.MaxValue;
        }

        protected static readonly Motion motionOpen = new Motion(MotionStance.NonCombat, MotionCommand.On);
        protected static readonly Motion motionClosed = new Motion(MotionStance.NonCombat, MotionCommand.Off);

        public override ActivationResult CheckUseRequirements(WorldObject activator)
        {
            var baseRequirements = base.CheckUseRequirements(activator);
            if (!baseRequirements.Success)
                return baseRequirements;

            if (!(activator is Player player))
                return new ActivationResult(false);

            if (IsLocked)
            {
                EnqueueBroadcast(new GameMessageSound(Guid, Sound.OpenFailDueToLock, 1.0f));
                return new ActivationResult(false);
            }

            if (UseLockTimestamp != null && activator.Guid.Full != LastUnlocker)
            {
                var currentTime = Time.GetUnixTime();

                // prevent ninja looting
                if (UseLockTimestamp.Value + PropertyManager.GetDouble("unlocker_window").Item > currentTime)
                {
                    player.SendTransientError(InUseMessage);
                    return new ActivationResult(false);
                }
            }

            if (IsOpen)
            {
                if (Viewer == player.Guid.Full)
                {
                    // current player has this chest open, close it
                    Close(player);
                }
                else
                {
                    // another player has this chest open -- ensure they are within range
                    var currentViewer = CurrentLandblock.GetObject(Viewer) as Player;

                    if (currentViewer == null)
                        Close(null);    // current viewer not found, close it
                    else
                        player.SendTransientError(InUseMessage);
                }

                return new ActivationResult(false);
            }

            // handle quest requirements
            if (Quest != null)
            {
                if (!player.QuestManager.HasQuest(Quest))
                    EmoteManager.OnQuest(player);
                else
                {
                    if (player.QuestManager.CanSolve(Quest))
                    {
                        EmoteManager.OnQuest(player);
                    }
                    else
                    {
                        player.QuestManager.HandleSolveError(Quest);
                        return new ActivationResult(false);
                    }
                }
            }

            return new ActivationResult(true);
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(WorldObject wo)
        {
            if (!(wo is Player player))
                return;

            // open chest
            Open(player);
        }

        public override void Open(Player player)
        {
            base.Open(player);

            if (!ResetMessagePending && !double.IsPositiveInfinity(ChestResetInterval))
            {
                var resetTimestamp = ResetTimestamp;

                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(ChestResetInterval);
                actionChain.AddAction(this, () => Reset(resetTimestamp));
                actionChain.EnqueueChain();

                ResetMessagePending = true;
            }

            UseLockTimestamp = null;
        }

        public override void Close(Player player)
        {
            Close(player);
        }

        /// <summary>
        /// Called when a chest is closed, or walked away from
        /// </summary>
        public void Close(Player player, bool tryReset = true)
        {
            base.Close(player);

            if (ChestRegenOnClose && tryReset)
                Reset(ResetTimestamp);
        }

        public override void FinishClose(Player player)
        {
            base.FinishClose(player);

            if (ChestClearedWhenClosed && InitCreate > 0)
            {
                if (CurrentCreate == 0)
                    FadeOutAndDestroy(); // Chest's complete generated inventory count has been wiped out
                    //Destroy(); // Chest's complete generated inventory count has been wiped out
            }
        }

        public void Reset(double? resetTimestamp)
        {
            if (resetTimestamp != ResetTimestamp)
                return;     // already cleared by previous reset

            // TODO: if 'ResetInterval' style, do we want to ensure a minimum amount of time for the last viewer?

            // should only be an edge case with reload-landblock
            if (CurrentLandblock == null)
                return;

            var player = CurrentLandblock.GetObject(Viewer) as Player;

            if (IsOpen)
                Close(player, false);

            if (DefaultLocked && !IsLocked)
            {
                IsLocked = true;
                EnqueueBroadcast(new GameMessagePublicUpdatePropertyBool(this, PropertyBool.Locked, IsLocked));
            }

            if (IsGenerator)
            {
                ResetGenerator();
                CurrentlyPoweringUp = true;
                if (InitCreate > 0)
                    Generator_Generate();
            }

            ResetTimestamp = Time.GetUnixTime();
            ResetMessagePending = false;
        }
        protected override float DoOnOpenMotionChanges()
        {
            if (MotionTableId != 0)
                return ExecuteMotion(motionOpen);
            else
                return 0;
        }

        protected override float DoOnCloseMotionChanges()
        {
            if (MotionTableId != 0)
                return ExecuteMotion(motionClosed);
            else
                return 0;
        }

        public string LockCode
        {
            get => GetProperty(PropertyString.LockCode);
            set { if (value == null) RemoveProperty(PropertyString.LockCode); else SetProperty(PropertyString.LockCode, value); }
        }

        /// <summary>
        /// Used for unlocking a chest via lockpick, so contains a skill check
        /// player.Skills[Skill.Lockpick].Current should be sent for the skill check
        /// </summary>
        public UnlockResults Unlock(uint unlockerGuid, uint playerLockpickSkillLvl, ref int difficulty)
        {
            var result = LockHelper.Unlock(this, playerLockpickSkillLvl, ref difficulty);

            if (result == UnlockResults.UnlockSuccess)
            {
                LastUnlocker = unlockerGuid;
                UseLockTimestamp = Time.GetUnixTime();
            }
            return result;
        }

        /// <summary>
        /// Used for unlocking a chest via a key
        /// </summary>
        public UnlockResults Unlock(uint unlockerGuid, Key key, string keyCode = null)
        {
            var result = LockHelper.Unlock(this, key, keyCode);

            if (result == UnlockResults.UnlockSuccess)
            {
                LastUnlocker = unlockerGuid;
                UseLockTimestamp = Time.GetUnixTime();
            }
            return result;
        }
    }
}
