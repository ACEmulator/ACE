using System.Threading.Tasks;

using ACE.Managers;

namespace ACE.Entity
{
    public class Monster : Creature
    {
        public Monster()
        {
        }

        protected override async Task Init(AceObject aceO)
        {
            await base.Init(aceO);
            Guid = GuidManager.NewNonStaticGuid();
            IsAlive = true;
            SetupVitals();
        }
    }
}
