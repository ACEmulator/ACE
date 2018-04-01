using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        /// <summary>
        /// Handles player targeted casting message
        /// </summary>
        public void HandleActionCastTargetedSpell(ObjectGuid guidTarget, uint spellId)
        {
            WorldObject target = CurrentLandblock.GetObject(guidTarget) as WorldObject;

            var turnToMotion = new UniversalMotion(MotionStance.Spellcasting, Location, guidTarget);
            turnToMotion.MovementTypes = MovementTypes.TurnToObject;

            ActionChain turnToTimer = new ActionChain();
            turnToTimer.AddAction(this, () => CurrentLandblock.EnqueueBroadcastMotion(this, turnToMotion));
            turnToTimer.AddDelaySeconds(1);
            turnToTimer.AddAction(this, () => CreatePlayerSpell(guidTarget, spellId));
            turnToTimer.EnqueueChain();
        }

        /// <summary>
        /// Handles player untargeted casting message
        /// </summary>
        public void HandleActionCastUntargetedSpell(uint spellId)
        {
            CreatePlayerSpell(spellId);
        }
    }
}
