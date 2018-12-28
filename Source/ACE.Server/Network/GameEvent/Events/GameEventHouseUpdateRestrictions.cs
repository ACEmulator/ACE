using System;
using ACE.Entity;
using ACE.Server.Network.Structure;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventHouseUpdateRestrictions : GameEventMessage
    {
        public GameEventHouseUpdateRestrictions(Session session, ObjectGuid sender, RestrictionDB restrictions, byte houseSequence)
            : base(GameEventType.HouseUpdateRestrictions, GameMessageGroup.UIQueue, session)
        {
            //Console.WriteLine("Sending 0x248 - House - UpdateRestrictions");

            Writer.Write(houseSequence);
            Writer.Write(sender.Full);  // The object restrictions are being updated for
            Writer.Write(restrictions);
        }
    }
}
