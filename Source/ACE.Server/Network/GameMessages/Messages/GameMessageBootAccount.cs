namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageBootAccount : GameMessage
    {
        public GameMessageBootAccount(Session session)
            : base(GameMessageOpcode.AccountBoot, GameMessageGroup.UIQueue)
        {
        }
    }
}
