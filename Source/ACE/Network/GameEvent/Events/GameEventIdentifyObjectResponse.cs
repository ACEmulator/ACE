using ACE.Entity;
using ACE.Network.Enum;

namespace ACE.Network.GameEvent.Events
{
    using System.Linq;

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
            // Started adding real data to this - I needed the information to debug combat stance
            // we really need to kill debug object.   Og II
            if (type.Name == "DebugObject" || type.Name == "Monster" || type.Name == "Generator")
            {
                flags |= IdentifyResponseFlags.StringStatsTable;
                if (obj.ExaminePropertiesInt.Count > 0)
                {
                    flags |= IdentifyResponseFlags.IntStatsTable;
                }
                Writer.Write((uint)flags); // Flags
                Writer.Write(1u); // Success bool

                if ((flags & IdentifyResponseFlags.IntStatsTable) != 0)
                {
                    var propertiesInt = obj.ExaminePropertiesInt.Where(x => x.PropertyId < 9000).ToList();
                    if (propertiesInt.Count != 0)
                    {
                        Writer.Write((ushort)propertiesInt.Count);
                        Writer.Write((ushort)16u);

                        foreach (var uintProperty in propertiesInt)
                        {
                            Writer.Write((uint)uintProperty.PropertyId);
                            Writer.Write(uintProperty.PropertyValue);
                        }
                    }
                }

                // Write a simple debug strings. Thanks to Pea for this idea.
                Writer.Write(1u + (1 << 16)); // One String, + table size
                Writer.Write(16u); // Long Description String
                string debugOutput = "baseAceObjectId: " + objectId + " (0x" + objectId.Full.ToString("X") + ")";
                debugOutput += "\n" + "weenieClassId: " + obj.WeenieClassid + " (0x" + obj.WeenieClassid.ToString("X") + ")";
                debugOutput += "\n" + "defaultCombatStyle: " + obj.DefaultCombatStyle;
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
