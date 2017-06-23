using ACE.Entity;
using ACE.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Network;

namespace ACE.Factories
{
    public class MonsterFactory
    {
        /// <summary>
        /// Create a new monster at the specified position
        /// </summary>
        public static Monster SpawnMonster(AceObject aceO, Position position)
        {
            aceO.Location = position;
            Monster newMonster = new Monster(aceO);

            return newMonster;
        }
    }
}
