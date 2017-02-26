using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class MotionStateDirection
    {
        public uint Command { get; set; }

        public uint HoldKey { get; set; }

        public float Speed { get; set; } = 1.0f;
    }
}
