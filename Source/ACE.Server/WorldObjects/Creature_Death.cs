using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Entity.Models;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Entity.TownControl;
using ACE.Server.Entity.WorldBoss;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Handlers;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public TreasureDeath DeathTreasure { get => DeathTreasureType.HasValue ? DatabaseManager.World.GetCachedDeathTreasure(DeathTreasureType.Value) : null; }

        private bool onDeathEntered = false;

        /// <summary>
        /// Called when a monster or player dies, in conjunction with Die()
        /// </summary>
        /// <param name="lastDamager">The last damager that landed the death blow</param>
        /// <param name="damageType">The damage type for the death message</param>
        /// <param name="criticalHit">True if the death blow was a critical hit, generates a critical death message</param>
        public virtual DeathMessage OnDeath(DamageHistoryInfo lastDamager, DamageType damageType, bool criticalHit = false)
        {
            if (onDeathEntered)
                return GetDeathMessage(lastDamager, damageType, criticalHit);

            onDeathEntered = true;

            IsTurning = false;
            IsMoving = false;

            //QuestManager.OnDeath(lastDamager?.TryGetAttacker());

            if (KillQuest != null)
                OnDeath_HandleKillTask(KillQuest);
            if (KillQuest2 != null)
                OnDeath_HandleKillTask(KillQuest2);
            if (KillQuest3 != null)
                OnDeath_HandleKillTask(KillQuest3);

            if (!IsOnNoDeathXPLandblock)
                OnDeath_GrantXP();

            if (this.IsTownControlBoss)
            {
                HandleTownControlBossDeath();
            }

            if(WorldBosses.IsWorldBoss(this.WeenieClassId))
            {
                HandleWorldBossDeath();
            }

            return GetDeathMessage(lastDamager, damageType, criticalHit);
        }


        public DeathMessage GetDeathMessage(DamageHistoryInfo lastDamagerInfo, DamageType damageType, bool criticalHit = false)
        {
            var lastDamager = lastDamagerInfo?.TryGetAttacker();

            if (lastDamagerInfo == null || lastDamagerInfo.Guid == Guid || lastDamager is Hotspot || lastDamager is Food)   // !(lastDamager is Creature)?
                return Strings.General[1];

            var deathMessage = Strings.GetDeathMessage(damageType, criticalHit);

            // if killed by a player, send them a message
            if (lastDamagerInfo.IsPlayer)
            {
                if (criticalHit && this is Player)
                    deathMessage = Strings.PKCritical[0];

                var killerMsg = string.Format(deathMessage.Killer, Name);

                if (lastDamager is Player playerKiller)
                    playerKiller.Session.Network.EnqueueSend(new GameEventKillerNotification(playerKiller.Session, killerMsg));
            }
            return deathMessage;
        }

        /// <summary>
        /// Kills a player/creature and performs the full death sequence
        /// </summary>
        public void Die()
        {
            Die(DamageHistory.LastDamager, DamageHistory.TopDamager);
        }

        private bool dieEntered = false;

        /// <summary>
        /// Performs the full death sequence for non-Player creatures
        /// </summary>
        protected virtual void Die(DamageHistoryInfo lastDamager, DamageHistoryInfo topDamager)
        {
            if (dieEntered) return;

            dieEntered = true;

            UpdateVital(Health, 0);

            if (topDamager != null)
            {
                KillerId = topDamager.Guid.Full;

                if (topDamager.IsPlayer)
                {
                    var topDamagerPlayer = topDamager.TryGetAttacker();
                    if (topDamagerPlayer != null)
                        topDamagerPlayer.CreatureKills = (topDamagerPlayer.CreatureKills ?? 0) + 1;
                }
            }

            CurrentMotionState = new Motion(MotionStance.NonCombat, MotionCommand.Ready);
            //IsMonster = false;

            PhysicsObj.StopCompletely(true);

            // broadcast death animation
            var motionDeath = new Motion(MotionStance.NonCombat, MotionCommand.Dead);
            var deathAnimLength = ExecuteMotion(motionDeath);

            EmoteManager.OnDeath(lastDamager);

            var dieChain = new ActionChain();

            // wait for death animation to finish
            //var deathAnimLength = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.Dead);
            dieChain.AddDelaySeconds(deathAnimLength);

            dieChain.AddAction(this, () =>
            {
                CreateCorpse(topDamager);
                Destroy();
            });

            dieChain.EnqueueChain();
        }

        /// <summary>
        /// Called when an admin player uses the /smite command
        /// to instantly kill a creature
        /// </summary>
        public void Smite(WorldObject smiter, bool useTakeDamage = false)
        {
            if (useTakeDamage)
            {
                // deal remaining damage
                TakeDamage(smiter, DamageType.Bludgeon, Health.Current);
            }
            else
            {
                OnDeath();
                var smiterInfo = new DamageHistoryInfo(smiter);
                Die(smiterInfo, smiterInfo);
            }
        }

        public void OnDeath()
        {
            OnDeath(null, DamageType.Undef);
        }

        /// <summary>
        /// Grants XP to players in damage history
        /// </summary>
        public void OnDeath_GrantXP()
        {
            if (this is Player && PlayerKillerStatus == PlayerKillerStatus.PKLite)
                return;

            var totalHealth = DamageHistory.TotalHealth;

            if (totalHealth == 0)
                return;

            foreach (var kvp in DamageHistory.TotalDamage)
            {
                var damager = kvp.Value.TryGetAttacker();

                var playerDamager = damager as Player;

                if (playerDamager == null && kvp.Value.PetOwner != null)
                    playerDamager = kvp.Value.TryGetPetOwner();

                if (playerDamager == null)
                    continue;

                var totalDamage = kvp.Value.TotalDamage;

                var damagePercent = totalDamage / totalHealth;

                var totalXP = (XpOverride ?? 0) * damagePercent;

                playerDamager.EarnXP((long)Math.Round(totalXP), XpType.Kill);

                // handle luminance
                if (LuminanceAward != null)
                {
                    var totalLuminance = (long)Math.Round(LuminanceAward.Value * damagePercent);
                    playerDamager.EarnLuminance(totalLuminance, XpType.Kill);
                }
            }
        }

        /// <summary>
        /// Handles the KillTask for a killed creature
        /// </summary>
        public void OnDeath_HandleKillTask(string killQuest)
        {
            /*var receivers = KillTask_GetEligibleReceivers(killQuest);

            foreach (var receiver in receivers)
            {
                var damager = receiver.Value.TryGetAttacker();

                var player = damager as Player;

                if (player == null && receiver.Value.PetOwner != null)
                    player = receiver.Value.TryGetPetOwner();

                if (player != null)
                    player.QuestManager.HandleKillTask(killQuest, this);
            }*/

            // new method

            // with full fellowship support and new config option for capping,
            // building a pre-flattened structure is no longer really necessary,
            // and we can do this more iteratively.

            // one caveat to do this, we need to keep track of player and summoning caps separately
            // this is to prevent ordering bugs, such as a player being processed after a summon,
            // and already being at the 1 cap for players

            var summon_credit_cap = (int)PropertyManager.GetLong("summoning_killtask_multicredit_cap").Item - 1;

            var playerCredits = new Dictionary<ObjectGuid, int>();
            var summonCredits = new Dictionary<ObjectGuid, int>();

            // this option isn't really needed anymore, but keeping it around for compatibility
            // it is now synonymous with summoning_killtask_multicredit_cap <= 1
            if (!PropertyManager.GetBool("allow_summoning_killtask_multicredit").Item)
                summon_credit_cap = 0;

            foreach (var kvp in DamageHistory.TotalDamage)
            {
                if (kvp.Value.TotalDamage <= 0)
                    continue;

                var damager = kvp.Value.TryGetAttacker();

                var combatPet = false;

                var playerDamager = damager as Player;

                if (playerDamager == null && kvp.Value.PetOwner != null)
                {
                    playerDamager = kvp.Value.TryGetPetOwner();
                    combatPet = true;
                }

                if (playerDamager == null)
                    continue;

                var killTaskCredits = combatPet ? summonCredits : playerCredits;

                var cap = combatPet ? summon_credit_cap : 1;

                if (cap <= 0)
                {
                    // handle special case: use playerCredits
                    killTaskCredits = playerCredits;
                    cap = 1;
                }

                if (playerDamager.QuestManager.HasQuest(killQuest))
                {
                    TryHandleKillTask(playerDamager, killQuest, killTaskCredits, cap);
                }
                // check option that requires killer to have killtask to pass to fellows
                else if (!PropertyManager.GetBool("fellow_kt_killer").Item)   
                {
                    continue;
                }

                if (playerDamager.Fellowship == null)
                    continue;

                // share with fellows in kill task range
                var fellows = playerDamager.Fellowship.WithinRange(playerDamager);

                foreach (var fellow in fellows)
                {
                    if (fellow.QuestManager.HasQuest(killQuest))
                        TryHandleKillTask(fellow, killQuest, killTaskCredits, cap);
                }
            }
        }

        public bool TryHandleKillTask(Player player, string killTask, Dictionary<ObjectGuid, int> killTaskCredits, int cap)
        {
            if (killTaskCredits.TryGetValue(player.Guid, out var currentCredits))
            {
                if (currentCredits >= cap)
                    return false;

                killTaskCredits[player.Guid]++;
            }
            else
                killTaskCredits[player.Guid] = 1;

            player.QuestManager.HandleKillTask(killTask, this);

            return true;
        }

        /// <summary>
        /// Returns a flattened structure of eligible Players, Fellows, and CombatPets
        /// </summary>
        public Dictionary<ObjectGuid, DamageHistoryInfo> KillTask_GetEligibleReceivers(string killQuest)
        {
            // http://acpedia.org/wiki/Announcements_-_2012/12_-_A_Growing_Twilight#Release_Notes

            var questName = QuestManager.GetQuestName(killQuest);

            // we are using DamageHistoryInfo here, instead of Creature or WorldObjectInfo
            // WeakReference<CombatPet> may be null for expired CombatPets, but we still need the WeakReference<PetOwner> references

            var receivers = new Dictionary<ObjectGuid, DamageHistoryInfo>();

            foreach (var kvp in DamageHistory.TotalDamage)
            {
                if (kvp.Value.TotalDamage <= 0)
                    continue;

                var damager = kvp.Value.TryGetAttacker();

                var playerDamager = damager as Player;

                if (playerDamager == null && kvp.Value.PetOwner != null)
                {
                    // handle combat pets
                    playerDamager = kvp.Value.TryGetPetOwner();

                    if (playerDamager != null && playerDamager.QuestManager.HasQuest(questName))
                    {
                        // only add combat pet to eligible receivers if player has quest, and allow_summoning_killtask_multicredit = true (default, retail)
                        if (DamageHistory.HasDamager(playerDamager, true) && PropertyManager.GetBool("allow_summoning_killtask_multicredit").Item)
                            receivers[kvp.Value.Guid] = kvp.Value;  // add CombatPet
                        else
                            receivers[playerDamager.Guid] = new DamageHistoryInfo(playerDamager);   // add dummy profile for PetOwner
                    }

                    // regardless if combat pet is eligible, we still want to continue traversing to the pet owner, and possibly fellows

                    // in a scenario where combat pet does 100% damage:

                    // - regardless if allow_summoning_killtask_multicredit is enabled/disabled, it should continue traversing into pet owner and possibly their fellows

                    // - if pet owner doesn't have kill task, and fellow_kt_killer=false, any fellows with the task should still receive 1 credit
                }

                if (playerDamager == null)
                    continue;

                // factors:
                // - has quest
                // - is killer (last damager, top damager, or any damager? in current context, considering it to be any damager)
                // - has fellowship
                // - server option: fellow_kt_killer
                // - server option: fellow_kt_landblock

                if (playerDamager.QuestManager.HasQuest(questName))
                {
                    // just add a fake DamageHistoryInfo for reference
                    receivers[playerDamager.Guid] = new DamageHistoryInfo(playerDamager);
                }
                else if (PropertyManager.GetBool("fellow_kt_killer").Item)
                {
                    // if this option is enabled (retail default), the killer is required to have kill task
                    // for it to share with fellowship
                    continue;
                }

                // we want to add fellowship members in a flattened structure
                // in this inner loop, instead of the outer loop

                // scenarios:

                // i am a summoner in a fellowship with 1 other player
                // we both have a killtask

                // - my combatpet does 100% damage to the monster
                // result: i get 1 killtask credit, and my fellow gets 1 killtask credit

                // - my combatpet does 50% damage to monster, and i do 50% damage
                // result: i get 2 killtask credits (1 if allow_summoning_killtask_multicredit server option is disabled), and my fellow gets 1 killtask credit
                // after update should be 2/2, instead of 2/1

                // - my combatpet does 33% damage to monster, i do 33% damage, and fellow does 33% damage
                // result: same as previous scenario
                // after update should be 2/2, instead of 2/1 again

                // 2 players not in a fellowship both have a killtask
                // they each do 50% damage to monster

                // result: both players receive killtask credit

                if (playerDamager.Fellowship == null)
                    continue;

                // share with fellows in kill task range
                var fellows = playerDamager.Fellowship.WithinRange(playerDamager);

                foreach (var fellow in fellows)
                {
                    if (fellow.QuestManager.HasQuest(questName))
                        receivers[fellow.Guid] = new DamageHistoryInfo(fellow);
                }
            }
            return receivers;
        }

        /// <summary>
        /// Create a corpse for both creatures and players currently
        /// </summary>
        protected void CreateCorpse(DamageHistoryInfo killer, bool hadVitae = false)
        {
            if (NoCorpse)
            {
                if (killer != null && killer.IsOlthoiPlayer) return;

                var loot = GenerateTreasure(killer, null);

                foreach(var item in loot)
                {
                    if (!string.IsNullOrEmpty(item.Quest)) // if the item has a Quest string, make the creature a "generator" of the item so that the pickup action applies the quest. 
                        item.GeneratorId = Guid.Full; 
                    item.Location = new Position(Location);
                    LandblockManager.AddObject(item);
                }
                return;
            }

            var cachedWeenie = DatabaseManager.World.GetCachedWeenie("corpse");

            var corpse = WorldObjectFactory.CreateNewWorldObject(cachedWeenie) as Corpse;

            var prefix = "Corpse";

            if (TreasureCorpse)
            {
                // Hardcoded values from PCAPs of Treasure Pile Corpses, everything else lines up exactly with existing corpse weenie
                corpse.SetupTableId  = 0x02000EC4;
                corpse.MotionTableId = 0x0900019B;
                corpse.SoundTableId  = 0x200000C2;
                corpse.ObjScale      = 0.4f;

                prefix = "Treasure";
            }
            else
            {
                corpse.SetupTableId = SetupTableId;
                corpse.MotionTableId = MotionTableId;
                //corpse.SoundTableId = SoundTableId; // Do not change sound table for corpses
                corpse.PaletteBaseDID = PaletteBaseDID;
                corpse.ClothingBase = ClothingBase;
                corpse.PhysicsTableId = PhysicsTableId;

                if (ObjScale.HasValue)
                    corpse.ObjScale = ObjScale;
                if (PaletteTemplate.HasValue)
                    corpse.PaletteTemplate = PaletteTemplate;
                if (Shade.HasValue)
                    corpse.Shade = Shade;
                //if (Translucency.HasValue) // Shadows have Translucency but their corpses do not, videographic evidence can be found on YouTube.
                //corpse.Translucency = Translucency;


                // Pull and save objdesc for correct corpse apperance at time of death
                var objDesc = CalculateObjDesc();

                corpse.Biota.PropertiesAnimPart = objDesc.AnimPartChanges.Clone(corpse.BiotaDatabaseLock);

                corpse.Biota.PropertiesPalette = objDesc.SubPalettes.Clone(corpse.BiotaDatabaseLock);

                corpse.Biota.PropertiesTextureMap = objDesc.TextureChanges.Clone(corpse.BiotaDatabaseLock);
            }

            // use the physics location for accuracy,
            // especially while jumping
            corpse.Location = PhysicsObj.Position.ACEPosition();

            corpse.VictimId = Guid.Full;
            corpse.Name = $"{prefix} of {Name}";

            // set 'killed by' for looting rights
            var killerName = "misadventure";
            if (killer != null)
            {
                if (!(Generator != null && Generator.Guid == killer.Guid) && Guid != killer.Guid)
                {
                    if (!string.IsNullOrWhiteSpace(killer.Name))
                        killerName = killer.Name.TrimStart('+');  // vtank requires + to be stripped for regex matching.

                    corpse.KillerId = killer.Guid.Full;

                    if (killer.PetOwner != null)
                    {
                        var petOwner = killer.TryGetPetOwner();
                        if (petOwner != null)
                            corpse.KillerId = petOwner.Guid.Full;
                    }
                }
            }

            corpse.LongDesc = $"Killed by {killerName}.";

            bool saveCorpse = false;

            var player = this as Player;

            if (player != null)
            {
                corpse.SetPosition(PositionType.Location, corpse.Location);
                var dropped = killer != null && killer.IsOlthoiPlayer ? player.CalculateDeathItems_Olthoi(corpse, hadVitae) : player.CalculateDeathItems(corpse);
                corpse.RecalculateDecayTime(player);

                if (dropped.Count > 0)
                    saveCorpse = true;

                if ((player.Location.Cell & 0xFFFF) < 0x100)
                {
                    player.SetPosition(PositionType.LastOutsideDeath, new Position(corpse.Location));
                    player.Session.Network.EnqueueSend(new GameMessagePrivateUpdatePosition(player, PositionType.LastOutsideDeath, corpse.Location));

                    if (dropped.Count > 0)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your corpse is located at ({corpse.Location.GetMapCoordStr()}).", ChatMessageType.Broadcast));
                }

                var isPKdeath = player.IsPKDeath(killer);
                var isPKLdeath = player.IsPKLiteDeath(killer);

                if (isPKdeath)
                {
                    corpse.PkLevel = PKLevel.PK;                                      
                }

                if (!isPKdeath && !isPKLdeath)
                {
                    var miserAug = player.AugmentationLessDeathItemLoss * 5;
                    if (miserAug > 0)
                        player.Session.Network.EnqueueSend(new GameMessageSystemChat($"Your augmentation has reduced the number of items you can lose by {miserAug}!", ChatMessageType.Broadcast));
                }

                if (dropped.Count == 0 && !isPKLdeath)
                    player.Session.Network.EnqueueSend(new GameMessageSystemChat($"You have retained all your items. You do not need to recover your corpse!", ChatMessageType.Broadcast));
            }
            else
            {
                corpse.IsMonster = true;

                if (killer == null || !killer.IsOlthoiPlayer)
                    GenerateTreasure(killer, corpse);
                else
                    GenerateTreasure_Olthoi(killer, corpse);

                if (killer != null && killer.IsPlayer && !killer.IsOlthoiPlayer)
                {
                    if (Level >= 100)
                    {
                        CanGenerateRare = true;
                    }
                    else
                    {
                        var killerPlayer = killer.TryGetAttacker();
                        if (killerPlayer != null && Level > killerPlayer.Level)
                            CanGenerateRare = true;
                    }
                }
                else
                    CanGenerateRare = false;
            }

            corpse.RemoveProperty(PropertyInt.Value);

            if (CanGenerateRare && killer != null)
                corpse.TryGenerateRare(killer);

            corpse.InitPhysicsObj();

            // persist the original creature velocity (only used for falling) to corpse
            corpse.PhysicsObj.Velocity = PhysicsObj.Velocity;

            corpse.EnterWorld();

            if (player != null)
            {
                if (corpse.PhysicsObj == null || corpse.PhysicsObj.Position == null)
                    log.Debug($"[CORPSE] {Name}'s corpse (0x{corpse.Guid}) failed to spawn! Tried at {player.Location.ToLOCString()}");
                else
                    log.Debug($"[CORPSE] {Name}'s corpse (0x{corpse.Guid}) is located at {corpse.PhysicsObj.Position}");
            }

            if (saveCorpse)
            {
                corpse.SaveBiotaToDatabase();

                foreach (var item in corpse.Inventory.Values)
                    item.SaveBiotaToDatabase();
            }            
        }

        public bool CanGenerateRare
        {
            get => GetProperty(PropertyBool.CanGenerateRare) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.CanGenerateRare); else SetProperty(PropertyBool.CanGenerateRare, value); }
        }

        /// <summary>
        /// Transfers generated treasure from creature to corpse
        /// </summary>
        private List<WorldObject> GenerateTreasure(DamageHistoryInfo killer, Corpse corpse)
        {
            var droppedItems = new List<WorldObject>();

            // create death treasure from loot generation factory
            if (DeathTreasure != null)
            {
                if (this.CurrentLandblock != null)
                {
                    if (PropertyManager.GetBool("equipmentset_whitelist_enabled").Item)
                    {
                        DeathTreasure.DisableSets = !Whitelist.IsEquipmentSetWhitelistedLandblock(this.CurrentLandblock.Id.Landblock);
                    }
                    else
                    {
                        DeathTreasure.DisableSets = false;
                    }

                    if (PropertyManager.GetBool("epic_whitelist_enabled").Item)
                    {
                        DeathTreasure.DisableEpicCantrips = !Whitelist.IsEpicWhitelistedLandblock(this.CurrentLandblock.Id.Landblock);
                    }
                    else
                    {
                        DeathTreasure.DisableEpicCantrips = false;
                    }

                    if (PropertyManager.GetBool("legendary_whitelist_enabled").Item)
                    {
                        DeathTreasure.DisableLegendaryCantrips = !Whitelist.IsLegendaryWhitelistedLandblock(this.CurrentLandblock.Id.Landblock);
                    }
                    else
                    {
                        DeathTreasure.DisableLegendaryCantrips = false;
                    }

                    if (PropertyManager.GetBool("aetheria_whitelist_enabled").Item)
                    {
                        DeathTreasure.DisableAetheria = !Whitelist.IsAetheriaWhitelistedLandblock(this.CurrentLandblock.Id.Landblock);
                    }
                    else
                    {
                        DeathTreasure.DisableAetheria = false;
                    }

                    if (PropertyManager.GetBool("aetheria_highlevel_whitelist_enabled").Item)
                    {
                        DeathTreasure.DisableHighLevelAetheria = !Whitelist.IsAetheriaWhitelistedLandblock(this.CurrentLandblock.Id.Landblock);
                    }
                    else
                    {
                        DeathTreasure.DisableHighLevelAetheria = false;
                    }

                }

                List<WorldObject> items = LootGenerationFactory.CreateRandomLootObjects(DeathTreasure);
                foreach (WorldObject wo in items)
                {
                    if (corpse != null)
                        corpse.TryAddToInventory(wo);
                    else
                        droppedItems.Add(wo);

                    //DoCantripLogging(killer, wo);
                }
            }

            // move wielded treasure over, which also should include Wielded objects not marked for destroy on death.
            // allow server operators to configure this behavior due to errors in createlist post 16py data
            var dropFlags = PropertyManager.GetBool("creatures_drop_createlist_wield").Item ? DestinationType.WieldTreasure : DestinationType.Treasure;

            var wieldedTreasure = Inventory.Values.Concat(EquippedObjects.Values).Where(i => (i.DestinationType & dropFlags) != 0);
            foreach (var item in wieldedTreasure.ToList())
            {
                if (item.Bonded == BondedStatus.Destroy)
                    continue;

                if (TryDequipObjectWithBroadcasting(item.Guid, out var wo, out var wieldedLocation))
                    EnqueueBroadcast(new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Wielder, ObjectGuid.Invalid));

                if (corpse != null)
                {
                    corpse.TryAddToInventory(item);
                    EnqueueBroadcast(new GameMessagePublicUpdateInstanceID(item, PropertyInstanceId.Container, corpse.Guid), new GameMessagePickupEvent(item));
                }
                else
                    droppedItems.Add(item);
            }

            // contain and non-wielded treasure create
            if (Biota.PropertiesCreateList != null)
            {
                var createList = Biota.PropertiesCreateList.Where(i => (i.DestinationType & DestinationType.Contain) != 0 ||
                                (i.DestinationType & DestinationType.Treasure) != 0 && (i.DestinationType & DestinationType.Wield) == 0).ToList();

                var selected = CreateListSelect(createList);

                foreach (var item in selected)
                {
                    var wo = WorldObjectFactory.CreateNewWorldObject(item);

                    if (wo != null)
                    {
                        if (corpse != null)
                            corpse.TryAddToInventory(wo);
                        else
                            droppedItems.Add(wo);
                    }
                }
            }

            return droppedItems;
        }

        /// <summary>
        /// Generates random amounts of slag on a corpse
        /// when an OlthoiPlayer is the killer
        /// </summary>
        private void GenerateTreasure_Olthoi(DamageHistoryInfo killer, Corpse corpse)
        {
            if (DeathTreasure == null) return;

            var slag = LootGenerationFactory.RollSlag(DeathTreasure);

            if (slag == null) return;

            corpse.TryAddToInventory(slag);
        }

        public void DoCantripLogging(DamageHistoryInfo killer, WorldObject wo)
        {
            var epicCantrips = wo.EpicCantrips;
            var legendaryCantrips = wo.LegendaryCantrips;

            if (epicCantrips.Count > 0)
                log.Debug($"[LOOT][EPIC] {Name} ({Guid}) generated item with {epicCantrips.Count} epic{(epicCantrips.Count > 1 ? "s" : "")} - {wo.Name} ({wo.Guid}) - {GetSpellList(epicCantrips)} - killed by {killer?.Name} ({killer?.Guid})");

            if (legendaryCantrips.Count > 0)
                log.Debug($"[LOOT][LEGENDARY] {Name} ({Guid}) generated item with {legendaryCantrips.Count} legendar{(legendaryCantrips.Count > 1 ? "ies" : "y")} - {wo.Name} ({wo.Guid}) - {GetSpellList(legendaryCantrips)} - killed by {killer?.Name} ({killer?.Guid})");
        }

        public static string GetSpellList(Dictionary<int, float> spellTable)
        {
            var spells = new List<Server.Entity.Spell>();

            foreach (var kvp in spellTable)
                spells.Add(new Server.Entity.Spell(kvp.Key, false));

            return string.Join(", ", spells.Select(i => i.Name));
        }

        public bool IsOnNoDeathXPLandblock => Location != null ? NoDeathXP_Landblocks.Contains(Location.LandblockId.Landblock) : false;

        /// <summary>
        /// A list of landblocks the player gains no xp from creature kills
        /// </summary>
        public static HashSet<ushort> NoDeathXP_Landblocks = new HashSet<ushort>()
        {
            0x00B0,     // Colosseum Arena One
            0x00B1,     // Colosseum Arena Two
            0x00B2,     // Colosseum Arena Three
            0x00B3,     // Colosseum Arena Four
            0x00B4,     // Colosseum Arena Five
            0x5960,     // Gauntlet Arena One (Celestial Hand)
            0x5961,     // Gauntlet Arena Two (Celestial Hand)
            0x5962,     // Gauntlet Arena One (Eldritch Web)
            0x5963,     // Gauntlet Arena Two (Eldritch Web)
            0x5964,     // Gauntlet Arena One (Radiant Blood)
            0x5965,     // Gauntlet Arena Two (Radiant Blood)
            0x596B,     // Gauntlet Staging Area (All Societies)
        };

        public void HandleTownControlBossDeath()
        {
            log.Info("Creature_Death.HandleTownControlBossDeath");

            try
            {
                var deadBoss = TownControlBosses.TownControlBossMap[this.WeenieClassId];
                var killer = DamageHistory.TopDamager;

                var deadBossWeenie = DatabaseManager.World.GetWeenie(this.WeenieClassId);
                var deadBossNameProp = deadBossWeenie.WeeniePropertiesString.FirstOrDefault(x => x.Type == (ushort)PropertyString.Name);
                var deadBossName = deadBossNameProp?.Value;

                var town = DatabaseManager.TownControl.GetTownById(deadBoss.TownID);

                if (deadBoss.BossType.Equals(TownControlBossType.InitiationBoss))
                {
                    log.DebugFormat("Town Control - {0} Init boss killed by player guid = {1}", town.TownName, killer.Guid);
                    bool validationFailed = false;

                    if (!killer.IsPlayer)
                    {
                        //TODO - what to do if the killer isn't a player?                        
                        //PlayerManager.BroadcastToAll(new GameMessageSystemChat("DEBUG - Init boss killer is not a player, conflict event not started", ChatMessageType.Broadcast));
                        log.DebugFormat("Town Control - {0} Init boss killer is not a player, conflict event not started", town.TownName);
                        validationFailed = true;
                    }

                    //Get the attacking clan ID and name
                    uint? killerMonarchId = null;
                    string killerAllegName = null;
                    var killerPlayer = PlayerManager.FindByGuid(killer.Guid);
                    var killerAllegiance = AllegianceManager.GetAllegiance(killerPlayer);
                    if (killerAllegiance != null && killerAllegiance.MonarchId.HasValue)
                    {
                        killerMonarchId = killerAllegiance.MonarchId;
                        killerAllegName = killerAllegiance.Monarch.Player.Name;
                        log.DebugFormat("Town Control - {0} Init boss killerMonarchId = {1}, killerAllegName = {2}", town.TownName, killerMonarchId, killerAllegName);
                    }
                    else
                    {
                        //TODO - what do we do if the killer isn't part of any allegiance?
                        log.DebugFormat("Town Control - {0} Init boss killer is not in an allegiance.  Killer PlayerID = {1}", town.TownName, killerPlayer.Guid);
                        //PlayerManager.BroadcastToAll(new GameMessageSystemChat("DEBUG - Killer isn't part of any allegiance.  Conflict event not started", ChatMessageType.Broadcast));
                        this.CurrentLandblock?.EnqueueBroadcast(null, true, null, null, new GameMessageSystemChat($"{deadBossName} has been killed by a poor lone wolf with no allegiance.  {town.TownName} will not yield to an incel.", ChatMessageType.Broadcast));
                        validationFailed = true;
                    }

                    //Verify that the killer is in an allegiance that is whitelisted                    
                    if (!TownControlAllegiances.IsAllowedAllegiance((int)killerMonarchId.Value))
                    {
                        log.DebugFormat("Town Control - {0} Init boss killer is in an allegiance that is not whitelisted.  Killer PlayerID = {1}, Killer MonarchID = {2}", town.TownName, killerPlayer.Guid, killerMonarchId);
                        this.CurrentLandblock?.EnqueueBroadcast(null, true, null, null, new GameMessageSystemChat($"{deadBossName} has been killed by an allegiance who has not been enabled for town control events.  As such, a conflict event will not be started.  Please reach out to an admin to have your clan enabled for town control.", ChatMessageType.Broadcast));
                        validationFailed = true;
                    }

                    //Get the defending clan ID and name, if the town has an owner already
                    //cover scenario where owning clan is no longer a valid monarchy
                    uint? defendingClanId = null;
                    string defendingClanName = null;
                    if (town.CurrentOwnerID.HasValue)
                    {
                        var owningMonarch = PlayerManager.FindByGuid(town.CurrentOwnerID.Value);
                        if (owningMonarch != null)
                        {
                            var owningAlleg = AllegianceManager.GetAllegiance(owningMonarch);
                            if (owningAlleg != null)
                            {
                                if (owningAlleg.Monarch.PlayerGuid != owningMonarch.Guid)
                                {
                                    //TODO
                                    //This is the scenario where the monarch who owned the town has sworn into a different allegiance                                    
                                    //PlayerManager.BroadcastToAll(new GameMessageSystemChat("DEBUG - Owning monarch is no longer a monarch, behaving as if town isn't owned by anyone", ChatMessageType.Broadcast));
                                    log.DebugFormat("Town Control - {0} Owning monarch is no longer a monarch, behaving as if town isn't owned by anyone", town.TownName);
                                }
                                else
                                {
                                    //The owning monarch is still a valid player and still a monarch
                                    defendingClanId = owningAlleg.MonarchId;
                                    defendingClanName = owningAlleg.Monarch.Player.Name;
                                    log.DebugFormat("Town Control - {0} defenders are defendingClanId = {1}, defendingClanName = {2}", town.TownName, defendingClanId, defendingClanName);
                                }
                            }
                            else
                            {
                                //TODO
                                //This is scenario where monarch who owned the town is no longer part of any allegiance
                                //PlayerManager.BroadcastToAll(new GameMessageSystemChat("DEBUG - owningAlleg == null.  Owning monarch is no longer a monarch, behaving as if town isn't owned by anyone", ChatMessageType.Broadcast));
                                log.DebugFormat("Town Control - {0} Init boss killer owningAlleg == null.  Owning monarch is no longer a monarch, behaving as if town isn't owned by anyone", town.TownName);
                            }
                        }
                        else
                        {
                            //TODO
                            //This is scenario where monarch who owned the town isn't a valid player, maybe deleted?
                            //PlayerManager.BroadcastToAll(new GameMessageSystemChat("DEBUG - owningMonarch == null.  Owning monarch is no longer a monarch, behaving as if town isn't owned by anyone", ChatMessageType.Broadcast));
                            log.DebugFormat("Town Control - {0} Init boss killer owningMonarch == null.  Owning monarch is no longer a monarch, behaving as if town isn't owned by anyone", town.TownName);
                        }
                    }
                    else
                    {
                        //When the town is not owned by any allegiance
                    }


                    //  Check if the killer is from the defending clan - if so don't start a conflict as the town owners can't start a conflict against themselves
                    if (defendingClanId.HasValue && defendingClanId.Value.Equals(killerMonarchId.Value))
                    {
                        //TODO - Send a global, or local message?
                        //TODO - manually respawn the initiation boss?
                        //PlayerManager.BroadcastToAll(new GameMessageSystemChat("DEBUG - Init boss killer is from allegiance who owns the town, conflict event will not be started", ChatMessageType.Broadcast));
                        this.CurrentLandblock?.EnqueueBroadcast(null, true, null, null, new GameMessageSystemChat($"{deadBossName} has been killed by a member of the allegiance who owns {town.TownName}.  You worthless dickbag, your name is now on the terrorist list and Im fucking coming for you.", ChatMessageType.Broadcast));
                        log.DebugFormat("Town Control - {0} Init boss killer is from clan that owns the town.  Conflict event not started", town.TownName);

                        validationFailed = true;
                    }


                    //Check if the attacking clan has started a conflict too recently in this town
                    //  get most recent conflict event for this town where this same clan was the attacker
                    //  check if respite time has elapsed - i.e. Now is > the most recent event's start + respite time for the town
                    var recentSameAttackerEvent = DatabaseManager.TownControl.GetLatestTownControlEventByAttackingMonarchId(killerMonarchId.Value, town.TownId);
                    if (recentSameAttackerEvent != null)
                    {
                        var sameAttackerRespiteExpiration = recentSameAttackerEvent.EventStartDateTime.Value.AddSeconds(town.ConflictRespiteLength.HasValue ? town.ConflictRespiteLength.Value : 0);
                        if (DateTime.UtcNow < sameAttackerRespiteExpiration)
                        {
                            //TODO - this clan has attacked this town too recently, send a global?
                            //TODO - manually respawn the initiation boss?

                            //PlayerManager.BroadcastToAll(new GameMessageSystemChat($"DEBUG - Respite timer has not passed.  Respite expires in {sameAttackerRespiteExpiration.Subtract(DateTime.UtcNow).TotalSeconds} seconds", ChatMessageType.Broadcast));
                            this.CurrentLandblock?.EnqueueBroadcast(null, true, null, null, new GameMessageSystemChat($"{deadBossName} has been killed by a member of {killerAllegName}, but Clan {killerAllegName} has attacked {town.TownName} too recently.  Your respite time expires in {(sameAttackerRespiteExpiration - DateTime.UtcNow).ToString("h'h 'm'm 's's'")}", ChatMessageType.Broadcast));
                            log.DebugFormat("Town Control - {0} Init boss killer is from clan whose respite timer is not expired.  Conflict event not started.", town.TownName);

                            validationFailed = true;
                        }
                    }

                    if(validationFailed)
                    {                        
                        //End the content generated event
                        if (TownControlLandblocks.LandblockEventsMap.TryGetValue(this.CurrentLandblock?.Id.Landblock ?? 0, out var eventName))
                        {
                            EventManager.StopEvent(eventName, null, null);
                        }

                        return;
                    }

                    //create the conflict event and update the town status in the DB
                    var tcEvent = DatabaseManager.TownControl.StartTownControlEvent(town.TownId, killerMonarchId.Value, killerAllegName, defendingClanId, defendingClanName);
                    town.IsInConflict = true;
                    DatabaseManager.TownControl.UpdateTown(town);

                    //send a global message about the conflict starting
                    string conflictStartMsg = "";

                    if (defendingClanId.HasValue)
                    {
                        conflictStartMsg = $"{killerPlayer.Name} has slain {deadBossName} throwing {town.TownName} into conflict.  A battle for town ownership has begun as clan {killerAllegName} attempts to wrest control of {town.TownName} from clan {defendingClanName}.  Will you join the fray and turn the tides of this battle?";
                    }
                    else
                    {
                        conflictStartMsg = $"{killerPlayer.Name} has slain {deadBossName} throwing {town.TownName} into conflict.  With no current owner, clan {killerAllegName} is poised to claim ownership of {town.TownName}.  Who will try to stop them?";
                    }

                    //End the content generated event
                    if (TownControlLandblocks.LandblockEventsMap.TryGetValue(this.CurrentLandblock?.Id.Landblock ?? 0, out var evtName))
                    {
                        EventManager.StartEvent(evtName, null, null);
                    }


                    PlayerManager.BroadcastToAll(new GameMessageSystemChat(conflictStartMsg, ChatMessageType.Broadcast));

                    //Send global to TC webhook
                    try
                    {
                        var webhookUrl = PropertyManager.GetString("town_control_globals_webhook").Item;
                        if (!string.IsNullOrEmpty(webhookUrl))
                        {
                            _ = TurbineChatHandler.SendWebhookedChat("God of PK", conflictStartMsg, webhookUrl, "General");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.ErrorFormat("Failed sending TownControl global message to webhook. Ex:{0}", ex);
                    }

                    //spawn the conflict bosses
                    //var tcConflictBosses = TownControlBosses.TownControlBossMap.Values.Where(x => x.BossType == TownControlBossType.ConflictBoss && x.TownID == town.TownId);

                    //foreach (var conflictBoss in tcConflictBosses)
                    //{
                    //    var bossWeenie = DatabaseManager.World.GetCachedWeenie(conflictBoss.WeenieID);
                    //    var bossWorldObj = WorldObjectFactory.CreateNewWorldObject(bossWeenie);
                    //    bossWorldObj.Location = this.Location;
                    //    bossWorldObj.Location.LandblockId = new LandblockId(this.Location.GetCell());
                    //    //bossWorldObj.CurrentLandblock = this.CurrentLandblock;
                    //    bossWorldObj.EnterWorld();
                    //}

                }
                else //If this is a Conflict boss
                {
                    //verify that a conflict event is still active for this town
                    var tcEvent = DatabaseManager.TownControl.GetLatestTownControlEventByTownId(town.TownId);

                    //End the content generated event
                    if (TownControlLandblocks.LandblockEventsMap.TryGetValue(this.CurrentLandblock?.Id.Landblock ?? 0, out var eventName))
                    {
                        EventManager.StopEvent(eventName, null, null);
                    }

                    if (!town.IsInConflict || tcEvent == null || !tcEvent.EventStartDateTime.HasValue || tcEvent.EventEndDateTime.HasValue)
                    {
                        //A conflict event isn't active
                        //TODO - broadcast something?

                        //PlayerManager.BroadcastToAll(new GameMessageSystemChat("DEBUG - conflict boss killed but no active conflict event found", ChatMessageType.Broadcast));

                        this.CurrentLandblock?.EnqueueBroadcast(null, true, null, null, new GameMessageSystemChat($"{deadBossName} has been killed but there is no active conflict event found for {town.TownName}.  {deadBossName} fucks off.", ChatMessageType.Broadcast));
                        log.DebugFormat("Town Control - {0} conflict boss killed but no active conflict event found.", town.TownName);
                        return;
                    }

                    var tcEventDurationExpiredTime = tcEvent.EventStartDateTime.Value.AddSeconds(town.ConflictLength);

                    //TODO - for future we may want multiple conflict bosses and need to track 
                    ////Check if all conflict bosses for this town are now dead and if so, end the event and determine whether the attackers succeeded within the time limit


                    string conflictEndMsg = "";

                    //If the town control event duration is not past and the boss is dead and he didn't self destruct, attackers are the winners
                    //end the TC event with attackers winning
                    if (DateTime.UtcNow < tcEventDurationExpiredTime)
                    {
                        log.DebugFormat("Town Control - {0} conflict boss killed within event duration, the attackers win.  Updating TC event, Town status and player quest stamps", town.TownName);

                        //Update the Town's owner and conflict status
                        town.CurrentOwnerID = tcEvent.AttackingClanId;
                        town.IsInConflict = false;
                        DatabaseManager.TownControl.UpdateTown(town);

                        //End the TownControlEvent
                        tcEvent.IsAttackSuccess = true;
                        tcEvent.EventEndDateTime = DateTime.UtcNow;
                        DatabaseManager.TownControl.UpdateTownControlEvent(tcEvent);

                        string tcQuestName = town.TownName.Trim().Replace(" ", "") + "TownControlOwner";

                        //Remove town's TC quest stamp from everyone online who has it
                        var charsWithQuestStamp = DatabaseManager.Shard.BaseDatabase.GetCharacterIDsWithQuestCompletion(tcQuestName);
                        if (charsWithQuestStamp != null)
                        {
                            foreach (uint charid in charsWithQuestStamp)
                            {
                                var playerWithQuest = PlayerManager.GetOnlinePlayer(charid);
                                if (playerWithQuest != null)
                                {
                                    ((Creature)playerWithQuest).QuestManager.Erase(tcQuestName);
                                }
                            }
                        }

                        //Add town's TC quest stamp to the attacking allegiance
                        var winnerAllegMembers = AllegianceManager.FindAllPlayers(new ObjectGuid(tcEvent.AttackingClanId));
                        if (winnerAllegMembers != null)
                        {
                            foreach (var allegMember in winnerAllegMembers)
                            {
                                var onlineAllegPlayer = PlayerManager.GetOnlinePlayer(allegMember.Guid);
                                if (onlineAllegPlayer != null)
                                {
                                    onlineAllegPlayer.QuestManager.Update(tcQuestName);
                                }
                            }
                        }

                        //Award bonus tears to attackers in the landblock
                        var landblockList = TownControlLandblocks.TownControlRewardLandblocksMap[town.TownId];
                        var attackerList = new List<Player>();
                        foreach (var landblockId in landblockList)
                        {
                            var landblock = LandblockManager.GetLandblock(new LandblockId(landblockId << 16), false);
                            var players = landblock.GetCurrentLandblockPlayers();
                            foreach (var player in players)
                            {
                                var playerAllegiance = AllegianceManager.GetAllegiance(player);
                                if (playerAllegiance != null && playerAllegiance.MonarchId == tcEvent.AttackingClanId)
                                {
                                    if (!attackerList.Contains(player))
                                    {
                                        attackerList.Add(player);
                                    }
                                }
                            }
                        }

                        var trophiesPerAttacker = attackerList.Count() == 0 ? 0 : town.AttackerAwardsTotal / attackerList.Count();
                        trophiesPerAttacker = trophiesPerAttacker > town.AttackerAwardsPerPerson ? town.AttackerAwardsPerPerson : trophiesPerAttacker;

                        foreach (var player in attackerList)
                        {
                            var tcTrophy = WorldObjectFactory.CreateNewWorldObject(1000002); //PK Trophy
                            tcTrophy.SetStackSize((int)trophiesPerAttacker);

                            var invCreateResult = player.TryCreateInInventoryWithNetworking(tcTrophy);
                            if (invCreateResult)
                            {
                                player.Session.Network.EnqueueSend(new GameMessageCreateObject(tcTrophy));
                                var msg = new GameMessageSystemChat($"You have received {trophiesPerAttacker} PK Trophies for successfully attacking {town.TownName}.", ChatMessageType.Broadcast);
                                player.Session.Network.EnqueueSend(msg);
                            }
                        }

                        //Send a global announcing the attackers win
                        if (tcEvent.DefendingClanId.HasValue)
                        {
                            conflictEndMsg = $"The battle for ownership of {town.TownName} has ended.  Clan {tcEvent.AttackingClanName} has slain the {deadBossName}.  {tcEvent.DefendingClanName} has failed to defend {town.TownName} from the attack of clan {tcEvent.AttackingClanName} and the attackers have prevailed in taking ownership of {town.TownName}!";
                        }
                        else
                        {
                            conflictEndMsg = $"The battle for ownership of {town.TownName} has ended.  Clan {tcEvent.AttackingClanName} has slain the {deadBossName}.  Clan {tcEvent.AttackingClanName} was victorious in their attack and are the new owners of {town.TownName}!";
                        }

                        PlayerManager.BroadcastToAll(new GameMessageSystemChat(conflictEndMsg, ChatMessageType.Broadcast));

                        //Send global to TC webhook
                        try
                        {
                            var webhookUrl = PropertyManager.GetString("town_control_globals_webhook").Item;
                            {
                                _ = TurbineChatHandler.SendWebhookedChat("God of PK", conflictEndMsg, webhookUrl, "General");
                            }
                        }
                        catch (Exception ex)
                        {
                            log.ErrorFormat("Failed sending TownControl global message to webhook. Ex:{0}", ex);
                        }
                    }
                    else
                    {
                        log.DebugFormat("Town Control - {0} conflict boss killed after event duration has passed, the defenders win.  Updating TC event and Town status", town.TownName);

                        //Update the Town's conflict status
                        town.IsInConflict = false;
                        DatabaseManager.TownControl.UpdateTown(town);

                        //End the TownControlEvent
                        tcEvent.IsAttackSuccess = false;
                        tcEvent.EventEndDateTime = DateTime.UtcNow;
                        DatabaseManager.TownControl.UpdateTownControlEvent(tcEvent);

                        //Award bonus tears to defenders in the landblock
                        var landblockList = TownControlLandblocks.TownControlRewardLandblocksMap[town.TownId];
                        var defenderList = new List<Player>();
                        foreach (var landblockId in landblockList)
                        {
                            var landblock = LandblockManager.GetLandblock(new LandblockId(landblockId << 16), false);
                            var players = landblock.GetCurrentLandblockPlayers();
                            foreach (var player in players)
                            {
                                var playerAllegiance = AllegianceManager.GetAllegiance(player);
                                if (playerAllegiance != null && playerAllegiance.MonarchId == tcEvent.DefendingClanId)
                                {
                                    if (!defenderList.Contains(player))
                                    {
                                        defenderList.Add(player);
                                    }
                                }
                            }
                        }

                        var trophiesPerDefender = defenderList.Count() == 0 ? 0 : town.DefenderAwardsTotal / defenderList.Count();
                        trophiesPerDefender = trophiesPerDefender > town.DefenderAwardsPerPerson ? town.DefenderAwardsPerPerson : trophiesPerDefender;

                        foreach (var player in defenderList)
                        {
                            var tcTrophy = WorldObjectFactory.CreateNewWorldObject(1000002); //PK Trophy
                            tcTrophy.SetStackSize((int)trophiesPerDefender);

                            var invCreateResult = player.TryCreateInInventoryWithNetworking(tcTrophy);
                            if (invCreateResult)
                            {
                                player.Session.Network.EnqueueSend(new GameMessageCreateObject(tcTrophy));
                                var msg = new GameMessageSystemChat($"You have received {trophiesPerDefender} PK Trophies for successfully defending {town.TownName}.", ChatMessageType.Broadcast);
                                player.Session.Network.EnqueueSend(msg);
                            }
                        }

                        //Send global announcing the defenders win
                        if (tcEvent.DefendingClanId.HasValue)
                        {
                            conflictEndMsg = $"The battle for ownership of {town.TownName} has ended.  Clan {tcEvent.AttackingClanName} has failed to kill {deadBossName} before the conflict period expired.  Clan {tcEvent.DefendingClanName} has successfully defended {town.TownName} from the attack of clan {tcEvent.AttackingClanName}.  The defenders retain their ownership of {town.TownName}!";
                        }
                        else
                        {
                            conflictEndMsg = $"The battle for ownership of {town.TownName} has ended.  Clan {tcEvent.AttackingClanName} has failed to kill {deadBossName} before the conflict period expired and have failed in their attack!  Ownership of {town.TownName} is still unclaimed.";
                        }

                        PlayerManager.BroadcastToAll(new GameMessageSystemChat(conflictEndMsg, ChatMessageType.Broadcast));

                        //Send global to TC webhook
                        try
                        {
                            var webhookUrl = PropertyManager.GetString("town_control_globals_webhook").Item;
                            if (!string.IsNullOrEmpty(webhookUrl))
                            {
                                _ = TurbineChatHandler.SendWebhookedChat("God of PK", conflictEndMsg, webhookUrl, "General");
                            }
                        }
                        catch (Exception ex)
                        {
                            log.ErrorFormat("Failed sending TownControl global message to webhook. Ex:{0}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Exception in Creature_Death.HandleTownControlBossDeath. Ex: {0}", ex);
            }
        }

        public void HandleWorldBossDeath()
        {
            log.Info("Creature_Death.HandleWorldBossDeath");

            try
            {
                var deadBoss = WorldBosses.WorldBossMap[this.WeenieClassId];
                var killer = DamageHistory.TopDamager;

                //var deadBossWeenie = DatabaseManager.World.GetWeenie(this.WeenieClassId);
                //var deadBossNameProp = deadBossWeenie.WeeniePropertiesString.FirstOrDefault(x => x.Type == (ushort)PropertyString.Name);
                //var deadBossName = deadBossNameProp?.Value;

                if (!killer.IsPlayer)
                {
                    //TODO - what to do if the killer isn't a player?                        
                    log.DebugFormat("World Boss - {0} killer is not a player", deadBoss.Name);  
                }

                WorldBossManager.HandleBossDeath();

                //Global broadcast the kill
                var globalMsg = $"{deadBoss.Name} has been slain by {killer.Name} and the land is once again safe from {deadBoss.Name}'s terror... for now";
                PlayerManager.BroadcastToAll(new GameMessageSystemChat(globalMsg, ChatMessageType.Broadcast));

                //Send global to webhook
                try
                {
                    var webhookUrl = PropertyManager.GetString("world_boss_webhook").Item;
                    if (!string.IsNullOrEmpty(webhookUrl))
                    {
                        _ = TurbineChatHandler.SendWebhookedChat("World Boss", globalMsg, webhookUrl, "Global");
                    }
                }
                catch (Exception ex)
                {
                    log.ErrorFormat("Failed sending World Boss Death global message to webhook. Ex:{0}", ex);
                }

                //Award bonus rewards to anyone in the landblock
                var landblockPlayers = this.CurrentLandblock.GetCurrentLandblockPlayers();
                foreach (var player in landblockPlayers)
                {
                    var pkTrophy = WorldObjectFactory.CreateNewWorldObject(1000002); //PK Trophy
                    pkTrophy.SetStackSize(5);

                    var invCreateResult = player.TryCreateInInventoryWithNetworking(pkTrophy);
                    if (invCreateResult)
                    {
                        player.Session.Network.EnqueueSend(new GameMessageCreateObject(pkTrophy));
                        var msg = new GameMessageSystemChat($"You have received 5 PK Trophies for participating in the death of the mighty {deadBoss.Name}.", ChatMessageType.Broadcast);
                        player.Session.Network.EnqueueSend(msg);
                    }
                }                        
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Exception in Creature_Death.HandleWorldBossDeath. Ex: {0}", ex);
            }
        }
    }
}
