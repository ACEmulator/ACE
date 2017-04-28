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
            // Writer.Write(2u + (8 << 16)); // count + tableSize (0x0010)
            // Writer.Write(320u); // ITEM_XP_STYLE_INT
            // Writer.Write(2); // Zero.
            // Writer.Write(319u); // ITEM_MAX_LEVEL_INT.
            // Writer.Write(2); // Zero.

            // Write int64s/long
            // Writer.Write(1u + (8 << 16)); // count + tableSize (0x0008)
            // Writer.Write(4u); // ITEM_TOTAL_XP_INT64
            // Writer.Write((long)1639577393); // Zero.
            // Writer.Write(5u); // ITEM_BASE_XP_INT64
            // Writer.Write((long)1000000000); // Zero.

            // Write BIGints
            // Writer.Write(1u + (8 << 16)); // count + tableSize (0x0008)
            // Writer.Write(19u); // VALUE (Pyreals)
            // Writer.Write(0); // Zero.

            // Write Bool
            // Writer.Write(1u + (1 << 16)); // count + tableSize (0x0008)
            // Writer.Write(2u); // IS_OPEN
            // Writer.Write(0); // False.

            // Write Float
            // Writer.Write(1u + (1 << 16)); // count + tableSize (0x0008)
            // Writer.Write(2u); // IS_OPEN
            // Writer.Write(0); // False.

            // Write Strings
            Writer.Write(1u + (1 << 16)); // One String
            Writer.Write(0x0E); // Use String
            Writer.WriteString16L("This item has a guid of " + objectID.ToString() + " (0x" + objectID.ToString("X") + ")");
        }
    }
}