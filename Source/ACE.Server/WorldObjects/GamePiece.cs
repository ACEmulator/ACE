using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network.Motion;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;

namespace ACE.Server.WorldObjects
{
    public class GamePiece : Creature
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public GamePiece(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public GamePiece(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        public void Kill()
        {
            ActionChain killChain = new ActionChain();
            killChain.AddAction(this, () =>
            {
                HandleActionMotion(new UniversalMotion(MotionStance.NonCombat, new MotionItem(MotionCommand.Dead)));
            });
            killChain.AddDelaySeconds(5);
            killChain.AddAction(this, () =>
            {
                ApplyVisualEffects(global::ACE.Entity.Enum.PlayScript.Destroy);
            });
            killChain.AddDelaySeconds(1);
            killChain.AddAction(this, () =>
            {
               LandblockManager.RemoveObject(this);
            });
            killChain.EnqueueChain();
        }
    }
}
