using ACE.Database.Models.Shard;
using ACE.Server.Entity;
using ACE.Server.Network.Structure;
using System;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventSendClientContractTracker : GameEventMessage
    {
        public GameEventSendClientContractTracker(Session session, uint contractId) : base(GameEventType.SendClientContractTracker, GameMessageGroup.UIQueue, session)
        {
            var contractTracker = session.Player.ContractManager.GetContractTracker(contractId);

            if (contractTracker == null) return;

            Writer.Write(contractTracker);
            Writer.Write(Convert.ToUInt32(contractTracker.DeleteContract));
            Writer.Write(Convert.ToUInt32(contractTracker.SetAsDisplayContract));
        }

        public GameEventSendClientContractTracker(Session session, CharacterPropertiesContractRegistry contract) : base(GameEventType.SendClientContractTracker, GameMessageGroup.UIQueue, session)
        {
            var contractTracker = new ContractTracker(contract);

            Writer.Write(contractTracker);
            Writer.Write(Convert.ToUInt32(contractTracker.DeleteContract));
            Writer.Write(Convert.ToUInt32(contractTracker.SetAsDisplayContract));
        }
    }
}
