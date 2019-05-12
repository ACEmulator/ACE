using System;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.WorldObjects
{
    public class Hooker : WorldObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Hooker(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Hooker(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            ActivationResponse |= ActivationResponse.Emote;
        }

        public override void ActOnUse(WorldObject activator)
        {
            // handled in base.OnActivate -> EmoteManager.OnUse()
        }
    }
}
