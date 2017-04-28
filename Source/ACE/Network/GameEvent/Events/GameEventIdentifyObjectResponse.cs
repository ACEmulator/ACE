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
            flags = IdentifyResponseFlags.PropertyString;
            Writer.Write((uint)flags); // Flags
            Writer.Write(1u); // Success bool

            // Write ints
            // Writer.Write(1u + (8 << 16)); // One int
            // Writer.Write(19u); // VALUE (Pyreals)
            // Writer.Write(0); // Zero.

            // Write Bool
            // Writer.Write(1u + (1 << 16)); // One bool
            // Writer.Write(2u); // IS_OPEN
            // Writer.Write(0); // False.

            // Write Strings
            Writer.Write(1u + (1 << 16)); // One String
            Writer.Write(0x0E); // Use String
            Writer.WriteString16L("This item has a guid of " + objectID.ToString() + " (0x" + objectID.ToString("X") + ")");
        }
    }
}