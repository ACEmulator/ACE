using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.Motion;
using ACE.Server.Network.GameMessages.Messages;

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

            if (Guid == guidTarget)
            {
                CreatePlayerSpell(guidTarget, spellId);
            }
            else
            {
                // get angle to target
                var angle = GetAngle(target);

                float delay = 0;
                if (angle < 10.0f)
                {
                    CreatePlayerSpell(guidTarget, spellId);
                    return;
                }

                if (angle < 120.0f) delay = 1.0f;
                else delay = 1.5f;

                var turnToMotion = new UniversalMotion(MotionStance.Spellcasting, Location, guidTarget);
                turnToMotion.MovementTypes = MovementTypes.TurnToObject;

                ActionChain turnToTimer = new ActionChain();
                turnToTimer.AddAction(this, () => CurrentLandblock.EnqueueBroadcastMotion(this, turnToMotion));
                turnToTimer.AddDelaySeconds(delay);
                turnToTimer.AddAction(this, () => CreatePlayerSpell(guidTarget, spellId));
                turnToTimer.EnqueueChain();
            }
        }

        /// <summary>
        /// Handles player untargeted casting message
        /// </summary>
        public void HandleActionMagicCastUnTargetedSpell(uint spellId)
        {
            CreatePlayerSpell(spellId);
        }
    }
}
