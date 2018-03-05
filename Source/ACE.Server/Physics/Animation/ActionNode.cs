namespace ACE.Server.Physics.Animation
{
    public class ActionNode
    {
        public int Action;
        public float Speed;
        public int Stamp;
        public bool Autonomous;

        public ActionNode() { }

        public ActionNode(int action, float speed, int stamp, bool autonomous)
        {
            Action = action;
            Speed = speed;
            Stamp = stamp;
            Autonomous = autonomous;
        }
    }
}
