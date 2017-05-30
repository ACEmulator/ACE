using ACE.Entity;
using ACE.Entity.Events;
using ACE.Network;
using ACE.Network.GameAction;

namespace ACE.InGameManager
{
    /// <summary>
    /// The 'Mediator' abstract class
    /// </summary>
    public abstract class GameMediator
    {
        public abstract void PlayerEnterWorld(Session session);
        public abstract void PlayerExitWorld(Session session);
        public abstract void Broadcast(BroadcastEventArgs args);
        public abstract void Register(WorldObject wo);
        public abstract void UnRegister(WorldObject wo);
        public abstract void CreateQueuedGameAction(QueuedGameAction action);
    }
}   