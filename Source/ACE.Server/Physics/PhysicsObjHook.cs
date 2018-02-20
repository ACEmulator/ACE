namespace ACE.Server.Physics
{
    public class PhysicsObjHook
    {
        public int HookType;
        public double TimeCreated;
        public double InterpolationTime;
        public PhysicsObjHook Prev;
        public PhysicsObjHook Next;
        public object UserData;
    }
}
