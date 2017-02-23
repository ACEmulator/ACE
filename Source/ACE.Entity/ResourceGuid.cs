using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class ResourceGuid
    {
        public uint Full { get; }

        public uint Low => Full & 0xFFFFFF;

        public ResourceGuidType ResourceType => (ResourceGuidType)(Full >> 24);

        public ResourceGuid(uint full) { Full = full; }

        public ResourceGuid(uint low, ResourceGuidType high)
        {
            Full = low | ((uint)high << 24);
        }
    }
}
