
using ACE.Entity;

namespace ACE.Server.WorldObjects
{
    public class MeleeWeapon : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public MeleeWeapon(ACE.Entity.Models.Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public MeleeWeapon(Database.Models.Shard.Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public MeleeWeapon(ACE.Entity.Models.Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            CurrentMotionState = null;
        }
    }
}
