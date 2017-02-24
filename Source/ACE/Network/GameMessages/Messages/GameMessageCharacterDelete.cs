using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageCharacterDelete : GameMessage
    {
        public GameMessageCharacterDelete() 
            : base(GameMessageOpcode.CharacterDelete, 0x9)
        {
            
        }
    }
}