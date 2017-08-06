using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameEvent.Events
{
    using System.Diagnostics;

    public class GameEventViewContents : GameEventMessage
    {
        public GameEventViewContents(Session session, AceObject container)
            : base(GameEventType.ViewContents, GameMessageGroup.Group09, session)
        {
            Writer.Write(container.AceObjectId);

            Writer.Write((uint)container.Inventory.Count);
            foreach (AceObject inv in container.Inventory.Values)
            {
                Writer.Write(inv.AceObjectId);
                Debug.Assert(inv.WeenieType != null, "container.WeenieType != null");
                switch ((WeenieType)inv.WeenieType)
                {
                    case WeenieType.Container:
                        Writer.Write((uint)ContainerType.Container);
                        break;
                    case WeenieType.Foci:
                        Writer.Write((uint)ContainerType.Foci);
                        break;
                    default:
                        Writer.Write((uint)ContainerType.NonContainer);
                        break;
                }
            }
        }
    }
}
