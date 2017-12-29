using System;
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

        public async Task Kill()
        {
            HandleActionMotion(MotionDeath);

            await Task.Delay(TimeSpan.FromSeconds(5));

            ApplyVisualEffects(Enum.PlayScript.Destroy);

            await Task.Delay(TimeSpan.FromSeconds(1));

           LandblockManager.RemoveObject(this);
        }
    }
}
