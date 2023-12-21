using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Database;
using ACE.Database.Models.TownControl;
using ACE.Entity.Enum;
using ACE.Server.Entity;
using ACE.Server.Entity.TownControl;
using ACE.Server.Entity.WorldBoss;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Handlers;

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

            if (this.IsTownControlBoss)
            {
                try
                {
                    var tcBoss = TownControlBosses.TownControlBossMap[this.WeenieClassId];
                    var town = DatabaseManager.TownControl.GetTownById(tcBoss.TownID);

                    if (this.IsTownControlConflictBoss)
                    {
                        //Check if there is an active Town Control event for this boss                        
                        var tcEvent = DatabaseManager.TownControl.GetLatestTownControlEventByTownId(town.TownId);

                        if (!town.IsInConflict || tcEvent == null || !tcEvent.EventStartDateTime.HasValue || tcEvent.EventEndDateTime.HasValue)
                        {
                            //PlayerManager.BroadcastToAll(new GameMessageSystemChat("DEBUG - conflict boss exists but there is no active conflict, destroying the boss", ChatMessageType.Broadcast));
                            log.DebugFormat("Conflict boss with WeenieID = {0} exists but there is no active conflict, destroying the boss", this.WeenieClassId);
                            var dmgHistory = new DamageHistoryInfo(this);
                            OnDeath(dmgHistory, DamageType.Bludgeon);
                            Die(dmgHistory, dmgHistory);

                            if (PropertyManager.GetBool("town_control_enable_webhook_debug").Item)
                            {
                                try
                                {
                                    var webhookUrl = PropertyManager.GetString("town_control_globals_webhook").Item;
                                    if (!string.IsNullOrEmpty(webhookUrl))
                                    {
                                        _ = TurbineChatHandler.SendWebhookedChat("God of PK", $"DEBUG: {town.TownName} conflict boss exists but there is no active conflict, destroying the boss", webhookUrl, "General");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.ErrorFormat("Failed sending TownControl global message to webhook. Ex:{0}", ex);
                                }
                            }
                        }
                        else
                        {
                            var tcEventDurationExpiredTime = tcEvent.EventStartDateTime.Value.AddSeconds(town.ConflictLength);

                            //If the town control event duration is past and this creature is still alive
                            //end the TC event with defenders winning
                            //destroy the TC boss
                            if (DateTime.UtcNow > tcEventDurationExpiredTime)
                            {
                                var dmgHistory = new DamageHistoryInfo(this);
                                OnDeath(dmgHistory, DamageType.Bludgeon);
                                Die(dmgHistory, dmgHistory);

                                if (PropertyManager.GetBool("town_control_enable_webhook_debug").Item)
                                {
                                    try
                                    {
                                        var webhookUrl = PropertyManager.GetString("town_control_globals_webhook").Item;
                                        if (!string.IsNullOrEmpty(webhookUrl))
                                        {
                                            _ = TurbineChatHandler.SendWebhookedChat("God of PK", "DEBUG: Creature_Tick.Heartbeat found conflict boss alive after event expiration", webhookUrl, "General");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        log.ErrorFormat("Failed sending TownControl global message to webhook. Ex:{0}", ex);
                                    }
                                }
                            }
                            else
                            {
                                //Send global broadcasts to update on remaining hp / time left in the event
                                if (!TownControl_ConflictBroadcast1Sent)
                                {
                                    if (this.Health.Percent < 0.75 || DateTime.UtcNow > tcEvent.EventStartDateTime.Value.AddSeconds(town.ConflictLength / 4))
                                    {
                                        string msg = $"{town.TownName} is under attack! Clan {tcEvent.AttackingClanName} has reduced the town's defenses to {Math.Round(this.Health.Percent * 100, 0)}% with {Math.Round((tcEventDurationExpiredTime - DateTime.UtcNow).TotalMinutes, 0, MidpointRounding.AwayFromZero)} minutes remaining.";
                                        PlayerManager.BroadcastToAll(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                                        TownControl_ConflictBroadcast1Sent = true;

                                        //Send global to TC webhook
                                        try
                                        {
                                            var webhookUrl = PropertyManager.GetString("town_control_globals_webhook").Item;
                                            if (!string.IsNullOrEmpty(webhookUrl))
                                            {
                                                _ = TurbineChatHandler.SendWebhookedChat("God of PK", msg, webhookUrl, "General");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            log.ErrorFormat("Failed sending TownControl global message to webhook. Ex:{0}", ex);
                                        }
                                    }
                                }
                                else if (!TownControl_ConflictBroadcast2Sent)
                                {
                                    if (this.Health.Percent < 0.5 || DateTime.UtcNow > tcEvent.EventStartDateTime.Value.AddSeconds(town.ConflictLength / 2))
                                    {
                                        string msg = $"{town.TownName} is under attack! Clan {tcEvent.AttackingClanName} has reduced the town's defenses to {Math.Round(this.Health.Percent * 100, 0)}% with {Math.Round((tcEventDurationExpiredTime - DateTime.UtcNow).TotalMinutes, 0, MidpointRounding.AwayFromZero)} minutes remaining.";
                                        PlayerManager.BroadcastToAll(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                                        TownControl_ConflictBroadcast2Sent = true;

                                        //Send global to TC webhook
                                        try
                                        {
                                            var webhookUrl = PropertyManager.GetString("town_control_globals_webhook").Item;
                                            if (!string.IsNullOrEmpty(webhookUrl))
                                            {
                                                _ = TurbineChatHandler.SendWebhookedChat("God of PK", msg, webhookUrl, "General");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            log.ErrorFormat("Failed sending TownControl global message to webhook. Ex:{0}", ex);
                                        }
                                    }
                                }
                                else if (!TownControl_ConflictBroadcast3Sent)
                                {
                                    if (this.Health.Percent < 0.25 || DateTime.UtcNow > tcEvent.EventStartDateTime.Value.AddSeconds(town.ConflictLength / 1.25))
                                    {
                                        string msg = $"{town.TownName} is under attack! Clan {tcEvent.AttackingClanName} has reduced the town's defenses to {Math.Round(this.Health.Percent * 100, 0)}% with {Math.Round((tcEventDurationExpiredTime - DateTime.UtcNow).TotalMinutes, 0, MidpointRounding.AwayFromZero)} minutes remaining.";
                                        PlayerManager.BroadcastToAll(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                                        TownControl_ConflictBroadcast3Sent = true;

                                        //Send global to TC webhook
                                        try
                                        {
                                            var webhookUrl = PropertyManager.GetString("town_control_globals_webhook").Item;
                                            if (!string.IsNullOrEmpty(webhookUrl))
                                            {
                                                _ = TurbineChatHandler.SendWebhookedChat("God of PK", msg, webhookUrl, "General");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            log.ErrorFormat("Failed sending TownControl global message to webhook. Ex:{0}", ex);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //Init boss
                        if (!TownControl_InitLowHpBroadcastSent)
                        {
                            if (this.Health.Percent < 0.33f)
                            {
                                string bossName = this.Weenie?.PropertiesString.FirstOrDefault(x => x.Key == ACE.Entity.Enum.Properties.PropertyString.Name).Value;
                                var msg = $"{town.TownName} is under attack! {bossName} is faltering and can't hold up much longer.  All those who seek to rule {town.TownName} must come at once.";
                                PlayerManager.BroadcastToAll(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));
                                this.TownControl_InitLowHpBroadcastSent = true;

                                //Send global to TC webhook
                                try
                                {
                                    var webhookUrl = PropertyManager.GetString("town_control_globals_webhook").Item;
                                    if (!string.IsNullOrEmpty(webhookUrl))
                                    {
                                        _ = TurbineChatHandler.SendWebhookedChat("God of PK", msg, webhookUrl, "General");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    log.ErrorFormat("Failed sending TownControl global message to webhook. Ex:{0}", ex);
                                }
                            }
                        }
                        else if (this.Health.Percent > 0.95f)
                        {
                            this.TownControl_InitLowHpBroadcastSent = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Exception applying Town Control behavior in creature tick.  WeenieClassID = {0}, Ex: {1}", this.WeenieClassId, ex);
                }
            }

            else if(WorldBosses.IsWorldBoss(this.WeenieClassId))
            {
                if(this.Health.Percent < 1 && (!this.WorldBoss_LastPeriodicGlobal.HasValue || this.WorldBoss_LastPeriodicGlobal < DateTime.Now.AddMinutes(-2)))
                {
                    var msg = "";
                    string coordsDisplay = this.Location.GetMapCoordStr();
                    if (this.WorldBoss_LastPeriodicGlobal.HasValue)
                    {
                        msg = $"The daring battle to destroy {this.Name} continues! Hurry to join the fray at {coordsDisplay}. Do not dawdle for {this.Name} has already been reduced to {Math.Round(this.Health.Percent * 100)}% of his power. But beware; while some may choose to assist in defeating {this.Name}, others will choose a darker path of greed and spilt blood in pursuit of self enrichment.";
                    }
                    else
                    {
                        msg = $"A brave adventurer has enjoined {this.Name} in battle! All those who seek glory and fortune must head to {coordsDisplay}. But beware; while some may choose to assist in defeating {this.Name}, others will choose a darker path of greed and spilt blood in pursuit of self enrichment.";
                    }
                    PlayerManager.BroadcastToAll(new GameMessageSystemChat(msg, ChatMessageType.Broadcast));

                    if(this.WorldBoss_LastPeriodicGlobal < DateTime.Now.AddMinutes(-10))
                    {
                        //Send global to webhook
                        try
                        {
                            var webhookUrl = PropertyManager.GetString("world_boss_webhook").Item;
                            if (!string.IsNullOrEmpty(webhookUrl))
                            {
                                _ = TurbineChatHandler.SendWebhookedChat("World Boss", msg, webhookUrl, "Global");
                            }
                        }
                        catch (Exception ex)
                        {
                            log.ErrorFormat("Failed sending World Boss Death global message to webhook. Ex:{0}", ex);
                        }
                    }

                    this.WorldBoss_LastPeriodicGlobal = DateTime.Now;
                }
            }

            base.Heartbeat(currentUnixTime);
        }
    }
}
