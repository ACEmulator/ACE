using ACE.Network.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventIdentifyObjectResponse : GameEventMessage
    {
        public GameEventIdentifyObjectResponse(Session session, uint objectID)
            : base(GameEventType.IdentifyObjectResponse, GameMessageGroup.Group09, session)
        {
            // TODO - Send Actual Properties
            Writer.Write(objectID);

            IdentifyResponseFlags flags = IdentifyResponseFlags.None;
            flags = IdentifyResponseFlags.StringStatsTable;
            Writer.Write((uint)flags); // Flags
            Writer.Write(1u); // Success bool

            // Write a simple debug strings. Thanks to Pea for this idea.
            Writer.Write(1u + (1 << 16)); // One String, + table size
            Writer.Write(16u); // Long Description String
            Writer.WriteString16L("This item has a guid of " + objectID.ToString() + " (0x" + objectID.ToString("X") + ")");
        }
    }
}