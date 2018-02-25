using System;

namespace ACE.Server.Physics.Animation
{
    [Flags]
    public enum ObjectInfoState
    {
        Default = 0x0,
        Contact = 0x1,
        OnWalkable = 0x2,
        IsViewer = 0x4,
        PathClipped = 0x8,
        FreeRotate = 0x10,
        PerfectClip = 0x40,
        IsImpenetrable = 0x80,
        IsPlayer = 0x100,
        EdgeSlide = 0x200,
        IgnoreCreates = 0x400,
        IsPK = 0x800,
        IsPKLite = 0x1000
    };

    public class ObjectInfo
    {
        public PhysicsObj Object;
        public ObjectInfoState State;
        public float Scale;
        public float StepUpHeight;
        public float StepDownHeight;
        public int Ethereal;
        public int StepDown;
        public int TargetID;

        public void Init(PhysicsObj obj, ObjectInfoState state)
        {

        }
    }
}
