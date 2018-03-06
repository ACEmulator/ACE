
using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventViewContents : GameEventMessage
    {
        public GameEventViewContents(Session session, Container container)
            : base(GameEventType.ViewContents, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(container.Guid.Full);

            Writer.Write((uint)container.Inventory.Count);
            foreach (var inv in container.Inventory.Values)
            {
                Writer.Write(inv.Guid.Full);

                if (inv.WeenieType == WeenieType.Container)
                    Writer.Write((uint)ContainerType.Container);
                else
                    Writer.Write((uint)ContainerType.NonContainer);
            }
        }
    }
}
