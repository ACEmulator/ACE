using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventInventoryServerSaveFailed : GameEventMessage
    {
        public GameEventInventoryServerSaveFailed(Session session, WeenieError errorType = WeenieError.None)
            : base(GameEventType.InventoryServerSaveFailed, GameMessageGroup.UIQueue, session)
        {
            Writer.Write((uint)errorType);
        }
    }
}
