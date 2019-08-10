namespace ACE.Server.Entity
{
    public class CreateListSetModifier
    {
        public CreateListSet Set;
        public float Modifier;

        /// <summary>
        /// Usually Modifier, unless Set.TrophyProbability * Modifier > 1.0
        /// In which case, TrophyMod is capped so Set.TrophyProbability * TrophyMod = 1.0
        /// </summary>
        public float TrophyMod;

        public float NoneMod => NoneProbability / Set.NoneProbability;

        public float TrophyProbability => Set.TrophyProbability * TrophyMod;

        public float NoneProbability => 1.0f - TrophyProbability;

        public CreateListSetModifier()
        {
            Modifier = 1.0f;
        }

        public CreateListSetModifier(CreateListSet set, float modifier)
        {
            Set = set;
            Modifier = modifier;

            // calculate TrophyMod
            var trophyProbability = Set.TrophyProbability;

            if (trophyProbability * modifier > 1.0f)
                TrophyMod = 1.0f / trophyProbability;
            else
                TrophyMod = modifier;
        }
    }
}
