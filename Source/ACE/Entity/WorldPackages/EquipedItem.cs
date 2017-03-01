using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using System.Collections.Generic;
using System.IO;

namespace ACE.Entity.WorldPackages
{
    /// <summary>
    /// This Class is used to add children 
    /// </summary>
    public class EquipedItem
    {
        public uint Guid { get; }
        public EquipMask EquipMask { get; }

        public EquipedItem(uint guid, EquipMask equipmask)
        {
            Guid = guid;
            EquipMask = equipmask;
        }
    }
}
