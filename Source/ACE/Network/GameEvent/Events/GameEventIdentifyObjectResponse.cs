using ACE.Network.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventIdentifyObjectResponse : GameEventMessage
    {
        public GameEventIdentifyObjectResponse(Session session, uint objectID)
            : base(GameEventType.IdentifyObjectResponse, GameMessageGroup.Group09, session)
        {
            // TODO

            // Writer.Write(objectID);
            // Writer.Write(0u);
            // Writer.Write(1u);
            Writer.Write(objectID);

            IdentifyResponseFlags flags = IdentifyResponseFlags.None;
            flags = IdentifyResponseFlags.PropertyString;
            Writer.Write((uint)flags); // Flags
            Writer.Write(1u); // Success bool
            // Writer.Write(1u); // Flags

            // Write Int32
            // Writer.Write(1u); // One Item
            // Writer.Write(0x13); // Value
            // Writer.Write(0x0); // No Value

            // Write Strings
            Writer.Write(1u); // One String
            Writer.Write(0x0E); // Use String
            Writer.WriteString16L("Use this item to set your resurrection point.");
        }
    }
}