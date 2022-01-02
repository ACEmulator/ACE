using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Database;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.TownControl;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        /// <summary>
        /// Called every ~5 seconds for Creatures
        /// </summary>
        public override void Heartbeat(double currentUnixTime)
        {
            var expireItems = new List<WorldObject>();

            // added where clause
            foreach (var wo in EquippedObjects.Values.Where(i => i.EnchantmentManager.HasEnchantments || i.Lifespan.HasValue))
            {
                // FIXME: wo.NextHeartbeatTime is double.MaxValue here
                //if (wo.NextHeartbeatTime <= currentUnixTime)
                    //wo.Heartbeat(currentUnixTime);

                // just go by parent heartbeats, only for enchantments?
                // TODO: handle players dropping / picking up items
                wo.EnchantmentManager.HeartBeat(CachedHeartbeatInterval);

                if (wo.IsLifespanSpent)
                    expireItems.Add(wo);
            }

            VitalHeartBeat();

            EmoteManager.HeartBeat();

            DamageHistory.TryPrune();

            // delete items when RemainingLifespan <= 0
            foreach (var expireItem in expireItems)
            {
                expireItem.DeleteObject(this);

                if (this is Player player)
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Its lifespan finished, your {expireItem.Name} crumbles to dust.", ChatMessageType.Broadcast));
            }

            if(TownControlBosses.IsTownControlConflictBoss(this.WeenieClassId))
            {
                //Check if there is an active Town Control event for this boss
                var tcBoss = TownControlBosses.TownControlBossMap[this.WeenieClassId];
                var town = DatabaseManager.TownControl.GetTownById(tcBoss.TownID);
                var tcEvent = DatabaseManager.TownControl.GetLatestTownControlEventByTownId(town.TownId);

                if (!town.IsInConflict || tcEvent == null || !tcEvent.EventStartDateTime.HasValue || tcEvent.EventEndDateTime.HasValue)
                {
                    //TODO what if the town isn't in conflict?  destroy the boss?
                    return;
                }

                var tcEventDurationExpiredTime = tcEvent.EventStartDateTime.Value.AddSeconds(town.ConflictLength);

                //If the town control event duration is past and this creature is still alive
                //end the TC event with defenders winning
                //destroy the TC boss
                if(DateTime.UtcNow > tcEventDurationExpiredTime)
                {
                    var dmgHistory = new DamageHistoryInfo(this);
                    OnDeath(dmgHistory, DamageType.Bludgeon);
                    Die(dmgHistory, dmgHistory);
                }
            }

            base.Heartbeat(currentUnixTime);
        }
    }
}
