using ACE.Database.Models.Shard;
using ACE.Database.Models.World;

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
                IsAlive = true;
                SetupVitals();
            }
        }
    }
}
