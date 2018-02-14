using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Server.Managers;

namespace ACE.Server.Entity.WorldObjects
{
    public class Monster : Creature
    {
        /// <summary>
        /// If biota is null, one will be created with default values for this WorldObject type.
        /// </summary>
        public Monster(Weenie weenie, Biota biota = null) : base(weenie, biota)
        {
            if (biota == null) // If no biota was passed our base will instantiate one, and we will initialize it with appropriate default values
            {
                // TODO we shouldn't be auto setting properties that come from our weenie by default

                Guid = GuidManager.NewDynamicGuid();
                IsAlive = true;
                SetupVitals();
            }
        }
    }
}
