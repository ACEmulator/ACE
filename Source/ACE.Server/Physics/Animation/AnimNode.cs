namespace ACE.Server.Physics.Animation
{
    public class AnimNode
    {
        public int Motion;
        public int NumAnims;

        public AnimNode() { }

        public AnimNode(int motion, int numAnims)
        {
            Motion = motion;
            NumAnims = numAnims;
        }
    }
}
