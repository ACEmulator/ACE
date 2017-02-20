using ACE.Network.Enum;

namespace ACE.Network.GameAction.Actions
{
    [GameAction(GameActionOpcode.SetSingleCharacterOption)]
    public class GameActionSetSingleCharacterOption : GameActionPacket
    {
        private SingleCharacterOption option;
        private bool optionValue;

        public GameActionSetSingleCharacterOption(Session session, ClientPacketFragment fragment) : base(session, fragment) { }

        public override void Read()
        {
            option = (SingleCharacterOption)Fragment.Payload.ReadUInt32();
            optionValue = Fragment.Payload.ReadUInt32() == 0 ? false : true;
        }

        public override void Handle()
        {
            switch(option)
            {
                case SingleCharacterOption.AppearOffline:
                    Session.Player.AppearOffline(optionValue);
                    break;
            }            
        }
    }
}