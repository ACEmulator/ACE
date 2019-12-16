namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageBootAccount : GameMessage
    {
        public GameMessageBootAccount(Session session, string reason = null)
            : base(GameMessageOpcode.AccountBoot, GameMessageGroup.UIQueue)
        {
            Writer.WriteString16L($"{(reason != null ? "\n" : "")}{reason}");
        }
    }
}
