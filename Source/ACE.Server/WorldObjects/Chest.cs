using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
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
        /// This is the default setup for resetting chests
        /// </summary>
        public double ChestResetInterval
        {
            get
            {
                var chestResetInterval = ResetInterval ?? Default_ChestResetInterval;

                if (chestResetInterval == 0)
                    chestResetInterval = Default_ChestResetInterval;

                return chestResetInterval;
            }
        }

        public double Default_ChestResetInterval = 120;

        /// <summary>
        /// The current player who has a chest opened
        /// </summary>
        public Player CurrentViewer;

        public bool ResetMessagePending
        {
            get => GetProperty(PropertyBool.ResetMessagePending) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.ResetMessagePending); else SetProperty(PropertyBool.ResetMessagePending, value); }
        }

        public bool ResetGenerator;

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

            CurrentMotionState = motionClosed;  // do any chests default to open?

            if (IsLocked)
                DefaultLocked = true;

            ResetGenerator = true;
        }

        protected static readonly Motion motionOpen = new Motion(MotionStance.NonCombat, MotionCommand.On);
        protected static readonly Motion motionClosed = new Motion(MotionStance.NonCombat, MotionCommand.Off);

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

            if (IsLocked)
            {
                if (player.LastUsedContainerId == Guid)
                    player.LastUsedContainerId = ObjectGuid.Invalid;

                EnqueueBroadcast(new GameMessageSound(Guid, Sound.OpenFailDueToLock, 1.0f));
                return;
            }

            if (IsOpen)
            {
                // player has this chest open, close it
                if (Viewer == player.Guid.Full)
                    Close(player);

                // else another player has this chest open - send error message?
                return;
            }

            // open chest
            Open(player);
        }

        public override void Open(Player player)
        {
            CurrentViewer = player;
            base.Open(player);

            // chests can have a couple of different profiles
            // by default, most chests use the 'ResetInterval' setup
            // some things like Mana Forge chests use the 'RegenOnClose' variant

            // ResetInterval (default):

            // if no ResetInterval is defined, the DefaultResetInterval of 2 mins is used.
            // when a player opens this chest, a timer starts, and the chest will automatically close/reset in ResetInterval

            // RegenOnClose (Mana Forge Chest etc.):

            // this chest resets whenever it is closed

            if (!ChestRegenOnClose && !ResetMessagePending)
            {
                //Console.WriteLine($"{player.Name}.Open({Name}) - enqueueing reset in {ChestResetInterval}s");

                // uses the ResetInterval setup
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(ChestResetInterval);
                actionChain.AddAction(this, Reset);
                actionChain.EnqueueChain();

                ResetMessagePending = true;

                //UseTimestamp++;
            }

            if (ActivationTalk != null)
            {
                // send only to activator?
                player.Session.Network.EnqueueSend(new GameMessageSystemChat(ActivationTalk, ChatMessageType.Broadcast));
            }

            if (SpellDID.HasValue)
            {
                var spell = new Server.Entity.Spell((uint)SpellDID);

                TryCastSpell(spell, player, this);
            }
        }

        /// <summary>
        /// Called when a chest is closed, or walked away from
        /// </summary>
        public void Close(Player player, bool tryReset = true)
        {
            base.Close(player);
            CurrentViewer = null;

            if (ChestRegenOnClose && tryReset)
                Reset();
        }

        public override void Reset()
        {
            // TODO: if 'ResetInterval' style, do we want to ensure a minimum amount of time for the last viewer?

            if (IsOpen)
                Close(CurrentViewer, false);

            if (DefaultLocked && !IsLocked)
            {
                IsLocked = true;
                EnqueueBroadcast(new GameMessagePublicUpdatePropertyBool(this, PropertyBool.Locked, IsLocked));
            }

            if (IsGenerator)
            {
                ResetGenerator = true;
                Generator_HeartBeat();
            }

            ResetMessagePending = false;
        }

        protected override float DoOnOpenMotionChanges()
        {
            return ExecuteMotion(motionOpen);
        }

        protected override float DoOnCloseMotionChanges()
        {
            return ExecuteMotion(motionClosed);
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
                LastUnlocker = unlockerGuid;

            return result;
        }

        /// <summary>
        /// Used for unlocking a chest via a key
        /// </summary>
        public UnlockResults Unlock(uint unlockerGuid, string keyCode)
        {
            var result = LockHelper.Unlock(this, keyCode);

            if (result == UnlockResults.UnlockSuccess)
                LastUnlocker = unlockerGuid;

            return result;
        }
    }
}
