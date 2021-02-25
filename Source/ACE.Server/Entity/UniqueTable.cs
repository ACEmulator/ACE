using System.Collections.Generic;

using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class UniqueTable
    {
        // helper counting the # of uniques in a container,
        // and possibly its sub-containers
        public Dictionary<uint, UniqueTableEntry> Entries;

        public UniqueTable(List<WorldObject> uniques)
        {
            Entries = new Dictionary<uint, UniqueTableEntry>();

            foreach (var unique in uniques)
            {
                var wcid = unique.WeenieClassId;
                var stackSize = unique.StackSize ?? 1;

                if (!Entries.TryGetValue(wcid, out var entry))
                {
                    entry = new UniqueTableEntry(stackSize, unique.Unique ?? 0);
                    Entries.Add(wcid, entry);
                }
                else
                    entry.Count += stackSize;
            }
        }

        public class UniqueTableEntry
        {
            public int Count;
            public int Max;

            public UniqueTableEntry(int count, int max)
            {
                Count = count;
                Max = max;
            }
        }
    }
}
