namespace ACE.Server.Physics.Animation
{
    public class MovementNode
    {
        public MovementType Type;
        public float Heading;

        public MovementNode() { }

        public MovementNode(MovementType type)
        {
            Type = type;
        }

        public MovementNode(MovementType type, float heading)
        {
            Type = type;
            Heading = heading;
        }
    }
}
