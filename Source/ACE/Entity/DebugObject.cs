﻿using ACE.Managers;

namespace ACE.Entity
{
    public class DebugObject : CollidableObject
    {
        public DebugObject(AceObject aceObject)
            : base(aceObject)
        {
        }

        public DebugObject(ObjectGuid guid, AceObject aceObject)
            : base(guid, aceObject)
        {
            aceObject.AceObjectId = new ObjectGuid(GuidManager.NewItemGuid()).Full;
        }

        public override void OnCollide(ObjectGuid playerId)
        {
            // TODO: Implement
        }
    }
}
