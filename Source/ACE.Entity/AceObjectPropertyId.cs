using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public struct AceObjectPropertyId
    {
        public AceObjectPropertyId(uint propertyId, AceObjectPropertyType propertyType)
        {
            PropertyId = propertyId;
            PropertyType = propertyType;
        }

        public uint PropertyId { get; set; }

        public AceObjectPropertyType PropertyType { get; set; }
    }
}
