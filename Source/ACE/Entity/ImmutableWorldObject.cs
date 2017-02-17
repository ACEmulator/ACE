using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    /// <summary>
    /// temporary portals (b/c they can disappear), corpses, items on the ground, etc.
    /// </summary>
    public class ImmutableWorldObject : WorldObject
    {
        public ImmutableWorldObject(ObjectType type, ObjectGuid guid) : base(type, guid)
        {
        }
    }
}
