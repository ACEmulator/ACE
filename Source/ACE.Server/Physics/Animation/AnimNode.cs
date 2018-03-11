namespace ACE.Server.Physics.Animation
{
    public class AnimNode
    {
        public uint Motion;
        public uint NumAnims;

        public AnimNode() { }

        public AnimNode(uint motion, uint numAnims)
        {
            Motion = motion;
            NumAnims = numAnims;
        }
    }
}
