using ACE.Entity;

namespace ACE.DatLoader.Entity
{
    public class CSphere
    {
        public AceVector3 Origin { get; set; }
        public float Radius { get; set; }

        public CSphere()
        {
            Origin = new AceVector3(0, 0, 0);
            Radius = 0;
        }

        public CSphere(AceVector3 origin, float radius)
        {
            Origin = origin;
            Radius = radius;
        }
    }
}
