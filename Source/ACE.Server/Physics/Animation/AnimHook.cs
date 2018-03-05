using System.Collections.Generic;

namespace ACE.Server.Physics.Animation
{
    public class AnimHook
    {
        public AnimHook NextHook;
        public int Direction;

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
