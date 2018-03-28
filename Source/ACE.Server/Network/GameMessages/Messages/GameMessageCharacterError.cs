using ACE.Server.Network.Enum;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageCharacterError : GameMessage
    {
        public GameMessageCharacterError(CharacterError error)
            : base(GameMessageOpcode.CharacterError, GameMessageGroup.UIQueue)
        {
            Writer.Write((uint)error);
        }
    }
}
