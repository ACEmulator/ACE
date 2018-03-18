using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;

namespace ACE.Server.WorldObjects
{
    public class AdvocateItem : GenericObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public AdvocateItem(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public AdvocateItem(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }
    }
}
