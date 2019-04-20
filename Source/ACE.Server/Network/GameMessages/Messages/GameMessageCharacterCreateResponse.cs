using ACE.Entity;
using ACE.Server.Network.Enum;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageCharacterCreateResponse : GameMessage
    {
        public GameMessageCharacterCreateResponse(CharacterGenerationVerificationResponse response, ObjectGuid guid, string charName)
            : base(GameMessageOpcode.CharacterCreateResponse, GameMessageGroup.UIQueue)
        {
            Writer.Write((uint)response);

            if (response == CharacterGenerationVerificationResponse.Ok)
            {
                Writer.WriteGuid(guid);
                Writer.WriteString16L(charName);
                Writer.Write(0u);
            }
        }
    }
}
