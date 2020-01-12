using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects.Managers;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventSendClientContractTrackerTable : GameEventMessage
    {
        public GameEventSendClientContractTrackerTable(Session session) : base(GameEventType.SendClientContractTrackerTable, GameMessageGroup.UIQueue, session)
        {
            Writer.Write(session.Player.ContractManager);
        }
    }
}
