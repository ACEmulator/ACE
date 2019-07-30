using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.Database.Models.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public TreasureDeath DeathTreasure { get => DeathTreasureType.HasValue ? DatabaseManager.World.GetCachedDeathTreasure(DeathTreasureType.Value) : null; }

        /// <summary>
        /// Called when a monster or player dies, in conjunction with Die()
        /// </summary>
        /// <param name="lastDamager">The last damager that landed the death blow</param>
        /// <param name="damageType">The damage type for the death message</param>
        /// <param name="criticalHit">True if the death blow was a critical hit, generates a critical death message</param>
        public virtual DeathMessage OnDeath(WorldObject lastDamager, DamageType damageType, bool criticalHit = false)
        {
            IsTurning = false;
            IsMoving = false;

            EmoteManager.OnDeath(DamageHistory);

            OnDeath_GrantXP();

            if (IsGenerator)
                OnGeneratorDeath();

            return GetDeathMessage(lastDamager, damageType, criticalHit);
        }


        public DeathMessage GetDeathMessage(WorldObject lastDamager, DamageType damageType, bool criticalHit = false)
        {
            var deathMessage = Strings.GetDeathMessage(damageType, criticalHit);

            if (lastDamager == null || lastDamager == this)
                deathMessage = Strings.General[1];

            // if killed by a player, send them a message
            var playerKiller = lastDamager as Player;
            if (playerKiller != null && playerKiller != this)
            {
                if (criticalHit && this is Player)
                    deathMessage = Strings.PKCritical[0];

                var killerMsg = string.Format(deathMessage.Killer, Name);

                // todo: verify message type
                playerKiller.Session.Network.EnqueueSend(new GameMessageSystemChat(killerMsg, ChatMessageType.Broadcast));
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

        /// <summary>
        /// Performs the full death sequence for non-Player creatures
        /// </summary>
        protected virtual void Die(WorldObject lastDamager, WorldObject topDamager)
        {
            UpdateVital(Health, 0);

            if (topDamager != null)
                KillerId = topDamager.Guid.Full;

            CurrentMotionState = new Motion(MotionStance.NonCombat, MotionCommand.Ready);
            //IsMonster = false;

            // broadcast death animation
            var motionDeath = new Motion(MotionStance.NonCombat, MotionCommand.Dead);
            var deathAnimLength = ExecuteMotion(motionDeath);

            var dieChain = new ActionChain();

            // wait for death animation to finish
            //var deathAnimLength = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.Dead);
            dieChain.AddDelaySeconds(deathAnimLength);

            dieChain.AddAction(this, () =>
            {
                Destroy();
                CreateCorpse(topDamager);
            });

            dieChain.EnqueueChain();
        }

        /// <summary>
        /// Called when an admin player uses the /smite command
        /// to instantly kill a creature
        /// </summary>
        public void Smite(WorldObject smiter)
        {
            // deal remaining damage?
            OnDeath();
            Die(smiter, smiter);
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

            foreach (var kvp in DamageHistory.TotalDamage)
            {
                var damager = kvp.Value.TryGetWorldObject();
                var totalDamage = kvp.Value.Value;

                var playerDamager = damager as Player;
                if (playerDamager == null)
                {
                    var petDamager = damager as CombatPet;
                    if (petDamager == null)
                        continue;

                    playerDamager = petDamager.P_PetOwner;
                    if (playerDamager == null)
                        continue;
                }

                var damagePercent = totalDamage / Health.MaxValue;
                var totalXP = (XpOverride ?? 0) * damagePercent;

                // should this be passed upstream to fellowship / allegiance?
                if (playerDamager.AugmentationBonusXp > 0)
                    totalXP *= 1.0f + playerDamager.AugmentationBonusXp * 0.05f;

                playerDamager.EarnXP((long)Math.Round(totalXP), XpType.Kill);
            }
        }

        /// <summary>
        /// Create a corpse for both creatures and players currently
        /// </summary>
        protected void CreateCorpse(WorldObject killer)
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

            var corpse = WorldObjectFactory.CreateNewWorldObject(DatabaseManager.World.GetCachedWeenie("corpse")) as Corpse;
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

                byte i = 0;
                foreach (var animPartChange in objDesc.AnimPartChanges)
                    corpse.Biota.BiotaPropertiesAnimPart.Add(new Database.Models.Shard.BiotaPropertiesAnimPart { ObjectId = corpse.Guid.Full, AnimationId = animPartChange.PartID, Index = animPartChange.PartIndex, Order = i++ });

                foreach (var subPalette in objDesc.SubPalettes)
                    corpse.Biota.BiotaPropertiesPalette.Add(new Database.Models.Shard.BiotaPropertiesPalette { ObjectId = corpse.Guid.Full, SubPaletteId = subPalette.SubID, Length = (ushort)subPalette.NumColors, Offset = (ushort)subPalette.Offset });

                i = 0;
                foreach (var textureChange in objDesc.TextureChanges)
                    corpse.Biota.BiotaPropertiesTextureMap.Add(new Database.Models.Shard.BiotaPropertiesTextureMap { ObjectId = corpse.Guid.Full, Index = textureChange.PartIndex, OldId = textureChange.OldTexture, NewId = textureChange.NewTexture, Order = i++ });
            }

            corpse.Location = new Position(Location);

            corpse.VictimId = Guid.Full;
            corpse.Name = $"{prefix} of {Name}";

            // set 'killed by' for looting rights
            if (killer != null)
            {
                corpse.LongDesc = $"Killed by {killer.Name.TrimStart('+')}.";  // vtank requires + to be stripped for regex matching.
                if (killer is CombatPet)
                    corpse.KillerId = killer.PetOwner.Value;
                else
                    corpse.KillerId = killer.Guid.Full;
            }
            else
                corpse.LongDesc = $"Killed by misadventure.";

            bool saveCorpse = false;

            if (this is Player player)
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
            }
            else
            {
                corpse.IsMonster = true;
                GenerateTreasure(killer, corpse);
                if (killer is Player && (Level >= 100 || Level >= killer.Level + 5))
                {
                    CanGenerateRare = true;
                }
            }

            corpse.RemoveProperty(PropertyInt.Value);

            if (CanGenerateRare && killer != null)
                corpse.GenerateRare(killer);

            corpse.EnterWorld();

            if (this is Player p)
            {
                if (corpse.PhysicsObj == null || corpse.PhysicsObj.Position == null)
                    log.Info($"{Name}'s corpse (0x{corpse.Guid}) failed to spawn! Tried at {p.Location.ToLOCString()}");
                else
                    log.Info($"{Name}'s corpse (0x{corpse.Guid}) is located at {corpse.PhysicsObj.Position}");
            }

            if (saveCorpse)
                corpse.SaveBiotaToDatabase();
        }

        public bool CanGenerateRare
        {
            get => GetProperty(PropertyBool.CanGenerateRare) ?? false;
            set { if (!value) RemoveProperty(PropertyBool.CanGenerateRare); else SetProperty(PropertyBool.CanGenerateRare, value); }
        }

        /// <summary>
        /// Transfers generated treasure from creature to corpse
        /// </summary>
        private List<WorldObject> GenerateTreasure(WorldObject killer, Corpse corpse)
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
            var createList = Biota.BiotaPropertiesCreateList.Where(i => (i.DestinationType & (int)DestinationType.Contain) != 0 ||
                (i.DestinationType & (int)DestinationType.Treasure) != 0 && (i.DestinationType & (int)DestinationType.Wield) == 0).ToList();

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

            // move wielded treasure over
            var wieldedTreasure = Inventory.Values.Concat(EquippedObjects.Values).Where(i => i.DestinationType.HasFlag(DestinationType.Treasure));
            foreach (var item in wieldedTreasure.ToList())
            {
                if ((item.Bonded ?? 0) == (int)BondedStatus.Destroy)
                    continue;

                if (TryDequipObjectWithBroadcasting(item.Guid, out var wo, out var wieldedLocation))
                    TryAddToInventory(wo);

                if (corpse != null)
                    corpse.TryAddToInventory(item);
                else
                    droppedItems.Add(item);
            }

            return droppedItems;
        }

        public void DoCantripLogging(WorldObject killer, WorldObject wo)
        {
            var epicCantrips = wo.EpicCantrips;
            var legendaryCantrips = wo.LegendaryCantrips;

            if (epicCantrips.Count > 0)
                log.Info($"[EPIC] {Name} ({Guid}) generated item with {epicCantrips.Count} epic{(epicCantrips.Count > 1 ? "s" : "")} - {wo.Name} ({wo.Guid}) - {GetSpellList(epicCantrips)} - killed by {killer.Name} ({killer.Guid})");

            if (legendaryCantrips.Count > 0)
                log.Info($"[LEGENDARY] {Name} ({Guid}) generated item with {legendaryCantrips.Count} legendar{(legendaryCantrips.Count > 1 ? "ies" : "y")} - {wo.Name} ({wo.Guid}) - {GetSpellList(legendaryCantrips)} - killed by {killer.Name} ({killer.Guid})");
        }

        public static string GetSpellList(List<BiotaPropertiesSpellBook> spellbook)
        {
            var spells = new List<Server.Entity.Spell>();

            foreach (var spell in spellbook)
                spells.Add(new Server.Entity.Spell(spell.Spell, false));

            return string.Join(", ", spells.Select(i => i.Name));
        }
    }
}
