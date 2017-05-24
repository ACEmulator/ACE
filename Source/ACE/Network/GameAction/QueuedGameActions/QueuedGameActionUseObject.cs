using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Managers;
using ACE.Network.GameEvent.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction.QueuedGameActions
{
    public class QueuedGameActionUseObject : QueuedGameAction
    {
        public QueuedGameActionUseObject(uint objectId)
        {
            ObjectId = objectId;
            StartTime = WorldManager.PortalYearTicks;
            EndTime = StartTime;
        }

        protected override void Handle(Player player)
        {
            var obj = LandManager.OpenWorld.ReadOnlyClone(new ObjectGuid(ObjectId));

            if ((obj.DescriptionFlags & ObjectDescriptionFlag.LifeStone) != 0)
                (obj as Lifestone).OnUse(player);
            else if ((obj.DescriptionFlags & ObjectDescriptionFlag.Portal) != 0)
                // TODO: When Physics collisions are implemented, this logic should be switched there, as normal portals are not onUse.
                (obj as Portal).OnCollide(player);
            else if ((obj.DescriptionFlags & ObjectDescriptionFlag.Door) != 0)
                (obj as Door).OnUse(player);

            else if ((obj.DescriptionFlags & ObjectDescriptionFlag.Vendor) != 0)
                (obj as Vendor).OnUse(player);

            // switch (obj.Type)
            // {
            //    case Enum.ObjectType.Portal:
            //        {
            //            // TODO: When Physics collisions are implemented, this logic should be switched there, as normal portals are not onUse.
            //
            //            (obj as Portal).OnCollide(player);
            //
            //            break;
            //        }
            //    case Enum.ObjectType.LifeStone:
            //        {
            //            (obj as Lifestone).OnUse(player);
            //            break;
            //        }
            // }
        }
    }
}