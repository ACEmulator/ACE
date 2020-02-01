
using ACE.Entity;

namespace ACE.Server.WorldObjects
{
    public class MissileLauncher : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public MissileLauncher(ACE.Entity.Models.Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public MissileLauncher(Database.Models.Shard.Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public MissileLauncher(ACE.Entity.Models.Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }
    }
}
