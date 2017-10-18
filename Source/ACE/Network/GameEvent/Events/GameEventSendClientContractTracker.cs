using ACE.Entity;

namespace ACE.Network.GameEvent.Events
{
    public class GameEventSendClientContractTracker : GameEventMessage
    {
        /// <summary>
        /// This message is used to both add and remove quests in your quest pannel.   The first use case, the add is stright forward
        /// and is sent in response to an onUse of a contract from your inventory. F7B1 0036 - Inventory_UseEvent
        /// The second use case is the abandon quest.   This sends a F7B1 0316  Social_AbandonContract in this case you send back the contract id
        /// you got in the message from the client and pass back a 1 in the deleteContract parameter. Og II
        /// </summary>
        /// <param name="session">Our player session used for getting message recipient guid and the correct message sequence.</param>
        /// <param name="contactTracker">This class contains all of the information we need to send the client about the contract. </param>

        public GameEventSendClientContractTracker(Session session, ContractTracker contactTracker)
                : base(GameEventType.SendClientContractTracker, GameMessageGroup.Group09, session)
        {
            Writer.Write(contactTracker.Version);
            Writer.Write(contactTracker.ContractId);
            Writer.Write(contactTracker.Stage);
            Writer.Write(contactTracker.TimeWhenDone);
            Writer.Write(contactTracker.TimeWhenRepeats);
            Writer.Write(contactTracker.DeleteContract);
            Writer.Write(contactTracker.SetAsDisplayContract);
        }
    }
}
