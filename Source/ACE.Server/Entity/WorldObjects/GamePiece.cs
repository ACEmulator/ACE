using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;

namespace ACE.Server.Entity.WorldObjects
{
    public class GamePiece : Creature
    {
        /// <summary>
        /// If biota is null, one will be created with default values for this WorldObject type.
        /// </summary>
        public GamePiece(Weenie weenie, Biota biota = null) : base(weenie, biota)
        {
            DescriptionFlags |= ObjectDescriptionFlag.Stuck | ObjectDescriptionFlag.Attackable;

            SetProperty(PropertyBool.Stuck, true);
            SetProperty(PropertyBool.Attackable, true);
        }

        public void Kill()
        {
            ActionChain killChain = new ActionChain();
            killChain.AddAction(this, () =>
            {
                HandleActionMotion(MotionDeath);
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
