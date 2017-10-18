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
        /// <param name="version">Version of the contract.   So far I have only seen 0, but I have not done an exhaustive search.</param>
        /// <param name="contractId">Id of the contract.   This is the index into the contract table in the portal.dat file</param>
        /// <param name="contractStage">Where are we in the quest - progress.   Starts at 0 on initial add to quest tracker</param>
        /// <param name="timeWhenDone">This is used for timed quests - kill so many within an hour</param>
        /// <param name="timeWhenRepeats">When is my quest timer up so I can do the quest again.  This will be tracked in the greater quest system.</param>
        /// <param name="deleteContract">delete flag 0 is false, 1 is true - delete the contract</param>
        /// <param name="setAsDisplayContract">flag to display details of the quest and not just the list of contracts in the quest panel.</param>
        public GameEventSendClientContractTracker(Session session, uint version, uint contractId, uint contractStage, ulong timeWhenDone, ulong timeWhenRepeats, uint deleteContract, uint setAsDisplayContract)
                : base(GameEventType.SendClientContractTracker, GameMessageGroup.Group09, session)
        {
            Writer.Write(version);
            Writer.Write(contractId);
            Writer.Write(contractStage);
            Writer.Write(timeWhenDone);
            Writer.Write(timeWhenRepeats);
            Writer.Write(deleteContract);
            Writer.Write(setAsDisplayContract);
        }
    }
}
