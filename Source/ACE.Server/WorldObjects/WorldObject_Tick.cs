using System.Collections.Generic;

using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        private readonly ActionQueue actionQueue = new ActionQueue();

        private const int DefaultHeartbeatInterval = 5;

        private double? cachedHeartbeatTimestamp;
        private double cachedHeartbeatInterval;

        public virtual void Tick(double lastTickDuration, double currentUnixTime)
        {
            actionQueue.RunActions();

            if (cachedHeartbeatTimestamp == null)
            {
                cachedHeartbeatInterval = HeartbeatInterval ?? DefaultHeartbeatInterval;
                HeartBeat(currentUnixTime);
            }
            else if (cachedHeartbeatTimestamp + cachedHeartbeatInterval <= currentUnixTime)
                HeartBeat(currentUnixTime);
        }

        /// <summary>
        /// Called every ~5 seconds for WorldObject base
        /// </summary>
        public virtual void HeartBeat(double currentUnixTime)
        {
            Generator_HeartBeat();

            EmoteManager.HeartBeat();

            EnchantmentManager.HeartBeat();

            cachedHeartbeatTimestamp = currentUnixTime;
            SetProperty(PropertyFloat.HeartbeatTimestamp, currentUnixTime);
        }

        /// <summary>
        /// Runs all actions pending on this WorldObject
        /// </summary>
        void IActor.RunActions()
        {
            actionQueue.RunActions();
        }

        /// <summary>
        /// Prepare new action to run on this object
        /// </summary>
        public LinkedListNode<IAction> EnqueueAction(IAction action)
        {
            return actionQueue.EnqueueAction(action);
        }

        /// <summary>
        /// Satisfies action interface
        /// </summary>
        void IActor.DequeueAction(LinkedListNode<IAction> node)
        {
            actionQueue.DequeueAction(node);
        }
    }
}
