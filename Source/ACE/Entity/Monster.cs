using ACE.Entity.Enum;
using ACE.Factories;
using ACE.Managers;
using ACE.Network.Enum;

namespace ACE.Entity
{
    public class Monster : Creature
    {
        public Monster(AceObject aceO)
            : base((ObjectType)aceO.ItemType,
                  // TODO: replace this with GuidManager.NewMonsterGuid once the GuidRanges are defined and GuidManager integrates with ObjectGuid class
                  new ObjectGuid(GuidManager.NewItemGuid()),
                  aceO.Name,
                  (ushort)aceO.WeenieClassId,
                  (ObjectDescriptionFlag)aceO.AceObjectDescriptionFlags,
                  (WeenieHeaderFlag)aceO.WeenieHeaderFlags,
                  aceO.Location)
        {
            // TODO: Check why Drudges don't appear on radar yet and don't have a healthbar when you select them
            if (aceO.WeenieClassId < 0x8000u)
                this.WeenieClassid = aceO.WeenieClassId;
            else
                this.WeenieClassid = (ushort)(aceO.WeenieClassId - 0x8000);

            SetObjectData(aceO);
            IsAlive = true;
            SetupVitals();
        }
    }
}
