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
        public uint ContextID;
        public uint ObjectID;
        public float Radius;
        public double Quantum;
        public Position TargetPosition;
        public Position InterpolatedPosition;
        public Vector3 InterpolatedHeading;
        public Vector3 Velocity;
        public TargetStatus Status;
        public double LastUpdateTime;

        public TargetInfo() { }

        public TargetInfo(uint contextID, uint objectID, float radius, double quantum)
        {
            ContextID = contextID;
            ObjectID = objectID;
            Radius = radius;
            Quantum = quantum;
            LastUpdateTime = PhysicsTimer.CurrentTime;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        public TargetInfo(TargetInfo info)
        {
            ContextID = info.ContextID;
            ObjectID = info.ObjectID;
            Radius = info.Radius;
            Quantum = info.Quantum;
            TargetPosition = new Position(info.TargetPosition);
            InterpolatedPosition = new Position(info.InterpolatedPosition);
            InterpolatedHeading = info.InterpolatedHeading;
            Velocity = info.Velocity;
            Status = info.Status;
            LastUpdateTime = info.LastUpdateTime;
        }
    }
}
