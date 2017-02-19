using ACE.Network.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.SetSingleCharacterOption)]
    public class GameActionSetSingleCharacterOption : GameActionPacket
    {
        private SingleCharacterOption option;
        private uint optionValue;

        public GameActionSetSingleCharacterOption(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            option = (SingleCharacterOption)Fragment.Payload.ReadUInt32();
            optionValue = Fragment.Payload.ReadUInt32();
        }

        public override void Handle()
        {
            switch(option)
            {
                case SingleCharacterOption.AppearOffline:
                    Session.Player.ChangeOnlineStatus(optionValue == 1 ? false : true);
                    break;
            }            
        }
    }
}
