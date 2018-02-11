using System.Diagnostics;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventViewContents : GameEventMessage
    {
        public GameEventViewContents(Session session, AceObject container)
            : base(GameEventType.ViewContents, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(container.AceObjectId);

            Writer.Write((uint)container.Inventory.Count);
            foreach (AceObject inv in container.Inventory.Values)
            {
                Writer.Write(inv.AceObjectId);
                Debug.Assert(inv.WeenieType != null, "container.WeenieType != null");
                if ((WeenieType)inv.WeenieType == WeenieType.Container)
                    Writer.Write((uint)ContainerType.Container);
                else
                    Writer.Write((uint)ContainerType.NonContainer);
            }
        }
    }
}
