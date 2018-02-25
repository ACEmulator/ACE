using System;

namespace ACE.Server.Physics.Hooks
{
    [Flags]
    public enum HookType
    {
        Setup = 0x1,
        MotionTable = 0x2,
        Velocity = 0x4,
        Acceleration = 0x8,
        Omega = 0x10,
        Parent = 0x20,
        Children = 0x40,
        ObjScale = 0x80,
        Friction = 0x100,
        Elasticity = 0x200,
        Timestamps = 0x400,
        Stable = 0x800,
        PETable = 0x1000,
        DefaultScript = 0x2000,
        DefaultScriptIntensity = 0x4000,
        Position = 0x8000,
        Movement = 0x10000,
        AnimFrameID = 0x20000,
        Translucency = 0x40000,
    };
    public class PhysicsObjHook
    {
        public HookType HookType;
        public double TimeCreated;
        public double InterpolationTime;
        public Object UserData;
    }
}
