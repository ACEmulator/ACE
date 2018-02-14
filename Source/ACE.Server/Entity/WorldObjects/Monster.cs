using ACE.Database.Models.World;
using ACE.Server.Managers;

namespace ACE.Server.Entity.WorldObjects
{
    public class Monster : Creature
    {
        public Monster(Weenie weenie) : base(weenie)
        {
            Guid = GuidManager.NewDynamicGuid();
            IsAlive = true;
            SetupVitals();
        }
    }
}
