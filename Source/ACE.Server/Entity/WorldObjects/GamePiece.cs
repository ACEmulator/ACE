using ACE.Database.Models.World;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;

namespace ACE.Server.Entity.WorldObjects
{
    public class GamePiece : Creature
    {
        public GamePiece(Weenie weenie) : base(weenie, null)
        {
            Stuck = true;
            Attackable = true;
            
            SetObjectDescriptionBools();
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
