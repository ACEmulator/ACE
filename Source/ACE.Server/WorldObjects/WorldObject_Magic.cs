using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        public enum CastResult
        {
            SpellTargetInvalid,
            SpellNotImplemented
        }
        /// <summary>
        /// Method used for handling the spell cast
        /// </summary>
        public CastResult CreateSpell(ObjectGuid guidCaster, ObjectGuid? guidTarget, uint spellId)
        {
            if (guidTarget != null)
            {
                WeenieType? spellTarget = CurrentLandblock.GetObject((ObjectGuid)guidTarget).WeenieType;

                if (spellTarget == null)
                    return CastResult.SpellTargetInvalid;
            }

            return CastResult.SpellNotImplemented;
        }

    }
}
