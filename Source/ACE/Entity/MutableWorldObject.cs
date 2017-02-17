using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity.Enum;

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
        
    }
}
