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
using ACE.Server.Network.Motion;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public uint? DeathTreasureType
        {
            get => GetProperty(PropertyDataId.DeathTreasureType);
            set { if (!value.HasValue) RemoveProperty(PropertyDataId.DeathTreasureType); else SetProperty(PropertyDataId.DeathTreasureType, value.Value); }
        }

        public TreasureDeath DeathTreasure
        {
            get
            {
                if (DeathTreasureType.HasValue)
                    return DatabaseManager.World.GetDeathTreasure(DeathTreasureType.Value);
                else
                    return null;
            }
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
                Killer = topDamager.Guid.Full;

            // broadcast death animation
            var motionDeath = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));
            CurrentLandblock?.EnqueueBroadcastMotion(this, motionDeath);

            var dieChain = new ActionChain();

            // wait for death animation to finish
            var deathAnimLength = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.Dead);
            dieChain.AddDelaySeconds(deathAnimLength);

            dieChain.AddAction(this, () =>
            {
                NotifyOfEvent(RegenerationType.Destruction);
                LandblockManager.RemoveObject(this);
                CreateCorpse();
            });

            dieChain.EnqueueChain();
        }

        /// <summary>
        /// Called when an admin player uses the /smite command
        /// to instantly kill a creature
        /// </summary>
        public void Smite(WorldObject smiter)
        {
            Die(smiter, smiter);
        }

        /// <summary>
        /// Create a corpse for both creatures and players currently
        /// </summary>
        protected void CreateCorpse()
        {
            if (NoCorpse ?? false)
                return;

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

            corpse.Location = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationFinalPositionFromStart(Location, ObjScale ?? 1, MotionCommand.Dead);
            //corpse.Location.PositionZ = corpse.Location.PositionZ - .5f; // Adding BaseDescriptionFlags |= ObjectDescriptionFlag.Corpse to Corpse objects made them immune to gravity.. this seems to fix floating corpse...

            corpse.Name = $"Corpse of {Name}";

            // set 'killed by' for looting rights
            string killerName = null;

            if (Killer.HasValue && Killer != 0)
            {
                var killer = CurrentLandblock?.GetObject(new ObjectGuid(Killer ?? 0));

                if (killer != null)
                    killerName = killer.Name;
            }

            if (String.IsNullOrEmpty(killerName))
                killerName = "misadventure";

            corpse.LongDesc = $"Killed by {killerName}";

            if (Killer.HasValue)
                corpse.SetProperty(PropertyInstanceId.AllowedActivator, Killer.Value);

            var player = this as Player;
            if (player != null)
            {
                corpse.SetPosition(PositionType.Location, corpse.Location);
                corpse.SetDecayTime(player);

                player.CalculateDeathItems(corpse);
            }
            else
            {
                corpse.IsMonster = true;
                GenerateTreasure(corpse);
            }

            corpse.RemoveProperty(PropertyInt.Value);
            LandblockManager.AddObject(corpse);

            if (player != null)
                corpse.SaveBiotaToDatabase();
        }

        /// <summary>
        /// Transfers generated treasure from creature to corpse
        /// </summary>
        private void GenerateTreasure(Corpse corpse)
        {
            var random = new Random((int)DateTime.UtcNow.Ticks);
            int level = (int)Level;
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

            foreach (var trophy in Biota.BiotaPropertiesCreateList.Where(x => x.DestinationType == (int)DestinationType.Contain || x.DestinationType == (int)DestinationType.Treasure || x.DestinationType == (int)DestinationType.ContainTreasure || x.DestinationType == (int)DestinationType.ShopTreasure || x.DestinationType == (int)DestinationType.WieldTreasure).OrderBy(x => x.Shade))
            {
                if (random.NextDouble() < trophy.Shade || trophy.Shade == 1 || trophy.Shade == 0) // Shade in this context is Probability
                                                                                                  // Should there be rolls for each item or one roll to rule them all?
                {
                    if (trophy.WeenieClassId == 0) // Randomized Loot
                    {
                        var wo = LootGenerationFactory.CreateRandomLootObjects(tier);

                        corpse.TryAddToInventory(wo);

                        //var book = WorldObjectFactory.CreateNewWorldObject("parchment") as Book;

                        //if (book == null)
                        //    continue;

                        //book.SetProperties("IOU", "An IOU for a random loot.", "Sorry about that chief...", "ACEmulator", "prewritten");
                        //book.AddPage(corpse.Guid.Full, "ACEmulator", "prewritten", false, $"Sorry but at this time we do not have randomized and mutated loot in ACEmulator, you can ignore this item as it's meant only to be placeholder");

                        //corpse.TryAddToInventory(book);
                    }
                    else // Trophy Loot
                    {
                        var wo = WorldObjectFactory.CreateNewWorldObject(trophy.WeenieClassId);

                        if (wo == null)
                            continue;

                        if (trophy.StackSize > 1)
                            wo.StackSize = (ushort)trophy.StackSize;

                        if (trophy.Palette > 0)
                            wo.PaletteTemplate = trophy.Palette;

                        corpse.TryAddToInventory(wo);
                    }
                }
            }
        }
    }
}
