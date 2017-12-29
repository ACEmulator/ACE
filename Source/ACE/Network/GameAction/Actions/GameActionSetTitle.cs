using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionSetTitle
    {
        [GameAction(GameActionType.TitleSet)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            var title = message.Payload.ReadUInt32();
            session.Player.SetTitle(title);
        }
        #pragma warning restore 1998
    }
}
