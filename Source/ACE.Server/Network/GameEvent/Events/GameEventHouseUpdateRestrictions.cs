using ACE.Server.Network.Sequence;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventHouseUpdateRestrictions : GameEventMessage
    {
        public GameEventHouseUpdateRestrictions(Session session, WorldObject obj, RestrictionDB restrictions)
            : base(GameEventType.HouseUpdateRestrictions, GameMessageGroup.UIQueue, session)
        {
            //Console.WriteLine("Sending 0x248 - House - UpdateRestrictions");

            Writer.Write(obj.Sequences.GetNextSequence(SequenceType.UpdateRestrictionDB));
            Writer.Write(obj.Guid.Full);  // The object restrictions are being updated for
            Writer.Write(restrictions);
        }
    }
}
