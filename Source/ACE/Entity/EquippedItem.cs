using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using System.Collections.Generic;
using System.IO;

namespace ACE.Entity
{
    /// <summary>
    /// This Class is used to add children
    /// </summary>
    public class HeldItem
    {
        public uint Guid { get; }

        public uint LocationId { get; }

        public EquipMask EquipMask { get; }

        public HeldItem(uint guid, uint locationId, EquipMask equipmask)
        {
            Guid = guid;
            EquipMask = equipmask;
            LocationId = locationId;
        }
    }
}
