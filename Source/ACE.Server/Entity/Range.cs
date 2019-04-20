using System;
using System.Collections.Generic;
using System.Text;

namespace ACE.Server.Entity
{
    public class Range
    {
        public float Min;
        public float Max;
        public float Avg;

        public Range() { }

        public Range(float min, float max)
        {
            Min = min;
            Max = max;
            Avg = (Min + Max) / 2.0f;
        }

        public override string ToString()
        {
            return $"{Min} - {Max}";
        }
    }
}
