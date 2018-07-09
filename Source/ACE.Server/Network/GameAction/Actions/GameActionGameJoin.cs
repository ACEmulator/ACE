using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionGameJoin
    {
        [GameAction(GameActionType.Join)]
        public static void Handle(ClientMessage message, Session session)
        {
            var gameId = message.Payload.ReadGuid(); // object id of gameboard
            var whichTeam = message.Payload.ReadUInt32(); // expecting 0xFFFFFFFF here

            Game wo = session.Player.CurrentLandblock?.GetObject(gameId) as Game;
            if (wo != null)
            {
                wo.ActOnJoin(session.Player.Guid);
            }
        }
    }
}
