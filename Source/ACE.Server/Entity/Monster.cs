using ACE.Entity;
using ACE.Server.Managers;

namespace ACE.Server.Entity
{
    public class Monster : Creature
    {
        public Monster(AceObject aceO)
          : base(aceO)
        {
            Guid = GuidManager.NewNonStaticGuid();
            IsAlive = true;
            SetupVitals();
        }
    }
}
