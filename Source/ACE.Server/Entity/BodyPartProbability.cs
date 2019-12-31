namespace ACE.Server.Entity
{
    public class BodyPartProbability
    {
        public ushort BodyPart;
        public float Probability;

        public BodyPartProbability(ushort bodyPart, float probability)
        {
            BodyPart = bodyPart;
            Probability = probability;
        }
    }
}
