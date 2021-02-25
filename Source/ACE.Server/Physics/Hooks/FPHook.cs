namespace ACE.Server.Physics.Hooks
{
    public class FPHook: PhysicsObjHook
    {
        public float StartValue;
        public float EndValue;

        public FPHook(PhysicsHookType type, double timeCreated, double delta, float startValue, float endValue, object userData)
        {
            HookType = type;
            TimeCreated = timeCreated;
            InterpolationTime = delta;
            UserData = userData;
            StartValue = startValue;
            EndValue = endValue;
        }

        public override bool Execute(PhysicsObj obj)
        {
            var deltaTime = obj.PhysicsTimer_CurrentTime - TimeCreated;
            var scale = 0.0;
            if (deltaTime > 0.0)
            {
                if (deltaTime < InterpolationTime)
                    scale = deltaTime / InterpolationTime;
                else
                    scale = 1.0;
            }
            var current = (EndValue - StartValue) * scale + StartValue;
            obj.process_fp_hook(HookType, (float)current, UserData);
            return scale == 1.0;
        }
    }
}
