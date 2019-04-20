using System.Collections.Generic;

using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Network;

namespace ACE.Server.WorldObjects
{
    public class Admin : Sentinel
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Admin(Weenie weenie, ObjectGuid guid, uint accountId) : base(weenie, guid, accountId)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Admin(Biota biota, IEnumerable<Biota> inventory, IEnumerable<Biota> wieldedItems, Character character, Session session) : base(biota, inventory, wieldedItems, character, session)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            //BaseDescriptionFlags |= ObjectDescriptionFlag.Admin;

            if (!ChannelsAllowed.HasValue)
                ChannelsAllowed = Channel.QA1 | Channel.QA2 | Channel.ValidChans;
            else
                ChannelsAllowed |= Channel.QA1 | Channel.QA2 | Channel.ValidChans;

        }
    }
}
