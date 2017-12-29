using System.Threading.Tasks;

using ACE.Common.Extensions;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionEmote
    {
        [GameAction(GameActionType.Emote)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            var emote = message.Payload.ReadString16L();
            session.Player.DoEmote(emote);
        }
        #pragma warning restore 1998
    }
}
