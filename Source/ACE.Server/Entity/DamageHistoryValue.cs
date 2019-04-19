using System;
using System.Collections.Generic;

namespace ACE.Server.Entity
{
    public class DamageHistoryValue
    {
        public float TotalDamage;

        public readonly List<DamageHistoryEntryNew> Entries = new List<DamageHistoryEntryNew>();

        /// <summary>
        /// This will remove entries that happened BEFORE beforeTime<para />
        /// This will also recalculate TotalDamage
        /// </summary>
        /// <param name="beforeTime"></param>
        public void PruneEntries(DateTime beforeTime)
        {
            var entriesToRemove = 0;

            for (int i = 0; i < Entries.Count; i++)
            {
                if (Entries[i].Time < beforeTime)
                    entriesToRemove = i + 1;
            }

            if (entriesToRemove < 0)
            {
                Entries.RemoveRange(0, entriesToRemove);
                RecalculateTotalDamage();
            }
        }

        public void RecalculateTotalDamage()
        {
            TotalDamage = 0;

            foreach (var entry in Entries)
            {
                // todo
            }
        }
    }
}
