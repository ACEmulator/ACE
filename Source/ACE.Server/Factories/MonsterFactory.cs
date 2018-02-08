using ACE.Entity;
using ACE.Server.Entity;

namespace ACE.Server.Factories
{
    public class MonsterFactory
    {
        /// <summary>
        /// Create a new monster at the specified position
        /// </summary>
        public static Monster SpawnMonster(AceObject aceO, Position position)
        {
            Monster newMonster = new Monster(aceO);
            newMonster.Location = position;
            newMonster.GeneratorId = aceO.GeneratorIID;

            // newMonster.PhysicsData.DefaultScript = aceO.PhysicsScript;
            // newMonster.DefaultScript = (uint)Network.Enum.PlayScript.Create;

            return newMonster;
        }
    }
}
