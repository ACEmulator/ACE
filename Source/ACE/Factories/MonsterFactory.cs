using ACE.Entity;
using ACE.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Network;
using ACE.Entity.Enum;

namespace ACE.Factories
{
    public class MonsterFactory
    {
        /// <summary>
        /// Create a new monster at the specified position
        /// </summary>
        public static Monster SpawnMonster(AceObject aceO, Position position)
        {
            aceO.SetPosition(PositionType.Location, position);
            // aceO.PhysicsScript = (ushort)Network.Enum.PlayScript.Create;
            Monster newMonster = new Monster(aceO);

            // newMonster.PhysicsData.DefaultScript = aceO.PhysicsScript;
            // newMonster.DefaultScript = (uint)Network.Enum.PlayScript.Create;

            return newMonster;
        }
    }
}
