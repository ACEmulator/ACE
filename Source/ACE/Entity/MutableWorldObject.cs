using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity.Enum;
using ACE.Managers;

namespace ACE.Entity
{

    /// <summary>
    /// any world object that can change a state of some sort that requires clients be updated.  players, monsters,
    /// doors, etc.
    /// </summary>
    public abstract class MutableWorldObject : WorldObject
   
    {
        public MutableWorldObject(ObjectType type, ObjectGuid guid) : base(type, guid)
        {
        }

        /// <summary>
        /// tick-stamp for the last time the player moved
        /// </summary>
        public double LastMovedTicks { get; set; }

        /// <summary>
        /// tick-stamp for the last time a movement update was sent
        /// </summary>
        public double LastMovementBroadcastTicks { get; set; }

        /// <summary>
        /// tick-stamp for the server time of the last time the player moved.
        /// TODO: implement
        /// </summary>
        public double LastAnimatedTicks { get; set; }
        
        public override Position Position
        {
            get { return base.Position; }
            protected set
            {
                if (base.Position != null)
                    LastMovedTicks = WorldManager.PortalYearTicks;

                base.Position = value;                
            }
        }
    }
}