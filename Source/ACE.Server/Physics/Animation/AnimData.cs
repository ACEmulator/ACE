using System;
using log4net;

namespace ACE.Server.Physics.Animation
{
    public class AnimData
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            if (speed == 0.0f)
            {
                log.Warn("Warning: AnimData speed == 0");
                log.Warn($"AnimID: {AnimID:X8}, LowFrame: {LowFrame}, HighFrame: {HighFrame}, Framerate: {animData.Framerate}, Speed: {speed}");
                log.Warn(Environment.StackTrace);
            }
        }
    }
}
