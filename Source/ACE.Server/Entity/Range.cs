using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.Entity
{
    public class Range
    {
        public float Min;
        public float Max;

        public Range() { }

        public Range(float min, float max)
        {
            Min = min;
            Max = max;
        }
    }
}
