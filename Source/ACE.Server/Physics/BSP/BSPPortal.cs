using System;
using System.Collections.Generic;

namespace ACE.Server.Physics.BSP
{
    public class BSPPortal: BSPNode, IEquatable<BSPPortal>
    {
        public int NumPortals;
        public List<PortalPoly> Portals;

        public BSPPortal() : base() { }

        public void portal_draw_portals_only(int portalPolyOrPortalContents)
        {
            // rendering stuff
        }

        public bool Equals(BSPPortal portal)
        {
            if (!base.Equals(portal) || NumPortals != portal.NumPortals)
                return false;

            for (var i = 0; i < NumPortals; i++)
            {
                if (!Portals[i].Equals(portal.Portals[i]))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            var hash = base.GetHashCode();

            hash = (hash * 397) ^ NumPortals.GetHashCode();

            for (var i = 0; i < NumPortals; i++)
                hash = (hash * 397) ^ Portals[i].GetHashCode();

            return hash;
        }
    }
}
