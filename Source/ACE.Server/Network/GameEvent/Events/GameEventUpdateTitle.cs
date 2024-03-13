using System;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventUpdateTitle : GameEventMessage
    {
        public GameEventUpdateTitle(Session session, uint title, bool setAsDisplayTitle = false)
            : base(GameEventType.UpdateTitle, GameMessageGroup.UIQueue, session, 12)
        {
            Writer.Write(title);
            Writer.Write(Convert.ToUInt32(setAsDisplayTitle));
        }
    }
}
