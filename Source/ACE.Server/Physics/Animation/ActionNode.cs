namespace ACE.Server.Physics.Animation
{
    public class ActionNode
    {
        public uint Action;
        public float Speed;
        public int Stamp;
        public bool Autonomous;

        public ActionNode() { }

        public ActionNode(uint action, float speed, int stamp, bool autonomous)
        {
            Action = action;
            Speed = speed;
            Stamp = stamp;
            Autonomous = autonomous;
        }
    }
}
