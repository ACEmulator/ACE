using ACE.Entity;
using ACE.Entity.Events;
using ACE.InGameManager;
using ACE.Managers;
using ACE.Network.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Network.GameAction.QueuedGameActions
{
    public class QueuedGameActionUpdateTicks : QueuedGameAction
    {
        private double tick;

        public QueuedGameActionUpdateTicks(WorldObject wo, uint objectId, double ticks)
        {
            InGameType = InGameManager.Enums.InGameType.PlayerClass;
            ObjectId = objectId;
            tick = ticks;
            WorldObject = wo;
        }

        protected override void Handle(GameMediator mediator, Player player)
        {
            BroadcastEventArgs args = BroadcastEventArgs.CreateTickEvent(player, tick);
            mediator.Broadcast(args);
        }
    }
}