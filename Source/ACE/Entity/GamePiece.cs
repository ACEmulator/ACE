using ACE.Network.GameEvent.Events;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Motion;
using ACE.Entity.Enum;
using ACE.Entity.Actions;
using ACE.Network.Enum;
using ACE.DatLoader.FileTypes;
using ACE.Managers;

namespace ACE.Entity
{
    public class GamePiece : Creature
    {
        public GamePiece(AceObject aceO)
            : base(aceO)
        {
            Stuck = true; Attackable = true;

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
