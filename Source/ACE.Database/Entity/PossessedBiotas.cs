using System.Collections.Generic;

using ACE.Database.Models.Shard;

namespace ACE.Database.Entity
{
    public class PossessedBiotas
    {
        public List<Biota> Inventory { get; } = new List<Biota>();

        public List<Biota> WieldedItems { get; } = new List<Biota>();

        public PossessedBiotas(ICollection<Biota> inventory, ICollection<Biota> wieldedItems)
        {
            Inventory.AddRange(inventory);

            WieldedItems.AddRange(wieldedItems);
        }
    }
}
