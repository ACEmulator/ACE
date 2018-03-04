using System.Numerics;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public enum InterpolationNodeType
    {
        InvalidType     = 0x0,
        PositionType    = 0x1,
        JumpType        = 0x2,
        VelocityType    = 0x3,
    };

    public class InterpolationNode
    {
        public InterpolationNodeType Type;
        public Position Position;
        public Vector3 Velocity;
        public float Extent;

        public InterpolationNode() { }

        public InterpolationNode(InterpolationNodeType type, Position position)
        {
            Type = type;
            Position = position;
        }
    }
}
