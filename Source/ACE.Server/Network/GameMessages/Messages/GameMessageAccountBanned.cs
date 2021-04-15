using System;

namespace ACE.Server.Network.GameMessages.Messages
{
    public class GameMessageAccountBanned : GameMessage
    {
        /// <summary>
        /// Tells the client the player has been banned from the server.
        /// </summary>
        public GameMessageAccountBanned(DateTime BanExpiration, string Reason = null)
            : base(GameMessageOpcode.AccountBanned, GameMessageGroup.UIQueue)
        {
            var tsBanExpiration = BanExpiration - DateTime.UtcNow;

            Writer.Write((uint)tsBanExpiration.TotalSeconds);

            if (Reason != null)
                Writer.WriteString16L(Reason);
        }
    }
}
