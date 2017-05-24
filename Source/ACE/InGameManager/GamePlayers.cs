using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.GameMessages.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity.Events;

namespace ACE.InGameManager
{
    public class GamePlayers
    {
        protected GameMediator gameMediator;

        public GamePlayers(GameMediator mediator)
        {
            this.gameMediator = mediator;
        }

        private readonly object objectCacheLocker = new object();
        private Dictionary<ObjectGuid, Player> worldplayers = new Dictionary<ObjectGuid, Player>();

        public void PlayerEnterWorld(Session session)
        {

            this.gameMediator.Register(session.Player);

            session.Network.EnqueueSend(new GameMessageSystemChat("Welcome to Asheron's Call", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat("  powered by ACEmulator  ", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat("", ChatMessageType.Broadcast));
            session.Network.EnqueueSend(new GameMessageSystemChat("For more information on commands supported by this server, type @acehelp", ChatMessageType.Broadcast));

        }

        public void Register(WorldObject wo)
        {
            lock (objectCacheLocker)
            {
                if (!worldplayers.ContainsKey(wo.Guid))
                    worldplayers.Add(wo.Guid, wo as Player);
            }
        }

        public void PlayerExitWorld(Session session)
        {
            lock (objectCacheLocker)
            {
                if (worldplayers.ContainsKey(session.Player.Guid))
                {
                    worldplayers.Remove(session.Player.Guid);
                    this.gameMediator.UnRegister(session.Player);
                }
            }
        }

        public void Tick()
        {

        }

        public void Broadcast(BroadcastEventArgs args)
        {
        }
    }
}
