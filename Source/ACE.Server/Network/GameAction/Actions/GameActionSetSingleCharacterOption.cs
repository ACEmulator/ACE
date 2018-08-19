using ACE.Entity.Enum;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionSetSingleCharacterOption
    {
        [GameAction(GameActionType.SetSingleCharacterOption)]
        public static void Handle(ClientMessage message, Session session)
        {
            var option = (CharacterOption)message.Payload.ReadUInt32();
            var optionValue = message.Payload.ReadUInt32() != 0;
            switch (option)
            {
                case CharacterOption.AppearOffline:
                    session.Player.AppearOffline(optionValue);
                    break;
                case CharacterOption.AutomaticallyAcceptFellowshipRequests:
                    session.Player.SetCharacterOption(CharacterOption.AutomaticallyAcceptFellowshipRequests, optionValue);
                    break;
                case CharacterOption.IgnoreFellowshipRequests:
                    session.Player.SetCharacterOption(CharacterOption.IgnoreFellowshipRequests, optionValue);
                    break;
                case CharacterOption.ShowYourCloak:
                    session.Player.SetCharacterOption(CharacterOption.ShowYourCloak, optionValue);
                    session.Player.EnqueueBroadcast(new GameMessageObjDescEvent(session.Player));
                    break;
                case CharacterOption.ShowYourHelmOrHeadGear:
                    session.Player.SetCharacterOption(CharacterOption.ShowYourHelmOrHeadGear, optionValue);
                    session.Player.EnqueueBroadcast(new GameMessageObjDescEvent(session.Player));
                    break;

                default:
                    session.Player.SetCharacterOption(option, optionValue);
                    break;
            }
        }
    }
}
