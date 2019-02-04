using System;
using System.Linq;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using System.Collections.Generic;

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

            return GetDeathMessage(lastDamager, damageType, criticalHit);
        }


        public DeathMessage GetDeathMessage(WorldObject lastDamager, DamageType damageType, bool criticalHit = false)
        {
            var deathMessage = Strings.GetDeathMessage(damageType, criticalHit);

            if (lastDamager == this)
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
            IsMonster = false;

            // broadcast death animation
            var motionDeath = new Motion(MotionStance.NonCombat, MotionCommand.Dead);
            var deathAnimLength = ExecuteMotion(motionDeath);

            var dieChain = new ActionChain();

            // wait for death animation to finish
            //var deathAnimLength = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.Dead);
            dieChain.AddDelaySeconds(deathAnimLength);

            dieChain.AddAction(this, () =>
            {
                NotifyOfEvent(RegenerationType.Destruction);
                LandblockManager.RemoveObject(this);
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
                var damager = kvp.Key;
                var totalDamage = kvp.Value;

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

                if (playerDamager.AugmentationBonusXp > 0)
                    totalXP *= 1.0f + playerDamager.AugmentationBonusXp * 0.05f;

                playerDamager.EarnXP((long)Math.Round(totalXP));
            }
        }

        /// <summary>
        /// Create a corpse for both creatures and players currently
        /// </summary>
        protected void CreateCorpse(WorldObject killer)
        {
            if (NoCorpse) return;

            var corpse = WorldObjectFactory.CreateNewWorldObject(DatabaseManager.World.GetCachedWeenie("corpse")) as Corpse;

            corpse.SetupTableId = SetupTableId;
            corpse.MotionTableId = MotionTableId;
            corpse.SoundTableId = SoundTableId;
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

            if (EquippedObjects.Values.Where(x => (x.CurrentWieldedLocation & (EquipMask.Clothing | EquipMask.Armor | EquipMask.Cloak)) != 0).ToList().Count > 0) // If creature is wearing objects, we need to save the appearance
            {
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

            //corpse.Location = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationFinalPositionFromStart(Location, ObjScale ?? 1, MotionCommand.Dead);

            //corpse.Location.SetLandblock();
            //corpse.Location.SetLandCell();

            corpse.Location = new Position(Location);

            //corpse.Location.PositionZ = corpse.Location.PositionZ - .5f; // Adding BaseDescriptionFlags |= ObjectDescriptionFlag.Corpse to Corpse objects made them immune to gravity.. this seems to fix floating corpse...

            corpse.VictimId = Guid.Full;
            corpse.Name = $"Corpse of {Name}";

            // set 'killed by' for looting rights
            if (killer != null)
            {
                corpse.LongDesc = $"Killed by {killer.Name}.";
                if (killer is CombatPet)
                    corpse.KillerId = killer.PetOwner.Value;
                else
                    corpse.KillerId = killer.Guid.Full;
            }
            else
                corpse.LongDesc = $"Killed by misadventure.";

            var player = this as Player;
            if (player != null)
            {
                corpse.SetPosition(PositionType.Location, corpse.Location);
                var dropped = player.CalculateDeathItems(corpse);
                corpse.RecalculateDecayTime(player);

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
                GenerateTreasure(corpse);
            }

            corpse.RemoveProperty(PropertyInt.Value);
            LandblockManager.AddObject(corpse);
        }

        /// <summary>
        /// Transfers generated treasure from creature to corpse
        /// </summary>
        private void GenerateTreasure(Corpse corpse)
        {
            var random = new Random((int)DateTime.UtcNow.Ticks);
            int level = Level ?? 0;
            int tier;
            if (level < 16)
            {
                tier = 1;
            }
            else if (level < 31)
            {
                tier = 2;
            }
            else if (level < 60)
            {
                tier = 3;
            }
            else if (level < 80)
            {
                tier = 4;
            }
            else if (level < 115)
            {
                tier = 5;
            }
            else if (level < 160)
            {
                tier = 6;
            }
            else
            {
                tier = 7;
            }
            ////Tier 8 is reserved for special creatures, usually based on which landblock they were on...Not level based. to be added later

            if (DeathTreasure?.Tier > 0)
                tier = DeathTreasure.Tier;
            var deathTreasure = this.DeathTreasure;
            if (deathTreasure != null)
            {
                List<WorldObject> items = LootGenerationFactory.CreateRandomLootObjects(deathTreasure);
                foreach (WorldObject wo in items)
                {
                    TryAddToInventory(wo);
                }
            }

            foreach (var trophy in Biota.BiotaPropertiesCreateList.Where(x => x.DestinationType == (int)DestinationType.Contain || x.DestinationType == (int)DestinationType.Treasure || x.DestinationType == (int)DestinationType.ContainTreasure || x.DestinationType == (int)DestinationType.ShopTreasure || x.DestinationType == (int)DestinationType.WieldTreasure).OrderBy(x => x.Shade))
            {
                if (random.NextDouble() < trophy.Shade || trophy.Shade == 1 || trophy.Shade == 0) // Shade in this context is Probability
                                                                                                  // Should there be rolls for each item or one roll to rule them all?
                {
                    //if (trophy.WeenieClassId == 0) // Randomized Loot
                    //{
                    //    var items = LootGenerationFactory.CreateRandomLootObjects(deathTreasure);

                    //    corpse.TryAddToInventory(wo);

                    //    //var book = WorldObjectFactory.CreateNewWorldObject("parchment") as Book;

                    //    //if (book == null)
                    //    //    continue;

                    //    //book.SetProperties("IOU", "An IOU for a random loot.", "Sorry about that chief...", "ACEmulator", "prewritten");
                    //    //book.AddPage(corpse.Guid.Full, "ACEmulator", "prewritten", false, $"Sorry but at this time we do not have randomized and mutated loot in ACEmulator, you can ignore this item as it's meant only to be placeholder");

                    //    //corpse.TryAddToInventory(book);
                    //}
                    //else // Trophy Loot
                    //{
                        var wo = WorldObjectFactory.CreateNewWorldObject(trophy.WeenieClassId);

                        if (wo == null)
                            continue;

                        if (trophy.StackSize > 1)
                            wo.StackSize = (ushort)trophy.StackSize;

                        if (trophy.Palette > 0)
                            wo.PaletteTemplate = trophy.Palette;

                        corpse.TryAddToInventory(wo);
                   // }
                }
            }

            if (Level > 3 && !Name.Equals("Chicken"))
            {
                var numItems = ThreadSafeRandom.Next(DeathTreasure.ItemMinAmount, DeathTreasure.ItemMaxAmount);
                for (int i = 0; i < numItems; i++)
                {
                    if (ThreadSafeRandom.Next(0, 100) <= DeathTreasure.ItemChance)
                    {
                        var wo = LootGenerationFactory.CreateRandomLootObjects(tier, false);
                        corpse.TryAddToInventory(wo);
                    }
                }

                numItems = ThreadSafeRandom.Next(DeathTreasure.MagicItemMinAmount, DeathTreasure.MagicItemMaxAmount);
                for (int i = 0; i < numItems; i++)
                {
                    if (ThreadSafeRandom.Next(0, 100) <= DeathTreasure.MagicItemChance)
                    {
                        var wo = LootGenerationFactory.CreateRandomLootObjects(tier, true);
                        corpse.TryAddToInventory(wo);
                    }
                }

                numItems = ThreadSafeRandom.Next(DeathTreasure.MundaneItemMinAmount, DeathTreasure.MundaneItemMaxAmount);
                for (int i = 0; i < numItems; i++)
                {
                    if (ThreadSafeRandom.Next(0, 100) <= DeathTreasure.MundaneItemChance)
                    {
                        var wo = LootGenerationFactory.CreateMundaneObjects(tier);
                        corpse.TryAddToInventory(wo);
                    }
                }
            }
        }
    }
}
