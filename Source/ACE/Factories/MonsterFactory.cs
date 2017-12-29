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
        public static async Task<Monster> SpawnMonster(AceObject aceO, Position position)
        {
            Monster newMonster = await WorldObject.CreateWorldObject<Monster>(aceO);
            newMonster.Location = position;
            newMonster.GeneratorId = aceO.GeneratorIID;

            // newMonster.PhysicsData.DefaultScript = aceO.PhysicsScript;
            // newMonster.DefaultScript = (uint)Network.Enum.PlayScript.Create;

            return newMonster;
        }
    }
}
