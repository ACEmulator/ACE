using System.Collections.Generic;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network;

namespace ACE.Server.WorldObjects
{
    public class Sentinel : Player
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Sentinel(Weenie weenie, ObjectGuid guid, uint accountId) : base(weenie, guid, accountId)
        {
            if (!Character.IsPlussed)
            {
                Character.IsPlussed = true;
                CharacterChangesDetected = true;
            }

            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Sentinel(Biota biota, IEnumerable<Biota> inventory, IEnumerable<Biota> wieldedItems, Character character, Session session) : base(biota, inventory, wieldedItems, character, session)
        {
            if (!Character.IsPlussed)
            {
                Character.IsPlussed = true;
                CharacterChangesDetected = true;
            }

            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            BaseDescriptionFlags |= ObjectDescriptionFlag.Admin;

            switch (CloakStatus)
            {
                case ACE.Entity.Enum.CloakStatus.Off:
                    goto default;
                case ACE.Entity.Enum.CloakStatus.On:
                    //Translucency = 0.5f;
                    Cloaked = true;
                    Ethereal = true;
                    NoDraw = true;
                    Visibility = true;
                    break;
                case ACE.Entity.Enum.CloakStatus.Player:
                    goto default;
                case ACE.Entity.Enum.CloakStatus.Creature:
                    Attackable = true;
                    goto default;
                default:
                    Translucency = null;
                    Cloaked = false;
                    Ethereal = false;
                    NoDraw = false;
                    Visibility = false;
                    break;
            }
        }
    }
}
