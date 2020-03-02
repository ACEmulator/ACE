using ACE.Entity.Enum;

namespace ACE.Server.Entity
{
    public class BodyPartProbability
    {
        public CombatBodyPart BodyPart;
        public float Probability;

        public BodyPartProbability(CombatBodyPart bodyPart, float probability)
        {
            BodyPart = bodyPart;
            Probability = probability;
        }
    }
}
