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
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameEvent.Events;
using ACE.Server.Network.GameMessages.Messages;

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

            QuestManager.OnDeath(lastDamager?.TryGetAttacker());
            
            if (!IsOnNoDeathXPLandblock)
                OnDeath_GrantXP();

            return GetDeathMessage(lastDamager, damageType, criticalHit);
        }


        public DeathMessage GetDeathMessage(DamageHistoryInfo lastDamager, DamageType damageType, bool criticalHit = false)
        {
            var deathMessage = Strings.GetDeathMessage(damageType, criticalHit);

            if (lastDamager == null || lastDamager.Guid == Guid)
                return Strings.General[1];

            // if killed by a player, send them a message
            if (lastDamager.IsPlayer)
            {
                if (criticalHit && this is Player)
                    deathMessage = Strings.PKCritical[0];

                var killerMsg = string.Format(deathMessage.Killer, Name);

                var playerKiller = lastDamager.TryGetAttacker() as Player;

                if (playerKiller != null)
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

            EmoteManager.OnDeath(lastDamager?.TryGetAttacker());

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

                // should this be passed upstream to fellowship / allegiance?
                if (playerDamager.AugmentationBonusXp > 0)
                    totalXP *= 1.0f + playerDamager.AugmentationBonusXp * 0.05f;

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
        /// Create a corpse for both creatures and players currently
        /// </summary>
        protected void CreateCorpse(DamageHistoryInfo killer)
        {
            if (NoCorpse)
            {
                var loot = GenerateTreasure(killer, null);

                foreach(var item in loot)
                {
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
                var dropped = player.CalculateDeathItems(corpse);
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
                    corpse.PkLevel = PKLevel.PK;

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
                GenerateTreasure(killer, corpse);

                if (killer != null && killer.IsPlayer)
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
                List<WorldObject> items = LootGenerationFactory.CreateRandomLootObjects(DeathTreasure);
                foreach (WorldObject wo in items)
                {
                    if (corpse != null)
                        corpse.TryAddToInventory(wo);
                    else
                        droppedItems.Add(wo);

                    DoCantripLogging(killer, wo);
                }
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

            return droppedItems;
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
    }
}
