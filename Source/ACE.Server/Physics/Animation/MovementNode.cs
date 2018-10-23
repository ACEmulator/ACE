using ACE.Entity.Enum;

namespace ACE.Server.Physics.Animation
{
    public class MovementNode
    {
        public MovementType Type;
        public float Heading;

        public MovementNode() { }

        public MovementNode(MovementType type, float heading = 0.0f)
        {
            Type = type;
            Heading = heading;
        }
    }
}
