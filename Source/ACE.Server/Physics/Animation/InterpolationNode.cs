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
        public int Type;
        public Position P;
        public Vector3 V;
        public float Extent;
    }
}
