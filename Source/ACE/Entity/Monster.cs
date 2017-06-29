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
            : base(aceO, new ObjectGuid(GuidManager.NewItemGuid()))
        {
            // TODO: Check why Drudges don't have a healthbar when you select them
            aceO.AceObjectId = new ObjectGuid(GuidManager.NewItemGuid()).Full;
            IsAlive = true;
            SetupVitals();
        }
    }
}
