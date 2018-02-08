using ACE.Entity.Enum;

namespace ACE.Entity
{
    public interface ICreatureXpSpendableStat
    {
        uint Ranks { get; set; }

        uint ExperienceSpent { get; set; }

        uint MaxValue { get; }

        uint UnbuffedValue { get; }

        uint Base { get; }

        Ability Ability { get; }

        uint Current { get; }
    }
}
