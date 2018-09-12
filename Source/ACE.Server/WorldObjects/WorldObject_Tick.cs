using System.Collections.Generic;

using ACE.Common;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;

namespace ACE.Server.WorldObjects
{
    partial class WorldObject
    {
        private readonly ActionQueue actionQueue = new ActionQueue();

        private readonly int defaultHeartbeatInterval = 5;

        public virtual void Tick(double lastTickDuration)
        {
            actionQueue.RunActions();

            if (HeartbeatTimestamp == null || HeartbeatTimestamp + (HeartbeatInterval ?? defaultHeartbeatInterval) <= Time.GetUnixTime())
                HeartBeat();
        }

        /// <summary>
        /// Called every ~5 seconds for WorldObject base
        /// </summary>
        public virtual void HeartBeat()
        {
            Generator_HeartBeat();

            EmoteManager.HeartBeat();

            EnchantmentManager.HeartBeat();

            SetProperty(PropertyFloat.HeartbeatTimestamp, Time.GetUnixTime());
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
