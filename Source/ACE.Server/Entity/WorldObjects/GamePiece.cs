using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
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
            if (biota == null) // If no biota was passed our base will instantiate one, and we will initialize it with appropriate default values
            {
                // TODO we shouldn't be auto setting properties that come from our weenie by default

                Stuck = true;
                Attackable = true;

                SetObjectDescriptionBools();
            }
        }

        ////public override void ActOnUse(ObjectGuid playerId)
        ////{

        ////}

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
