using ACE.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.Motion
{
    public abstract class MotionState
    {
        public bool IsAutonomous { get; set; }

        public virtual byte[] GetPayload(WorldObject animationTarget, float distanceFromObject = 0.6f)
        {
            return null;
        }        
    }
}
