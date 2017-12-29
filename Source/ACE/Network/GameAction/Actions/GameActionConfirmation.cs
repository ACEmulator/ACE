using System.Threading.Tasks;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionConfirmation
    {
        [GameAction(GameActionType.ConfirmationResponse)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            int confirmType = message.Payload.ReadInt32();
            uint context = message.Payload.ReadUInt32();
            int response = message.Payload.ReadInt32();

            // TODO: do something
        }
        #pragma warning restore 1998
    }
}
