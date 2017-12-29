using System.Threading.Tasks;

using ACE.Entity;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionGameJoin
    {
        [GameAction(GameActionType.Join)]
        public static async Task Handle(ClientMessage message, Session session)
        {
            var gameId = message.Payload.ReadGuid(); // object id of gameboard
            var whichTeam = message.Payload.ReadUInt32(); // expecting 0xFFFFFFFF here

            Game wo = await session.Player.CurrentLandblock.GetObject(gameId) as Game;
            if (wo != null)
            {
                await wo.ActOnJoin(session.Player.Guid);
            }
        }
    }
}
