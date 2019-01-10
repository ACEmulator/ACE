using System.Collections.Generic;
using System.Linq;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Common
{
    public class PartCell
    {
        public List<ShadowPart> ShadowPartList { get; set; }

        public PartCell()
        {
            ShadowPartList = new List<ShadowPart>();
        }

        public void AddPart(PhysicsPart part, List<int> planes, AFrame frame, int numShadowParts)
        {
            return;

            /*
            if (part == null) return;

            var shadowPart = new ShadowPart();
            if (planes != null)
                shadowPart = new ShadowPart(1, planes, frame, part);
            else
                shadowPart = new ShadowPart(null, null, part);

            ShadowPartList.Add(shadowPart);
            */
        }

        public void RemovePart(PhysicsPart part)
        {
            return;

            /*
            if (part == null) return;

            var shadowPart = ShadowPartList.FirstOrDefault(p => p != null && p.Part != null && p.Part.Equals(part));

            if (shadowPart != null)
                ShadowPartList.Remove(shadowPart);
            */
        }
    }
}
