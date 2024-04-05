using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects.Managers;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventSendClientContractTrackerTable : GameEventMessage
    {
        public GameEventSendClientContractTrackerTable(Session session) : base(GameEventType.SendClientContractTrackerTable, GameMessageGroup.UIQueue, session, 512) // 436 is the average seen in retail pcaps, 3208 is the max seen in retail pcaps
        {
            Writer.Write(session.Player.ContractManager);
        }
    }
}
