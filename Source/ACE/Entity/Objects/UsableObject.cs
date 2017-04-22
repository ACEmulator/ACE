using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Entity.Objects
{
    public class UsableObject : WorldObject
    {
        public UsableObject(ObjectType type, ObjectGuid guid)
            : base(type, guid)
        {
        }

        public UsableObject(ObjectType type, ObjectGuid guid, string name, ushort weenieClassId, ObjectDescriptionFlag descriptionFlag, WeenieHeaderFlag weenieFlag, Position position)
            : base(type, guid)
        {
            this.Name = name;
            this.DescriptionFlags = descriptionFlag;
            this.WeenieFlags = weenieFlag;
            this.Location = position;
            this.WeenieClassid = weenieClassId;
        }

        public virtual void OnUse(Player player)
        {
            // todo: implement.  default is probably to pick it up off the ground
        }
    }
}
