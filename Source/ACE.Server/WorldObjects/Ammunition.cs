using System;

using ACE.Entity;
using ACE.Entity.Models;

namespace ACE.Server.WorldObjects
{
    public class Ammunition : Stackable
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Ammunition(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Ammunition(Biota biota) : base(biota)
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
