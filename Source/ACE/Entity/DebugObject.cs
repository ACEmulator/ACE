using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.Motion;
using System;
using System.Diagnostics;

namespace ACE.Entity
{
    public class DebugObject : CollidableObject
    {
        public DebugObject(AceObject aceObject)
            : base(aceObject)
        {
        }

        public override void OnCollide(ObjectGuid playerId)
        {
            // TODO: Implement
        }
    }
}
