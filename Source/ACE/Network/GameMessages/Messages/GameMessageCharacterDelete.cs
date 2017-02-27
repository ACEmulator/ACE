using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageCharacterDelete : GameMessage
    {
        public GameMessageCharacterDelete() 
            : base(GameMessageOpcode.CharacterDelete, GameMessageGroup.Group09)
        {
            
        }
    }
}