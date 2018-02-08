using ACE.Server.Network.Enum;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageCharacterError : GameMessage
    {
        public GameMessageCharacterError(CharacterError error)
            : base(GameMessageOpcode.CharacterError, GameMessageGroup.Group09)
        {
            Writer.Write((uint)error);
        }
    }
}