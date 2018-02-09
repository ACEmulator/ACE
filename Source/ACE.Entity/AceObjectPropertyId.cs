using ACE.Entity.Enum;

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
