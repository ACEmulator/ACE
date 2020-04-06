using System;

using ACE.Entity;
using ACE.Entity.Models;

namespace ACE.Server.WorldObjects
{
    public class MeleeWeapon : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public MeleeWeapon(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public MeleeWeapon(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            CurrentMotionState = null;
        }
    }
}
