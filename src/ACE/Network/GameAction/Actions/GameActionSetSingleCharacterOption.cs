using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Network.GameAction.Actions
{
    public static class GameActionSetSingleCharacterOption
    {
        [GameAction(GameActionType.SetSingleCharacterOption)]
        public static void Handle(ClientMessage message, Session session)
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
    }
}
