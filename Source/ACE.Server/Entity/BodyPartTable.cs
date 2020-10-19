using System.Collections.Generic;
using System.Linq;

using log4net;

using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Models;

namespace ACE.Server.Entity
{
    public class BodyPartTable
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Weenie Weenie;

        public readonly List<BodyPartProbability>[] Quadrants = new List<BodyPartProbability>[12];

        public BodyPartTable(Weenie weenie)
        {
            Weenie = weenie;

            for (var i = 0; i < Quadrants.Length; i++)
                Quadrants[i] = new List<BodyPartProbability>();

            if (Weenie.PropertiesBodyPart == null)
            {
                log.Error($"BodyPartTable is null for {weenie.WeenieClassId} - {weenie.ClassName}!");
                return;
            }

            foreach (var kvp in Weenie.PropertiesBodyPart)
            {
                var bodyPart = kvp.Value;

                if (bodyPart.HLF > 0.0f) Quadrants[0].Add(new BodyPartProbability(kvp.Key, bodyPart.HLF));
                if (bodyPart.MLF > 0.0f) Quadrants[1].Add(new BodyPartProbability(kvp.Key, bodyPart.MLF));
                if (bodyPart.LLF > 0.0f) Quadrants[2].Add(new BodyPartProbability(kvp.Key, bodyPart.LLF));

                if (bodyPart.HRF > 0.0f) Quadrants[3].Add(new BodyPartProbability(kvp.Key, bodyPart.HRF));
                if (bodyPart.MRF > 0.0f) Quadrants[4].Add(new BodyPartProbability(kvp.Key, bodyPart.MRF));
                if (bodyPart.LRF > 0.0f) Quadrants[5].Add(new BodyPartProbability(kvp.Key, bodyPart.LRF));

                if (bodyPart.HLB > 0.0f) Quadrants[6].Add(new BodyPartProbability(kvp.Key, bodyPart.HLB));
                if (bodyPart.MLB > 0.0f) Quadrants[7].Add(new BodyPartProbability(kvp.Key, bodyPart.MLB));
                if (bodyPart.LLB > 0.0f) Quadrants[8].Add(new BodyPartProbability(kvp.Key, bodyPart.LLB));

                if (bodyPart.HRB > 0.0f) Quadrants[9].Add(new BodyPartProbability(kvp.Key, bodyPart.HRB));
                if (bodyPart.MRB > 0.0f) Quadrants[10].Add(new BodyPartProbability(kvp.Key, bodyPart.MRB));
                if (bodyPart.LRB > 0.0f) Quadrants[11].Add(new BodyPartProbability(kvp.Key, bodyPart.LRB));
            }
        }

        public CombatBodyPart RollBodyPart(Quadrant quadrant)
        {
            var idx = (int)quadrant.GetIndex();

            var bodyParts = Quadrants[idx];

            var total = bodyParts.Sum(i => i.Probability);

            if (total == 0.0f)
                return CombatBodyPart.Undefined;

            var rng = ThreadSafeRandom.Next(0.0f, total);

            var totalProbability = 0.0f;
            foreach (var bodyPart in bodyParts)
            {
                totalProbability += bodyPart.Probability;
                if (rng < totalProbability)
                    return bodyPart.BodyPart;
            }
            return CombatBodyPart.Undefined;
        }
    }
}
