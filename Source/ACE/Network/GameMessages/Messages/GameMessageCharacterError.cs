
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageCharacterError : GameMessage
    {
        public GameMessageCharacterError(CharacterError error) : base(GameMessageOpcode.UpdatePosition, 0x9)
        {
            Writer.Write((uint)error);
        }
    }
}