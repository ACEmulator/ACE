namespace ACE.Network.GameEvent.Events
{
    public class GameEventSendClientContractTracker : GameEventMessage
    {
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