using System;
using System.IO;

using ACE.Entity;

namespace ACE.Server.WorldObjects
{
    public class Ammunition : Stackable
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Ammunition(ACE.Entity.Models.Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Ammunition(Database.Models.Shard.Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Ammunition(ACE.Entity.Models.Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }

        public override void OnCollideObject(WorldObject target)
        {
            var proj = new Projectile(this);
            proj.OnCollideObject(target);
        }

        public override void OnCollideEnvironment()
        {
            var proj = new Projectile(this);
            proj.OnCollideEnvironment();
        }

        public override void ActOnUse(WorldObject wo)
        {
            // Do nothing
        }
    }
}
