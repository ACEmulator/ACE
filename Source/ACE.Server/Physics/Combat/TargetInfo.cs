using System.Numerics;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Combat
{
    public enum TargetStatus
    {
        Undefined = 0x0,
        OK = 0x1,
        ExitWorld = 0x2,
        Teleported = 0x3,
        Contained = 0x4,
        Parented = 0x5,
        TimedOut = 0x6,
    };

    public class TargetInfo
    {
        public int ContextID;
        public int ObjectID;
        public float Radius;
        public double Quantum;
        public Position TargetPosition;
        public Position InterpolatedPosition;
        public Vector3 InterpolatedHeading;
        public Vector3 Velocity;
        public TargetStatus Status;
        public double LastUpdateTime;
    }
}
