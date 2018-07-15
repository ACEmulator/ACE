using ACE.DatLoader.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.Physics.Hooks
{
    public class AnimHook: PhysicsObjHook
    {
        public AnimationHookDir Direction;

        public AnimHook() { }

        public AnimHook(AnimationHook animHook)
        {
            HookType = (HookType)animHook.HookType;
            Direction = animHook.Direction;
        }

        public override bool Execute(PhysicsObj obj)
        {
            obj.Hook_AnimDone();
            return true;
        }
    }
}
