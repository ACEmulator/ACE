using ACE.Entity.Enum;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventWeenieErrorWithString : GameEventMessage
    {
        public GameEventWeenieErrorWithString(Session session, WeenieErrorWithString errorType, string message)
            : base(GameEventType.WeenieErrorWithString, GameMessageGroup.UIQueue, session, 52) // 52 is the max seen in retail pcaps
        {
            Writer.Write((uint)errorType);
            Writer.WriteString16L(message);
        }
    }
}
