using ACE.Entity;

namespace ACE.DatLoader.Entity
{
    public class CylSphere
    {
        public AceVector3 Origin { get; set; }
        public float Radius { get; set; }
        public float Height { get; set; }

        public CylSphere(AceVector3 origin, float radius, float height)
        {
            this.Origin = origin;
            this.Radius = radius;
            this.Height = height;
        }
    }
}
