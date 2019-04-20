using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Entity
{
    public class AnimData
    {
        public int AnimId { get; set; }
        public int LowFrame { get; set; }
        public int HighFrame { get; set; }
        /// <summary>
        /// Negative framerates play animation in reverse
        /// </summary>
        public float Framerate { get; set; }

        public AnimData()
        {

        }

        public AnimData(int animationID, int lowFrame, int highFrame, float framerate)
        {
            AnimId = animationID;
            LowFrame = lowFrame;
            HighFrame = highFrame;
            Framerate = framerate;
        }
    }
}
