using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
