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
    public class EquippedItem
    {
        public uint Guid { get; }

        public EquipMask EquipMask { get; }

        public EquippedItem(uint guid, EquipMask equipmask)
        {
            Guid = guid;
            EquipMask = equipmask;
        }
    }
}
