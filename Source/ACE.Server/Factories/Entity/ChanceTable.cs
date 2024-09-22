using System;
using System.Collections.Generic;
using System.Linq;
using log4net;

using ACE.Common;

namespace ACE.Server.Factories.Entity
{
    public class ChanceTable<T> : List<(T result, float chance)>
    {
        private bool verified;
        private const decimal threshold = 0.0000001M;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private void VerifyTable()
        {
            var total = 0.0M;

            foreach (var entry in this)
                total += (decimal)entry.chance;

            if (Math.Abs(1.0M - total) > threshold)
                log.Error($"Chance table adds up to {total}, expected 1.0: {string.Join(", ", this)}");

            verified = true;
        }

        public T Roll(float qualityMod = 0.0f)
        {
            if (!verified)
                VerifyTable();

            var total = 0.0f;

            //var rng = ThreadSafeRandom.NextIntervalMax(qualityMod);
            var rng = ThreadSafeRandom.Next(0.0f, 1.0f);

            foreach (var entry in this)
            {
                total += entry.chance;

                if (rng < total && total >= qualityMod)
                    return entry.result;
            }

            //Console.WriteLine($"Rolled {rng}, everything >= {total}");

            return this.Last(i => i.chance > 0).result;
        }
    }
}
