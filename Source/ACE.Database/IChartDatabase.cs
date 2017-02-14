using ACE.Entity;

namespace ACE.Database
{
    public interface IChartDatabase
    {
        /// <summary>
        /// reads a data source to load the leveling xp chart
        /// </summary>
        LevelingChart GetLevelingXpChart();

        /// <summary>
        /// reads a data source to load the xp chart for abililties
        /// </summary>
        ExperienceExpenditureChart GetAbilityXpChart();

        /// <summary>
        /// reads a data source to load the xp chart for vitals
        /// </summary>
        ExperienceExpenditureChart GetVitalXpChart();

        /// <summary>
        /// reads a data source to load the xp chart for specialized skills
        /// </summary>
        ExperienceExpenditureChart GetSpecializedSkillXpChart();

        /// <summary>
        /// reads a data source to load the xp chart for trained skills
        /// </summary>
        ExperienceExpenditureChart GetTrainedSkillXpChart();
    }
}
