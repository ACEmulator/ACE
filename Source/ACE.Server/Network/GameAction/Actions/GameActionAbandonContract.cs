
namespace ACE.Server.Network.GameAction.Actions
{
    /// <summary>
    /// This method handles the Game Action F7B1 - 0x0316 Abandon Contract.   This is sent to the server when the player
    /// selects a tracked quest from the quest panel and clicks on the abandon button.   We get the id of the contract we want to delete.
    /// We will respond with a F7B0 - 0x0315 message SendClientContractTracker passing the deleteContract flag set to true.   Og II
    /// </summary>
    public static class GameActionAbandonContract
    {
        [GameAction(GameActionType.AbandonContract)]
        public static void Handle(ClientMessage message, Session session)
        {
            // Read in the applicable data.
            uint contractId = message.Payload.ReadUInt32();

            session.Player.HandleActionAbandonContract(contractId);
        }
    }
}
