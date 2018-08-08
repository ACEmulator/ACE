using System;
using System.Numerics;

namespace ACE.Server.Physics.Common
{
    public enum SetPositionError
    {
        OK = 0x0,
        GeneralFailure = 0x1,
        NoValidPosition = 0x2,
        NoCell = 0x3,
        Collided = 0x4,
        InvalidArguments = 0x100
    };

    [Flags]
    public enum SetPositionFlags
    {
        Placement = 0x1,
        Teleport = 0x2,
        Restore = 0x4,
        Slide = 0x10,
        DontCreateCells = 0x20,
        Scatter = 0x100,
        RandomScatter = 0x200,
        Line = 0x400,
        SendPositionEvent = 0x1000,
    };

    public class SetPosition
    {
        public Position Pos;
        public SetPositionFlags Flags;
        public Vector3 Line;
        public float RadX;
        public float RadY;
        public int NumTries;

        public SetPosition() { }

        public SetPosition(Position pos, SetPositionFlags flags)
        {
            Pos = pos;
            Flags = flags;
        }
    }
}
