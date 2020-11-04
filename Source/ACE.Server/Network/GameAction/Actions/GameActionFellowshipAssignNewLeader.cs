namespace ACE.Server.Network.GameAction.Actions
{
    public static class GameActionFellowshipAssignNewLeader
    {
        [GameAction(GameActionType.FellowshipAssignNewLeader)]
        public static void Handle(ClientMessage message, Session session)
        {
            uint newLeaderID = message.Payload.ReadUInt32();

            session.Player.FellowshipNewLeader(newLeaderID);
        }
    }
}
