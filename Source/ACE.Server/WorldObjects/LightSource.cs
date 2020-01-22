
using ACE.Entity;

namespace ACE.Server.WorldObjects
{
    public class LightSource : GenericObject
    {
        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public LightSource(Database.Models.World.Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public LightSource(Database.Models.Shard.Biota biota) : base(biota)
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
