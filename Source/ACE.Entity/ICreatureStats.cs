using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public interface ICreatureStats
    {
        uint Strength { get; }
        
        uint Endurance { get; }
        
        uint Coordination { get; }
        
        uint Quickness { get; }
        
        uint Focus { get; }
        
        uint Self { get; }
    }
}
