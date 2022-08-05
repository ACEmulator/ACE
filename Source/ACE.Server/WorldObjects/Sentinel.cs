using System.Collections.Generic;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Models;
using ACE.Server.Network;

using Character = ACE.Database.Models.Shard.Character;

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
        public Sentinel(Biota biota, IEnumerable<ACE.Database.Models.Shard.Biota> inventory, IEnumerable<ACE.Database.Models.Shard.Biota> wieldedItems, Character character, Session session) : base(biota, inventory, wieldedItems, character, session)
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
            ObjectDescriptionFlags |= ObjectDescriptionFlag.Admin;

            if (!ChannelsAllowed.HasValue)
                ChannelsAllowed = Channel.Audit | Channel.Advocate1 | Channel.Advocate2 | Channel.Advocate3 | Channel.Sentinel | Channel.AllBroadcast;
            else
                ChannelsAllowed |= Channel.Audit | Channel.Advocate1 | Channel.Advocate2 | Channel.Advocate3 | Channel.Sentinel | Channel.AllBroadcast;
        }

        public override void InitPhysicsObj()
        {
            base.InitPhysicsObj();

            switch (CloakStatus)
            {
                case CloakStatus.Off:
                    goto default;
                case CloakStatus.On:
                    //Translucency = 0.5f;
                    Cloaked = true;
                    Ethereal = true;
                    NoDraw = true;
                    Visibility = true;
                    break;
                case CloakStatus.Player:
                    goto default;
                case CloakStatus.Creature:
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
