using System.Numerics;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public enum InterpolationNodeType
    {
        INVALID_TYPE = 0x0,
        POSITION_TYPE = 0x1,
        JUMP_TYPE = 0x2,
        VELOCITY_TYPE = 0x3,
        FORCE_InterpolationNode_enum_32_BIT = 0x7FFFFFFF,
    };

    public class InterpolationNode
    {
        public int Type;
        public Position P;
        public Vector3 V;
        public float Extent;
    }
}
