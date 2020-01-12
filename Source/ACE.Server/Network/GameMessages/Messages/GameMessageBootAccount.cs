namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageBootAccount : GameMessage
    {
        public GameMessageBootAccount(Session session, string reason = null)
            : base(GameMessageOpcode.AccountBoot, GameMessageGroup.UIQueue)
        {
            if (reason != null)
            {
                Writer.WriteString16L($" - {reason}");
            }
        }
    }
}
