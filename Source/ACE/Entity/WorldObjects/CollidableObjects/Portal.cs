using ACE.Entity.Enum;
using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Entity
{
    public class Portal : CollidableObject
    {
        public Portal(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid, name, weenieClassId, descriptionFlag, weenieFlag, position)
        {
        }

        public Portal(AceObject aceObject)
            : base(aceObject)
        {
        }

        public override void OnCollide(Player player)
        {
            // TODO: Implement
        }
    }
}
