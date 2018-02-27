using ACE.Entity.Enum.Properties;

namespace ACE.Entity
{
    public struct GenericPropertyId
    {
        public GenericPropertyId(uint propertyId, PropertyType propertyType)
        {
            PropertyId = propertyId;
            PropertyType = propertyType;
        }

        public uint PropertyId { get; set; }

        public PropertyType PropertyType { get; set; }
    }
}
