using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventInventoryServerSaveFailed : GameEventMessage
    {
        public GameEventInventoryServerSaveFailed(Session session, uint itemGuid, WeenieError errorType = WeenieError.None)
            : base(GameEventType.InventoryServerSaveFailed, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(itemGuid);

            // client doesn't show this error mostly, and defaults to specific error messages,
            // depending on the item name + action
            // there are some exceptions, such as WeenieError.ActionCancelled being appended
            Writer.Write((uint)errorType);      
       }
    }
}
