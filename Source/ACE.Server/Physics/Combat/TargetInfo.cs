using System.Numerics;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Combat
{
    public enum TargetStatus
    {
        Undef_TargetStatus = 0x0,
        Ok_TargetStatus = 0x1,
        ExitWorld_TargetStatus = 0x2,
        Teleported_TargetStatus = 0x3,
        Contained_TargetStatus = 0x4,
        Parented_TargetStatus = 0x5,
        TimedOut_TargetStatus = 0x6,
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
