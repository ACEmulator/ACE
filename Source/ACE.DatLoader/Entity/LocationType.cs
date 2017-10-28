using ACE.Entity;

namespace ACE.DatLoader.Entity
{
    public class LocationType
    {

        public uint PartId;
        public Position Frame;

        public static LocationType Read(DatReader datReader)
        {
            LocationType obj = new LocationType();

            obj.PartId = datReader.ReadUInt32();
            obj.Frame = PositionExtensions.ReadPosition(datReader);

            return obj;
        }
    }
}
