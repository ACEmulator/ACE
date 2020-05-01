using System;
using System.Collections.Generic;

using log4net;

using ACE.Common;
using ACE.Entity;
using ACE.Server.Entity.Actions;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    public partial class Corpse : Container
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// The maximum number of seconds for an empty corpse to stick around
        /// </summary>
        public static readonly double EmptyDecayTime = 15.0;

        /// <summary>
        /// Flag indicates if a corpse is from a monster or a player
        /// </summary>
        public bool IsMonster = false;

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Corpse(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Corpse(Biota biota) : base(biota)
        {
            SetEphemeralValues();

            // for player corpses restored from database,
            // ensure any floating corpses fall to the ground
            BumpVelocity = true;
        }

        private void SetEphemeralValues()
        {
            ObjectDescriptionFlags |= ObjectDescriptionFlag.Corpse;

            CurrentMotionState = new Motion(MotionStance.NonCombat, MotionCommand.Dead);

            ContainerCapacity = 10;
            ItemCapacity = 120;

            SuppressGenerateEffect = true;
        }

        protected override void OnInitialInventoryLoadCompleted()
        {
            if (Level.HasValue)
            {
                var dtTimeToRot = DateTime.UtcNow.AddSeconds(TimeToRot ?? 0);
                var tsDecay = dtTimeToRot - DateTime.UtcNow;

                log.Debug($"[CORPSE] {Name} (0x{Guid}) Reloaded from Database: Corpse Level: {Level ?? 0} | InventoryLoaded: {InventoryLoaded} | Inventory.Count: {Inventory.Count} | TimeToRot: {TimeToRot} | CreationTimestamp: {CreationTimestamp} ({Time.GetDateTimeFromTimestamp(CreationTimestamp ?? 0).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")}) | Corpse should not decay before: {dtTimeToRot.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")}, {tsDecay.ToString("%d")} day(s), {tsDecay.ToString("%h")} hours, {tsDecay.ToString("%m")} minutes, and {tsDecay.ToString("%s")} seconds from now.");
            }
        }

        /// <summary>
        /// Sets the object description for a corpse
        /// </summary>
        public override ObjDesc CalculateObjDesc()
        {
            if (Biota.PropertiesAnimPart.GetCount(BiotaDatabaseLock) == 0 && Biota.PropertiesPalette.GetCount(BiotaDatabaseLock) == 0 && Biota.PropertiesTextureMap.GetCount(BiotaDatabaseLock) == 0)
                return base.CalculateObjDesc(); // No Saved ObjDesc, let base handle it.

            var objDesc = new ObjDesc();

            AddBaseModelData(objDesc);

            Biota.PropertiesAnimPart.CopyTo(objDesc.AnimPartChanges, BiotaDatabaseLock);

            Biota.PropertiesPalette.CopyTo(objDesc.SubPalettes, BiotaDatabaseLock);

            Biota.PropertiesTextureMap.CopyTo(objDesc.TextureChanges, BiotaDatabaseLock);

            return objDesc;
        }

        /// <summary>
        /// Sets the decay time for player corpse.
        /// This should be called AFTER the items (if any) have been added to the corpse.
        /// Corpses that have no items will decay much faster.
        /// </summary>
        public void RecalculateDecayTime(Player player)
        {
            // empty corpses decay faster
            if (Inventory.Count == 0)
                TimeToRot = EmptyDecayTime;
            else
                // a player corpse decays after 5 mins * playerLevel with a minimum of 1 hour
                TimeToRot = Math.Max(3600, (player.Level ?? 1) * 300);

            var dtTimeToRot = DateTime.UtcNow.AddSeconds(TimeToRot ?? 0);
            var tsDecay = dtTimeToRot - DateTime.UtcNow;

            Level = player.Level ?? 1;

            log.Debug($"[CORPSE] {Name}.RecalculateDecayTime({player.Name}) 0x{Guid}: Player Level: {player.Level} | Inventory.Count: {Inventory.Count} | TimeToRot: {TimeToRot} | CreationTimestamp: {CreationTimestamp} ({Time.GetDateTimeFromTimestamp(CreationTimestamp ?? 0).ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")}) | Corpse should not decay before: {dtTimeToRot.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss")}, {tsDecay.ToString("%d")} day(s), {tsDecay.ToString("%h")} hours, {tsDecay.ToString("%m")} minutes, and {tsDecay.ToString("%s")} seconds from now.");
        }

        /// <summary>
        /// Called when a player attempts to loot a corpse
        /// </summary>
        public override void Open(Player player)
        {
            // check for looting permission
            if (!HasPermission(player))
            {
                player.Session.Network.EnqueueSend(new GameMessageSystemChat("You don't have permission to loot the " + Name, ChatMessageType.Broadcast));
                return;
            }
            base.Open(player);
        }

        /// <summary>
        /// Returns TRUE if input player has permission to loot this corpse
        /// </summary>
        public bool HasPermission(Player player)
        {
            // players can loot their own corpses
            if (VictimId == null || player.Guid.Full == VictimId)
                return true;

            // players can loot monsters they killed
            if (KillerId != null && player.Guid.Full == KillerId || IsLooted && !CorpseGeneratedRare)
                return true;

            // players can /permit other players to loot their corpse if not killed by another player killer.
            if (player.HasLootPermission(new ObjectGuid(VictimId.Value)) && PkLevel != PKLevel.PK)
                return true;

            // all players can loot monster corpses after 1/2 decay time except if corpse generates a rare
            if (TimeToRot != null && TimeToRot < HalfLife && !new ObjectGuid(VictimId.Value).IsPlayer() && !CorpseGeneratedRare)
                return true;

            // players in the same fellowship as the killer w/ loot sharing enabled except if corpse generates a rare
            if (player.Fellowship != null && player.Fellowship.ShareLoot)
            {
                var onlinePlayer = PlayerManager.GetOnlinePlayer(KillerId ?? 0);
                if (onlinePlayer != null && onlinePlayer.Fellowship != null && player.Fellowship == onlinePlayer.Fellowship && !CorpseGeneratedRare)
                    return true;
            }
            return false;
        }

        public bool IsLooted;

        /// <summary>
        /// The number of seconds before all players can loot a monster corpse
        /// </summary>
        public static int HalfLife = 180;


        public override void Close(Player player)
        {
            base.Close(player);

            if (VictimId != null && !new ObjectGuid(VictimId.Value).IsPlayer())
                IsLooted = true;
        }

        public bool CorpseGeneratedRare
        {
            get => GetProperty(PropertyBool.CorpseGeneratedRare) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.CorpseGeneratedRare); else SetProperty(PropertyBool.CorpseGeneratedRare, value); }
        }

        public bool IsOnNoDropLandblock => Location != null ? NoDrop_Landblocks.Contains(Location.LandblockId.Landblock) : false;

        public override bool EnterWorld()
        {
            var actionChain = new ActionChain();

            var success = base.EnterWorld();
            if (!success)
            {
                log.Error($"{Name} ({Guid}) failed to spawn @ {Location?.ToLOCString()}");
                return false;
            }

            actionChain.AddDelaySeconds(0.5f);
            actionChain.AddAction(this, () =>
            {
                if (Location != null && CorpseGeneratedRare)
                {
                    EnqueueBroadcast(new GameMessageSystemChat($"{killerName} has discovered the {rareGenerated.Name}!", ChatMessageType.System));
                    ApplySoundEffects(Sound.TriggerActivated, 10);
                }
            });
            actionChain.EnqueueChain();

            return true;
        }

        private WorldObject rareGenerated;
        private string killerName;

        /// <summary>
        /// Called to attempt to generate rare and add to corpse inventory
        /// </summary>
        public void TryGenerateRare(DamageHistoryInfo killer)
        {
            var killerPlayer = killer.TryGetAttacker() as Player;
            var timestamp = (int)Time.GetUnixTime();
            var luck = 0;
            var secondChanceGranted = false;

            var realTimeRares = PropertyManager.GetBool("rares_real_time").Item;
            var realTimeRaresAlt = PropertyManager.GetBool("rares_real_time_v2").Item;
            if (realTimeRares && killerPlayer != null)
            {
                if (killerPlayer.RaresLoginTimestamp.HasValue)
                {
                    // http://acpedia.org/wiki/Announcements_-_2010/04_-_Shedding_Skin#Rares_Update

                    // Real Time Rares work the same as they always have. It rolls a number between 1 second and 2 months worth of seconds. When that number is up you get an additional chance of finding a rare on any valid rare kill.
                    // That additional chance is very high. You can still only find one rare on any given kill but it's possible to find a normal rare when your Real Time Rare timer is up but you haven't found one yet.

                    var now = Time.GetDateTimeFromTimestamp(timestamp);
                    var playerLastRareFound = Time.GetDateTimeFromTimestamp(killerPlayer.RaresLoginTimestamp.Value);

                    if (now >= playerLastRareFound)
                        secondChanceGranted = true;
                }
                else
                    killerPlayer.RaresLoginTimestamp = (int)Time.GetFutureUnixTime(ThreadSafeRandom.Next(1, (int)PropertyManager.GetLong("rares_max_seconds_between").Item));
            }
            else if (realTimeRaresAlt && killerPlayer != null)
            {
                if (killerPlayer.RaresLoginTimestamp.HasValue)
                {
                    // This version of the system is based on interpretation of the following way the system was originally described. The one above is how it was stated to *really* works as detailed in a dev chat from 2010.

                    // http://acpedia.org/wiki/Rare#Real_Time_Rares

                    // Also there is a real time rare timer for each character, this timer starts the first time you kill a rare eligible creature. A character such as a mule that has never killed anything will not have an active timer.
                    // It works by looking at the real time that has elapsed since the last rare was found, it increases as the time gets closer to two months (real life time) at which point it is a 100% chance.
                    // People have found that for characters that don't normally hunt, it's best to take them out at at the 30 day mark and a rare will drop after a few kills (although it sometimes can take longer since it's still a % chance at the 30 day mark).

                    var now = Time.GetDateTimeFromTimestamp(timestamp);
                    var playerLastRareFound = Time.GetDateTimeFromTimestamp(killerPlayer.RaresLoginTimestamp.Value);
                    var timeBetweenRareSighting = now - playerLastRareFound;
                    var daysSinceRareSighting = timeBetweenRareSighting.TotalDays;

                    var maxDaysSinceLastRareFound = (int)PropertyManager.GetLong("rares_max_days_between").Item; // 30? 45? 60?
                    var chancesModifier = Math.Round(daysSinceRareSighting / maxDaysSinceLastRareFound, 2, MidpointRounding.ToZero);
                    var chancesModifierAdjusted = Math.Min(chancesModifier, 1.0f);

                    var t1_chance = 2500;
                    luck = (int)Math.Round(t1_chance * chancesModifierAdjusted, 0, MidpointRounding.ToZero);
                }
                else
                    killerPlayer.RaresLoginTimestamp = timestamp;
            }

            var wo = LootGenerationFactory.TryCreateRare(luck);

            if (secondChanceGranted && wo == null)
                wo = LootGenerationFactory.TryCreateRare(2490);

            if (wo == null)
                return;

            if (!wo.IconUnderlayId.HasValue || wo.IconUnderlayId.Value != 0x6005B0C) // ensure icon underlay exists for rare (loot profiles use this)
                wo.IconUnderlayId = 0x6005B0C;

            var tier = LootGenerationFactory.GetRareTier(wo.WeenieClassId);
            LootGenerationFactory.RareChances.TryGetValue(tier, out var chance);

            log.Debug($"[LOOT][RARE] {Name} ({Guid}) generated rare {wo.Name} ({wo.Guid}) for {killer.Name} ({killer.Guid})");
            log.Debug($"[LOOT][RARE] Tier {tier} -- 1 / {chance:N0} chance -- {luck:N0} luck");

            if (TryAddToInventory(wo))
            {
                rareGenerated = wo;
                killerName = killer.Name.TrimStart('+');
                CorpseGeneratedRare = true;
                LongDesc += " This corpse generated a rare item!";
                if (killerPlayer != null)
                {
                    if (realTimeRares)
                        killerPlayer.RaresLoginTimestamp = (int)Time.GetFutureUnixTime(ThreadSafeRandom.Next(1, (int)PropertyManager.GetLong("rares_max_seconds_between").Item));
                    else
                        killerPlayer.RaresLoginTimestamp = timestamp;
                    switch (tier)
                    {
                        case 1:
                            killerPlayer.RaresTierOne++;
                            killerPlayer.RaresTierOneLogin = timestamp;
                            break;
                        case 2:
                            killerPlayer.RaresTierTwo++;
                            killerPlayer.RaresTierTwoLogin = timestamp;
                            break;
                        case 3:
                            killerPlayer.RaresTierThree++;
                            killerPlayer.RaresTierThreeLogin = timestamp;
                            break;
                        case 4:
                            killerPlayer.RaresTierFour++;
                            killerPlayer.RaresTierFourLogin = timestamp;
                            break;
                        case 5:
                            killerPlayer.RaresTierFive++;
                            killerPlayer.RaresTierFiveLogin = timestamp;
                            break;
                        case 6:
                            killerPlayer.RaresTierSix++;
                            killerPlayer.RaresTierSixLogin = timestamp;
                            break;
                        //case 7:
                        //    killerPlayer.RaresTierSeven++;
                        //    killerPlayer.RaresTierSevenLogin = timestamp;
                        //    break;
                    }
                }
            }
            else
                log.Error($"[RARE] failed to add to corpse inventory");
        }

        /// <summary>
        /// A list of landblocks the player cannot drop items on corpse on death 
        /// </summary>
        public static HashSet<ushort> NoDrop_Landblocks = new HashSet<ushort>()
        {
            0x005F,     // Tanada House of Pancakes (Seasonal)
            0x00AF,     // Colosseum Staging Area and Secret Mini-Bosses
            0x00B0,     // Colosseum Arena One
            0x00B1,     // Colosseum Arena Two
            0x00B2,     // Colosseum Arena Three
            0x00B3,     // Colosseum Arena Four
            0x00B4,     // Colosseum Arena Five
            0x00B6,     // Colosseum Arena Mini-Bosses
            0x5960,     // Gauntlet Arena One (Celestial Hand)
            0x5961,     // Gauntlet Arena Two (Celestial Hand)
            0x5962,     // Gauntlet Arena One (Eldritch Web)
            0x5963,     // Gauntlet Arena Two (Eldritch Web)
            0x5964,     // Gauntlet Arena One (Radiant Blood)
            0x5965,     // Gauntlet Arena Two (Radiant Blood)
            0x596B,     // Gauntlet Staging Area (All Societies)
            0x8A04,     // Night Club (Seasonal Anniversary)
            0xB5F0,     // Aerfalle's Sanctum
        };
    }
}
