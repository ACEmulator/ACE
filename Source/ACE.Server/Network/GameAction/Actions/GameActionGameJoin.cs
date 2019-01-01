using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionGameJoin
    {
        [GameAction(GameActionType.Join)]
        public static void Handle(ClientMessage message, Session session)
        {
            var gameGuid = message.Payload.ReadUInt32(); // object id of gameboard
            var whichTeam = message.Payload.ReadUInt32(); // expecting 0xFFFFFFFF here

            if (session.Player.CurrentLandblock?.GetObject(gameGuid) is Game game)
                game.ActOnJoin(session.Player.Guid);
        }
    }
}
