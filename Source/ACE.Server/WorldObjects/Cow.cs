using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;

namespace ACE.Server.WorldObjects
{
    public class Cow : Creature
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Cow(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Cow(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// </summary>
        public override void ActOnUse(WorldObject activator)
        {
            if (!(activator is Player player) || AllowedActivator != null)
                return;

            AllowedActivator = activator.Guid.Full;

            // Stamp Cow tipping quest here;
            EmoteManager.IsBusy = true;

            //var dir = player.GetSplatterDir(this);
            //var motion = dir.Contains("Left") ? MotionCommand.TippedRight : MotionCommand.TippedLeft;

            var motion = MotionCommand.TippedRight;     // left bugged?

            var actionChain = new ActionChain();
            EnqueueMotion(actionChain, motion);
            actionChain.AddAction(this, () => ResetCow());
            actionChain.EnqueueChain();

            UseTimestamp++;
        }

        private void ResetCow()
        {
            CurrentMotionState = new Motion(MotionStance.NonCombat, MotionCommand.Ready);

            EmoteManager.IsBusy = false;

            AllowedActivator = null;

            ResetTimestamp++;
        }
    }
}
