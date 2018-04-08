using ACE.Entity.Enum;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageHearDirectSpeech : GameMessage
    {
        public GameMessageHearDirectSpeech(WorldObject worldObject, string messageText, WorldObject targetObject, ChatMessageType chatMessageType)
            : base(GameMessageOpcode.HearDirectSpeech, GameMessageGroup.UIQueue)
        {
            Writer.WriteString16L(messageText);
            Writer.WriteString16L(worldObject.Name);
            Writer.WriteGuid(worldObject.Guid);
            Writer.WriteGuid(targetObject.Guid);
            Writer.Write((uint)chatMessageType);
            Writer.Write(0u); // secretFlags - doesn't seem to be used by the client
        }
    }
}
