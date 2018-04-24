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
                // turn if required
                var rotateTime = Rotate(target);
                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(rotateTime);

                actionChain.AddAction(this, () => CreatePlayerSpell(guidTarget, spellId));
                actionChain.EnqueueChain();
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
