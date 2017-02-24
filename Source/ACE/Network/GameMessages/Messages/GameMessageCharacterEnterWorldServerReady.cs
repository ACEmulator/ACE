using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageCharacterEnterWorldServerReady : GameMessage
    {
        public GameMessageCharacterEnterWorldServerReady() 
            : base(GameMessageOpcode.CharacterEnterWorldServerReady, 0x9)
        {
            
        }
    }
}