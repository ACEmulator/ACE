using ACE.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity.Objects;

namespace ACE.Network.Motion
{
    public abstract class MotionState
    {
        public bool IsAutonomous { get; }
        public virtual byte[] GetPayload(WorldObject animationTarget)
        {
            return null;
        }
    }
}
