using System.Collections.Generic;

using ACE.Database.Models.Shard;

namespace ACE.Database.Entity
{
    public class PlayerBiotas
    {
        public Biota Player { get; }

        public List<Biota> Inventory { get; } = new List<Biota>();

        public List<Biota> WieldedItems { get; } = new List<Biota>();

        public PlayerBiotas(Biota player, ICollection<Biota> inventory, ICollection<Biota> wieldedItems)
        {
            Player = player;

            Inventory.AddRange(inventory);

            WieldedItems.AddRange(wieldedItems);
        }
    }
}
