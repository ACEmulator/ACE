using ACE.Entity;

namespace ACE.DatLoader.Entity
{
    public class Plane
    {
        public Position N { get; set; }
        public float D { get; set; }

        public static Plane Read(DatReader datReader)
        {
            Plane obj = new Plane();

            obj.N = PositionExtensions.ReadPositionFrame(datReader);
            obj.D = datReader.ReadSingle();

            return obj;
        }
    }
}
