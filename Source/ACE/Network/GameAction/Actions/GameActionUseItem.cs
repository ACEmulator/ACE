using ACE.Common.Extensions;
using ACE.Database;
using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionUseItem
    {
        [GameAction(GameActionType.Use)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint fullId = message.Payload.ReadUInt32();
            session.Player.HandleActionUse(new ObjectGuid(fullId));
        }
    }
}
