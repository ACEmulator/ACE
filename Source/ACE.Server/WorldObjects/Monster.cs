using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;

namespace ACE.Server.WorldObjects
{
    public class Monster : Creature
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Monster(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            IsAlive = true;
            //SetupVitals();

            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Monster(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
        }
    }
}
