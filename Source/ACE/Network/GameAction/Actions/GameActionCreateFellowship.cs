using System.Threading.Tasks;

using ACE.Network.GameMessages.Messages;
using ACE.Common.Extensions;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionCreateFellowship
    {
        [GameAction(GameActionType.CreateFellowship)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            // Should create new Fellowship with 1 member
            // todo: option for sharing XP

            var fellowshipName = message.Payload.ReadString16L();
            bool shareXp = message.Payload.ReadUInt32() > 0;

            session.Player.CreateFellowship(fellowshipName, shareXp);
            session.Network.EnqueueSend(new GameMessageFellowshipFullUpdate(session));            
        }
        #pragma warning restore 1998
    }
}
