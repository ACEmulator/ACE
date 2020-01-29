using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Entity.Enum;
using ACE.Entity.Models;

namespace ACE.Server.Entity
{
    public class BodyPartTable
    {
        public Weenie Weenie;

        public readonly List<BodyPartProbability>[] Quadrants = new List<BodyPartProbability>[12];

        public BodyPartTable(Weenie weenie)
        {
            Weenie = weenie;

            for (var i = 0; i < Quadrants.Length; i++)
                Quadrants[i] = new List<BodyPartProbability>();

            foreach (var bodyPart in Weenie.PropertiesBodyPart)
            {
                if (bodyPart.Value.HLF > 0.0f) Quadrants[0].Add(new BodyPartProbability(bodyPart.Key, bodyPart.Value.HLF));
                if (bodyPart.Value.MLF > 0.0f) Quadrants[1].Add(new BodyPartProbability(bodyPart.Key, bodyPart.Value.MLF));
                if (bodyPart.Value.LLF > 0.0f) Quadrants[2].Add(new BodyPartProbability(bodyPart.Key, bodyPart.Value.LLF));

                if (bodyPart.Value.HRF > 0.0f) Quadrants[3].Add(new BodyPartProbability(bodyPart.Key, bodyPart.Value.HRF));
                if (bodyPart.Value.MRF > 0.0f) Quadrants[4].Add(new BodyPartProbability(bodyPart.Key, bodyPart.Value.MRF));
                if (bodyPart.Value.LRF > 0.0f) Quadrants[5].Add(new BodyPartProbability(bodyPart.Key, bodyPart.Value.LRF));

                if (bodyPart.Value.HLB > 0.0f) Quadrants[6].Add(new BodyPartProbability(bodyPart.Key, bodyPart.Value.HLB));
                if (bodyPart.Value.MLB > 0.0f) Quadrants[7].Add(new BodyPartProbability(bodyPart.Key, bodyPart.Value.MLB));
                if (bodyPart.Value.LLB > 0.0f) Quadrants[8].Add(new BodyPartProbability(bodyPart.Key, bodyPart.Value.LLB));

                if (bodyPart.Value.HRB > 0.0f) Quadrants[9].Add(new BodyPartProbability(bodyPart.Key, bodyPart.Value.HRB));
                if (bodyPart.Value.MRB > 0.0f) Quadrants[10].Add(new BodyPartProbability(bodyPart.Key, bodyPart.Value.MRB));
                if (bodyPart.Value.LRB > 0.0f) Quadrants[11].Add(new BodyPartProbability(bodyPart.Key, bodyPart.Value.LRB));
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
                    return (CombatBodyPart)bodyPart.BodyPart;
            }
            return CombatBodyPart.Undefined;
        }
    }
}
