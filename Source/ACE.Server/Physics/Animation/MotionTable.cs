using System.Collections.Generic;

namespace ACE.Server.Physics.Animation
{
    public class MotionTable
    {
        public HashSet<long> StyleDefaults;
        public HashSet<MotionData> Cycles;
        public HashSet<MotionData> Modifiers;
        public HashSet<HashSet<MotionData>> Links;
        public int DefaultStyle;
    }
}
