using ACE.DatLoader.Entity;

namespace ACE.Server.Physics.Hooks
{
    public class AnimDoneHook: AnimHook
    {
        public AnimDoneHook() { }

        public AnimDoneHook(AnimationHook animHook)
            : base(animHook) { }

        public override bool Execute(PhysicsObj obj)
        {
            obj.Hook_AnimDone();
            return true;
        }
    }
}
