using System.Collections.Generic;

namespace ACE.Server.Physics.Animation
{
    public class Animation
    {
        public List<AFrame> PosFrames;
        public List<AnimFrame> PartFrames;
        public bool HasHooks;
        public int NumParts;
        public int NumFrames;
    }
}
