using System;
using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        private int initialAge;
        private DateTime initialAgeTime;

        private DateTime lastSendAgeIntUpdateTime;

        /// <summary>
        /// The amount of time in seconds to send player age updates
        /// </summary>
        private double ageUpdateInterval = 7.0f;

        /// <summary>
        /// Called every ~5 seconds for Players
        /// </summary>
        public override void HeartBeat()
        {
            NotifyLandblocks();

            ManaConsumersTick();

            HandleTargetVitals();

            LifestoneProtectionTick();

            PK_DeathTick();

            AgeTick();

            // Check if we're due for our periodic SavePlayer
            if (LastRequestedDatabaseSave == DateTime.MinValue)
                LastRequestedDatabaseSave = DateTime.UtcNow;

            if (LastRequestedDatabaseSave + PlayerSaveInterval <= DateTime.UtcNow)
                SavePlayerToDatabase();

            base.HeartBeat();
        }

        public void AgeTick()
        {
            // TODO: separate action w/ ageUpdateInterval delay
            if (initialAgeTime == DateTime.MinValue)
            {
                initialAge = Age ?? 1;
                initialAgeTime = DateTime.UtcNow;
            }

            Age = initialAge + (int)(DateTime.UtcNow - initialAgeTime).TotalSeconds;

            if (lastSendAgeIntUpdateTime == DateTime.MinValue)
                lastSendAgeIntUpdateTime = DateTime.UtcNow;

            if (lastSendAgeIntUpdateTime.AddSeconds(ageUpdateInterval) <= DateTime.UtcNow)
            {
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.Age, Age ?? 1));
                lastSendAgeIntUpdateTime = DateTime.UtcNow;
            }
        }
    }
}
