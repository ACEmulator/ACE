using System.Threading.Tasks;

using ACE.Entity.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionSetSingleCharacterOption
    {
        [GameAction(GameActionType.SetSingleCharacterOption)]
        #pragma warning disable 1998
        public static async Task Handle(ClientMessage message, Session session)
        {
            var option = (CharacterOption)message.Payload.ReadUInt32();
            var optionValue = message.Payload.ReadUInt32() == 0 ? false : true;
            switch (option)
            {
                case CharacterOption.AppearOffline:
                    session.Player.AppearOffline(optionValue);
                    break;

                default:
                    session.Player.SetCharacterOption(option, optionValue);
                    break;
            }
        }
        #pragma warning restore 1998
    }
}
