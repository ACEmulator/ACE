using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity.Enum;
using ACE.Entity.Events;

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
        /// raised whenever this object moves
        /// </summary>
        public event EventHandler OnMove;

        public override Position Position
        {
            get { return base.Position; }
            protected set
            {
                base.Position = value;

                // TODO: should we be passing event args?  "this" has everything needed, i believe
                if (OnMove != null)
                    OnMove(this, null);
            }
        }
    }
}
