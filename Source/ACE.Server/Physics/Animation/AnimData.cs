namespace ACE.Server.Physics.Animation
{
    public class AnimData
    {
        public uint AnimID;
        public int LowFrame;
        public int HighFrame;
        public float Framerate;

        public AnimData() { }

        public AnimData(DatLoader.Entity.AnimData animData, float speed = 1.0f)
        {
            AnimID = animData.AnimId;
            LowFrame = animData.LowFrame;
            HighFrame = animData.HighFrame;
            Framerate = animData.Framerate * speed;
        }
    }
}
