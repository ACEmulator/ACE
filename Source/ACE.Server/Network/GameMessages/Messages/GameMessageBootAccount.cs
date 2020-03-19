namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageBootAccount : GameMessage
    {
        /// <summary>
        /// Tells the client the player has been booted from the server.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="reason">message to tell player why they were booted, typically you will want to start with a space because client does not automatically add one<para>If null client will see " for Code of Conduct Violations"</para></param>
        public GameMessageBootAccount(Session session, string reason = null)
            : base(GameMessageOpcode.AccountBoot, GameMessageGroup.UIQueue)
        {
            if (reason != null)
                Writer.WriteString16L(reason);
        }
    }
}
