using System;

using ACE.Entity;
using ACE.Entity.Models;

using Biota = ACE.Database.Models.Shard.Biota;

namespace ACE.Server.WorldObjects
{
    public class MissileLauncher : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public MissileLauncher(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public MissileLauncher(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }
    }
}
