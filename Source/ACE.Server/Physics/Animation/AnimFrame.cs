using System.Collections.Generic;
using ACE.Entity;

namespace ACE.Server.Physics.Animation
{
    public class AnimFrame
    {
        public List<AFrame> Frame;
        public int NumFrameHooks;
        public AnimHook Hooks;
        public int NumParts;
    }
}
