namespace ACE.Server.Physics.Hooks
{
    public class FPHook: PhysicsObjHook
    {
        public float StartValue;
        public float EndValue;

        public FPHook(HookType type, double timeCreated, double delta, float startValue, float endValue, object userData)
        {
            HookType = type;
            TimeCreated = timeCreated;
            InterpolationTime = delta;
            UserData = userData;
            StartValue = startValue;
            EndValue = endValue;
        }
    }
}
