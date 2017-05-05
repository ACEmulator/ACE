namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageCharacterLogOff : GameMessage
    {
        public GameMessageCharacterLogOff() : base(GameMessageOpcode.CharacterLogOff, GameMessageGroup.Group09)
        {
        }
    }
}