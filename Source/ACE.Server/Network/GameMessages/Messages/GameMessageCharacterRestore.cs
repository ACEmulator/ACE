using ACE.Entity;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageCharacterRestore : GameMessage
    {
        public GameMessageCharacterRestore(ObjectGuid guid, string name, uint secondsDisabled)
            : base(GameMessageOpcode.CharacterRestoreResponse, GameMessageGroup.UIQueue)
        {
            Writer.Write(1u /* Verification OK flag */);
            Writer.WriteGuid(guid);
            Writer.WriteString16L(name);
            Writer.Write(secondsDisabled /* secondsGreyedOut */);
        }
    }
}