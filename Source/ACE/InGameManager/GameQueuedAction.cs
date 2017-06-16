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
using ACE.Network.GameAction;
using ACE.InGameManager.Enums;

namespace ACE.InGameManager
{
    public class GameQueuedAction
    {
        protected GameMediator gameMediator;

        public GameQueuedAction(GameMediator mediator)
        {
            this.gameMediator = mediator;
        }

        private readonly object objectCacheLocker = new object();
        private Queue<QueuedGameAction> quedgameaction = new Queue<QueuedGameAction>();

        public void CreateQueuedGameAction(QueuedGameAction action)
        {
            lock (objectCacheLocker)
            {
                quedgameaction.Enqueue(action);
            }
        }

        public void Tick()
        {
            lock (objectCacheLocker)
            {
                if (quedgameaction.Count > 0)
                {
                    QueuedGameAction gameaction = new QueuedGameAction();
                    gameaction = quedgameaction.Dequeue();

                    switch (gameaction.InGameType)
                    {
                        case InGameType.PlayerClass:
                            gameaction.Handler(this.gameMediator, gameaction.WorldObject as Player);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
