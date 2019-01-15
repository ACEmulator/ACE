using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventInventoryServerSaveFailed : GameEventMessage
    {
        public GameEventInventoryServerSaveFailed(Session session, uint itemGuid, WeenieError errorType = WeenieError.None)
            : base(GameEventType.InventoryServerSaveFailed, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(itemGuid);
            Writer.Write((uint)errorType);
        }
    }
}
