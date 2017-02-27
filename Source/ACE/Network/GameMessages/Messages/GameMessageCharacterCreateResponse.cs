using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;

namespace ACE.Network.GameMessages.Messages
{
    public class GameMessageCharacterCreateResponse : GameMessage
    {
        public GameMessageCharacterCreateResponse(CharacterGenerationVerificationResponse response, ObjectGuid guid, string charName) 
            : base(GameMessageOpcode.CharacterCreateResponse, GameMessageGroup.Group09)
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