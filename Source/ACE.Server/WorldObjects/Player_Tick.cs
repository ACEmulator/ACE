using System;

using ACE.Entity.Enum.Properties;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        private int initialAge;
        private DateTime initialAgeTime;

        private DateTime lastSendAgeIntUpdateTime = DateTime.UtcNow;
        private DateTime lastAutoSaveTime = DateTime.UtcNow;

        public override void Tick(double lastTickDuration, long currentTimeTick)
        {
            if (initialAgeTime == DateTime.MinValue)
            {
                initialAge = Age ?? 1;
                initialAgeTime = DateTime.UtcNow;
            }

            Age = initialAge + (int)(DateTime.UtcNow - initialAgeTime).TotalSeconds;

            if (lastSendAgeIntUpdateTime.AddSeconds(7) <= DateTime.UtcNow)
            {
                Session.Network.EnqueueSend(new GameMessagePrivateUpdatePropertyInt(this, PropertyInt.Age, Age ?? 1));
                lastSendAgeIntUpdateTime = DateTime.UtcNow;
            }

            base.Tick(lastTickDuration, currentTimeTick);
        }

        /// <summary>
        /// Called every ~5 seconds for Players
        /// </summary>
        public override void HeartBeat()
        {
            NotifyLandblocks();

            ManaConsumersTick();

            ItemEnchantmentTick();

            // First, we check if the player hasn't been saved in the last 5 minutes
            if (LastRequestedDatabaseSave + PlayerSaveInterval <= DateTime.UtcNow)
            {
                // Secondly, we make sure this session hasn't requested a save in the last 5 minutes.
                // We do this because EnqueueSaveChain will queue an ActionChain that may not execute immediately. This prevents refiring while a save is pending.
                if (lastAutoSaveTime + PlayerSaveInterval <= DateTime.UtcNow)
                {
                    lastAutoSaveTime = DateTime.UtcNow;
                    EnqueueSaveChain();
                }
            }

            base.HeartBeat();
        }
    }
}
