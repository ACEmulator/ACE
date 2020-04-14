using System;
using ACE.DatLoader.Entity;
using ACE.DatLoader.Entity.AnimationHooks;
using ACE.Entity.Enum;

namespace ACE.Server.Physics.Hooks
{
    public class AnimHook
    {
        public static void Execute(PhysicsObj obj, AnimationHook animHook)
        {
            switch (animHook.HookType)
            {
                case AnimationHookType.AnimationDone:
                    obj.Hook_AnimDone();
                    break;

                /*case AnimationHookType.Ethereal:
                    if (animHook is EtherealHook hook)
                        obj.set_ethereal(Convert.ToBoolean(hook.Ethereal), false);
                    break;*/
            }
        }
    }
}
