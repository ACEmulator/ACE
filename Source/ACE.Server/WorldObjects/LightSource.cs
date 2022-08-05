using System;

using ACE.Entity;
using ACE.Entity.Models;

namespace ACE.Server.WorldObjects
{
    public class LightSource : GenericObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public LightSource(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public LightSource(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {        
        }

        public override void ActOnUse(WorldObject wo)
        {
            // Do nothing
        }
    }
}
