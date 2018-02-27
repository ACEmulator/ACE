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
    }
}
