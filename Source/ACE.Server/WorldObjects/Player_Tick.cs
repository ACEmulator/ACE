using System;

using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        private readonly ActionQueue actionQueue = new ActionQueue();

        private int initialAge;
        private DateTime initialAgeTime;

        private const double ageUpdateInterval = 7;
        private double nextAgeUpdateTime;

        public void Player_Tick(double currentUnixTime)
        {
            actionQueue.RunActions();

            if (nextAgeUpdateTime <= currentUnixTime)
            {
                nextAgeUpdateTime = currentUnixTime + ageUpdateInterval;

                if (initialAgeTime == DateTime.MinValue)
                {
                    initialAge = Age ?? 1;
                    initialAgeTime = DateTime.UtcNow;
                }

                Age = initialAge + (int)(DateTime.UtcNow - initialAgeTime).TotalSeconds;

                // In retail, this is sent every 7 seconds. If you adjust ageUpdateInterval from 7, you'll need to re-add logic to send this every 7s (if you want to match retail)
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.Age, Age ?? 1));
            }
        }

        /// <summary>
        /// Called every ~5 seconds for Players
        /// </summary>
        public override void Heartbeat(double currentUnixTime)
        {
            NotifyLandblocks();

            ManaConsumersTick();

            HandleTargetVitals();

            LifestoneProtectionTick();

            PK_DeathTick();

            // Check if we're due for our periodic SavePlayer
            if (LastRequestedDatabaseSave == DateTime.MinValue)
                LastRequestedDatabaseSave = DateTime.UtcNow;

            if (LastRequestedDatabaseSave + PlayerSaveInterval <= DateTime.UtcNow)
                SavePlayerToDatabase();

            base.Heartbeat(currentUnixTime);
        }

        /// <summary>
        /// Prepare new action to run on this player
        /// </summary>
        public override void EnqueueAction(IAction action)
        {
            actionQueue.EnqueueAction(action);
        }
    }
}
