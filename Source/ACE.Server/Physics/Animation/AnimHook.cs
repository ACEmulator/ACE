using System.Collections.Generic;

namespace ACE.Server.Physics.Animation
{
    public enum AnimHookDir
    {
        Unknown = -2,
        Backward = -1,
        Both = 0,
        Forward = 1,
    }

    public class AnimHook
    {
        public AnimHook NextHook;
        public AnimHookDir Direction;

        public bool Execute(PhysicsObj obj)
        {
            return false;
        }

        public void add_to_list(List<AnimHook> animList)
        {
            animList.Add(NextHook);
        }
    }
}
