namespace ACE.DatLoader.Entity
{
    public class PortalPoly
    {
        public int PortalIndex { get; set; }
        public int PolygonId { get; set; }

        // In a BSPPortal, this is set by looking up the PolygonID in the "InPolys" list 
        public Polygon Portal { get; set; } 

        public static PortalPoly Read(DatReader datReader)
        {
            PortalPoly obj = new PortalPoly();
            obj.PortalIndex = datReader.ReadInt16();
            obj.PolygonId = datReader.ReadInt16();
            return obj;
        }
    }
}
