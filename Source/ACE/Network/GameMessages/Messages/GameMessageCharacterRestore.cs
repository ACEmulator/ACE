using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageCharacterRestore : GameMessage
    {
        public GameMessageCharacterRestore(ObjectGuid guid, string name, uint secondsDisabled) : base(GameMessageOpcode.CharacterRestoreResponse, 0x9)
        {
            Writer.Write(1u /* Verification OK flag */);
            Writer.WriteGuid(guid);
            Writer.WriteString16L(name);
            Writer.Write(secondsDisabled /* secondsGreyedOut */);
        }
    }
}