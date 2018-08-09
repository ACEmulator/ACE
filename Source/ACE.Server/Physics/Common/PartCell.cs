using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Common
{
    public class PartCell
    {
        public List<ShadowPart> ShadowPartList;

        public PartCell()
        {
            ShadowPartList = new List<ShadowPart>();
        }

        public void AddPart(PhysicsPart part, List<int> planes, AFrame frame, int numShadowParts)
        {
            if (part == null) return;

            var shadowPart = new ShadowPart();
            if (planes != null)
                shadowPart = new ShadowPart(1, planes, frame, part);
            else
                shadowPart = new ShadowPart(null, null, part);

            ShadowPartList.Add(shadowPart);
        }

        public void RemovePart(PhysicsPart part)
        {
            // FIXME: null parts
            if (part == null) return;

            var removeParts = new List<ShadowPart>();

            foreach (var shadowPart in ShadowPartList.ToList())
            {
                if (part.Equals(shadowPart))
                    removeParts.Add(shadowPart);
            }

            foreach (var removePart in removeParts)
                ShadowPartList.Remove(removePart);
        }
    }
}
