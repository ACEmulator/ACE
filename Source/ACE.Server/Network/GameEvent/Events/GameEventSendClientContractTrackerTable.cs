using System.Collections.Generic;
using ACE.Server.Entity;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventSendClientContractTrackerTable : GameEventMessage
    {
        /// <summary>
        /// This message is used to send the list quests in your quest panel.
        /// This is sent as part of the login sequence - if we have any tracked quests, we will send them on to the client.
        /// The second use case is the abandon quest.   This sends a F7B1 0316  Social_AbandonContract in this case you send back the contract id
        /// you got in the message from the client and pass back a 1 in the deleteContract parameter. Og II
        /// </summary>
        /// <param name="session">Our player session used for getting message recipient guid and the correct message sequence.</param>
        /// <param name="contactTracker">This is a list of the contact class containing all of the information we need to send the client about the contracts. </param>

        public GameEventSendClientContractTrackerTable(Session session, List<ContractTracker> contactTracker)
                : base(GameEventType.SendClientContractTrackerTable, GameMessageGroup.Group09, session)
        {
            const ushort tableSize = 32;
            Writer.Write((ushort)contactTracker.Count);
            Writer.Write(tableSize);
            foreach (ContractTracker contract in contactTracker)
            {
                Writer.Write(contract.ContractId);
                Writer.Write(contract.Version);
                Writer.Write(contract.ContractId);
                Writer.Write(contract.Stage);
                Writer.Write(contract.TimeWhenDone);
                Writer.Write(contract.TimeWhenRepeats);
            }
       }
    }
}
