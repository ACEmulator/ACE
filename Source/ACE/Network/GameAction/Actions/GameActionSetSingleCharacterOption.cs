using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionType.SetSingleCharacterOption)]
    public class GameActionSetSingleCharacterOption : GameActionPacket
    {
        private CharacterOption option;
        private bool optionValue;

        public GameActionSetSingleCharacterOption(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            option = (CharacterOption)Fragment.Payload.ReadUInt32();
            optionValue = Fragment.Payload.ReadUInt32() == 0 ? false : true;
        }

        public override void Handle()
        {
            switch (option)
            {
                case CharacterOption.AppearOffline:
                    Session.Player.AppearOffline(optionValue);
                    break;

                default:
                    Session.Player.SetCharacterOption(option, optionValue);
                    break;
            }
        }
    }
}
