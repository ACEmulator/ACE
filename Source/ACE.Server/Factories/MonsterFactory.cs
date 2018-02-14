using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Server.Entity;
using ACE.Server.Entity.WorldObjects;

namespace ACE.Server.Factories
{
    public class MonsterFactory
    {
        /// <summary>
        /// Create a new monster at the specified position
        /// </summary>
        public static Monster SpawnMonster(Weenie weenie, Position position)
        {
            Monster newMonster = new Monster(weenie);
            newMonster.Location = position;
            throw new System.NotImplementedException();
            //newMonster.GeneratorId = aceO.GeneratorIID;

            // newMonster.PhysicsData.DefaultScript = aceO.PhysicsScript;
            // newMonster.DefaultScript = (uint)Network.Enum.PlayScript.Create;

            return newMonster;
        }
    }
}
