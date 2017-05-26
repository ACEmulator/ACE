using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ACE.Entity;
using ACE.Entity.Events;
using ACE.Network;
using ACE.Network.GameAction;

namespace ACE.InGameManager
{
    /// <summary>
    /// The 'ConcreteMediator' class
    /// </summary>
    public class GameConcreteMediator : GameMediator
    {
        // man in the middle class.. I see evrything that happens..
        private GamePlayers gamePlayers;
        private GameWorld gameworld;

        // pending broadcast messages to the world
        private static readonly object pendingbroadcastCacheLocker = new object();
        private static Queue<QueuedGameAction> pendingbroadcast = new Queue<QueuedGameAction>();

        public void RegisterPlayers(GamePlayers players)
        {
            gamePlayers = players;
        }

        public void RegisterGameWorld(GameWorld world)
        {
            gameworld = world;
        }

        public override void Broadcast(BroadcastEventArgs args)
        {
            // args = PhysicsAuthrity(args);
            gamePlayers.Broadcast(args);
            gameworld.Broadcast(args);
        }

        public override void PlayerEnterWorld(Session session)
        {
            gameworld.PlayerEnterWorld(session);
            gamePlayers.PlayerEnterWorld(session);
        }

        public override void PlayerExitWorld(Session session)
        {
            gameworld.PlayerExitWorld(session);
            gamePlayers.PlayerExitWorld(session);
        }

        public override void Register(WorldObject wo)
        {
            if (wo.Guid.IsPlayer())
                gamePlayers.Register(wo);
            gameworld.Register(wo);
        }

        public override void UnRegister(WorldObject wo)
        {
            if (wo.Guid.IsPlayer())
                gamePlayers.UnRegister(wo);
            gameworld.UnRegister(wo);
        }
    }
}
