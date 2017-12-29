using System.Threading.Tasks;

using ACE.Common.Extensions;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionSoulEmote
    {
        [GameAction(GameActionType.SoulEmote)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            var emote = message.Payload.ReadString16L();
            session.Player.DoSoulEmote(emote);
        }
        #pragma warning restore 1998
    }
}
