using System.Collections.Generic;

using ACE.Entity.Enum;
using ACE.Server.Managers;

namespace ACE.Server.Network.GameEvent.Events
{
    public class GameEventChannelList : GameEventMessage
    {
        public GameEventChannelList(Session session, Channel chatChannel) : base(GameEventType.ChannelList, GameMessageGroup.UIQueue, session)
        {
            // TODO: This should send back to the client a correct count followed by name strings of the channel requested.
            //      Obviously this would be based on who was subscribed to the channel at the time

            // Writer.Write(1u);
            // Writer.WriteString16L("+Sentinel Nostromo");

            // For now, since everyone is subscribed and unable to alter, let's just list every character connected.
            uint numClientsConnected = 0;
            List<string> playerNames = new List<string>();
            foreach (var client in PlayerManager.GetAllOnline())
            {
                numClientsConnected++;
                playerNames.Add(client.Name);
            }

            Writer.Write(numClientsConnected);
            foreach (var name in playerNames.ToArray())
                Writer.WriteString16L(name);
        }
    }
}
