using System.Threading.Tasks;

using ACE.Entity.Actions;
using ACE.Managers;

namespace ACE.Entity
{
    public class GamePiece : Creature
    {
        public GamePiece()
        {
        }

        protected override async Task Init(AceObject aceO)
        {
            await base.Init(aceO);
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
                ApplyVisualEffects(Enum.PlayScript.Destroy);
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
