using System.Collections.Generic;

namespace ACE.Server.Physics.Animation
{
    public class AnimFrame
    {
        public List<AFrame> Frame;
        public int NumFrameHooks;
        public List<AnimHook> Hooks;
        public int NumParts;

        public AnimFrame()
        {
            Frame = new List<AFrame>();
            Hooks = new List<AnimHook>();
        }
    }
}
