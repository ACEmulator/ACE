using ACE.Entity;
using ACE.Network.Enum;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventIdentifyObjectResponse : GameEventMessage
    {
        public GameEventIdentifyObjectResponse(Session session, ObjectGuid objectId, WorldObject obj)
            : base(GameEventType.IdentifyObjectResponse, GameMessageGroup.Group09, session)
        {
            // TODO - Send Actual Properties
            Writer.Write(objectId.Full);

            // Check if item is WorldObject or a DebugObject
            System.Type type = obj.GetType();
            IdentifyResponseFlags flags = IdentifyResponseFlags.None;

            // We will send some debug properties if this is a true DebugObject
            if (type.Name == "DebugObject" || type.Name == "Monster" || type.Name == "Generator")
            {
                flags = IdentifyResponseFlags.StringStatsTable;
                Writer.Write((uint)flags); // Flags
                Writer.Write(1u); // Success bool

                // Write a simple debug strings. Thanks to Pea for this idea.
                Writer.Write(1u + (1 << 16)); // One String, + table size
                Writer.Write(16u); // Long Description String
                string debugOutput = "baseAceObjectId: " + objectId.Full.ToString() + " (0x" + objectId.Full.ToString("X") + ")";
                debugOutput += "\n" + "weenieClassId: " + obj.WeenieClassid.ToString() + " (0x" + obj.WeenieClassid.ToString("X") + ")";
                Writer.WriteString16L(debugOutput);
            }
            else
            {
                Writer.Write((uint)flags); // Flags
                Writer.Write(0u); // Success bool (fail)
            }
        }
    }
}
