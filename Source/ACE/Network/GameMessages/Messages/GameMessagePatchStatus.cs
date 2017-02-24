using ACE.Entity.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessagePatchStatus : GameMessage
    {
        public GameMessagePatchStatus() 
            : base(GameMessageOpcode.PatchStatus, 0x5)
        {
            
        }
    }
}