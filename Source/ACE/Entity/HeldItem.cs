using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using System.Collections.Generic;
namespace ACE.Entity
{
    /// <summary>
    /// This Class is used to add children
    /// </summary>
    public class HeldItem
    {
        public uint Guid { get; }

        public int LocationId { get; }

        public EquipMask EquipMask { get; }

        public HeldItem(uint guid, int locationId, EquipMask equipmask)
        {
            Guid = guid;
            EquipMask = equipmask;
            LocationId = locationId;
        }
    }
}
