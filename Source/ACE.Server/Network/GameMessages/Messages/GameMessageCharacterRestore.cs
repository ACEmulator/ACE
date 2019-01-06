
namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageCharacterRestore : GameMessage
    {
        public GameMessageCharacterRestore(uint objectGuid, string name, uint secondsDisabled)
            : base(GameMessageOpcode.CharacterRestoreResponse, GameMessageGroup.UIQueue)
        {
            Writer.Write(1u /* Verification OK flag */);
            Writer.Write(objectGuid);
            Writer.WriteString16L(name);
            Writer.Write(secondsDisabled /* secondsGreyedOut */);
        }
    }
}
