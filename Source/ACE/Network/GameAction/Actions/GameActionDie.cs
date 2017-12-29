using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    // Death feels is less morbid then suicide as a human, used "Die" instead.
    public static class GameActionDie
    {
        [GameAction(GameActionType.Suicide)]
        public static async Task Handle(ClientMessage message, Session session)
        {
            await session.Player.HandleActionKill(session.Player.Guid);
        }
    }
}
