using System;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;

namespace ACE.Server.WorldObjects
{
    public class Lockpick : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Lockpick(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Lockpick(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            ObjectDescriptionFlags |= ObjectDescriptionFlag.Lockpick;
        }

        public override void HandleActionUseOnTarget(Player player, WorldObject target)
        {
            UnlockerHelper.UseUnlocker(player, this, target);
        }
    }
}
