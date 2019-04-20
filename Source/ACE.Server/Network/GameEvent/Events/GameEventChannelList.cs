using System.Collections.Generic;
using System.Linq;
using ACE.Entity.Enum;
using ACE.Server.Managers;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventChannelList : GameEventMessage
    {
        public GameEventChannelList(Session session, Channel chatChannel) : base(GameEventType.ChannelList, GameMessageGroup.UIQueue, session)
        {
            uint numClientsConnected = 0;
            List<string> playerNames = new List<string>();
            foreach (var client in PlayerManager.GetAllOnline().Where(p => (p.ChannelsActive ?? 0).HasFlag(chatChannel)))
            {
                numClientsConnected++;
                playerNames.Add(client.Name);
            }

            Writer.Write(numClientsConnected);
            foreach (var name in playerNames)
                Writer.WriteString16L(name);
        }
    }
}
