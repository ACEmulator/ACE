using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Common
{
    public class PartCell
    {
        public int NumShadowParts;
        public List<ShadowPart> ShadowPartList;

        public PartCell()
        {
            ShadowPartList = new List<ShadowPart>();
        }

        public void AddPart(PhysicsPart part, List<int> planes, AFrame frame, int numShadowParts)
        {
            var shadowPart = new ShadowPart();
            if (planes != null)
                shadowPart = new ShadowPart(1, planes, frame, part);
            else
                shadowPart = new ShadowPart(null, null, part);

            ShadowPartList.Add(shadowPart);
            NumShadowParts++;
        }

        public void RemovePart(PhysicsPart part)
        {
            var removeParts = new List<ShadowPart>();

            foreach (var shadowPart in ShadowPartList)
            {
                if (!part.Equals(shadowPart.Part)) continue;
                removeParts.Add(shadowPart);
            }

            foreach (var removePart in removeParts)
                ShadowPartList.Remove(removePart);

            NumShadowParts -= removeParts.Count;
        }
    }
}
