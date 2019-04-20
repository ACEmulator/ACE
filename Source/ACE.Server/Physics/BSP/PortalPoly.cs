using System;

namespace ACE.Server.Physics.BSP
{
    public class PortalPoly: IEquatable<PortalPoly>
    {
        public int PortalIdx;
        public Polygon Portal;

        public PortalPoly()
        {
            PortalIdx = -1;
        }

        public bool Equals(PortalPoly pPoly)
        {
            return PortalIdx == pPoly.PortalIdx && Portal.Equals(pPoly.Portal);
        }

        public override int GetHashCode()
        {
            int hash = 0;

            hash = (hash * 397) ^ PortalIdx.GetHashCode();
            hash = (hash * 397) ^ Portal.GetHashCode();

            return hash;
        }
    }
}
