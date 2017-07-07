using ACE.Entity.Enum;
using ACE.Factories;
using ACE.Managers;
using ACE.Network.Enum;
using ACE.Network.Motion;
using System;

namespace ACE.Entity
{
    public class Monster : Creature
    {
        public Monster(AceObject aceO)
          : base(aceO)
        {
            aceO.AceObjectId = new ObjectGuid(GuidManager.NewMonsterGuid()).Full;
            IsAlive = true;
            SetupVitals();
        }
    }
}
