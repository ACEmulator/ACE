using ACE.Database;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.Motion;
using System;
using System.Linq;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        private static readonly UniversalMotion dead = new UniversalMotion(MotionStance.Standing, new MotionItem(MotionCommand.Dead));

        public void Die()
        {
            ActionChain dieChain = new ActionChain();
            dieChain.AddAction(this, () =>
            {
                CurrentLandblock.EnqueueBroadcastMotion(this, dead);
            });
            dieChain.AddDelaySeconds(DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationLength(MotionCommand.Dead));
            dieChain.AddAction(this, () =>
            {
                NotifyOfEvent(RegenerationType.Destruction);
                LandblockManager.RemoveObject(this);
                PhysicsObj.leave_cell(false);
                PhysicsObj.remove_shadows_from_cells();
                CreateCorpse();
            });
            dieChain.EnqueueChain();
        }

        public uint? Killer
        {
            get => GetProperty(PropertyInstanceId.Killer);
            set { if (!value.HasValue) RemoveProperty(PropertyInstanceId.Killer); else SetProperty(PropertyInstanceId.Killer, value.Value); }
        }

        public void Smite(ObjectGuid smiter)
        {
            //SetProperty(PropertyInstanceId.CurrentAttacker, smiter.Full);
            //SetProperty(PropertyInstanceId.CurrentDamager, smiter.Full);
            //Health.Current = 0;
            Killer = smiter.Full;
            Die();
        }

        public bool? NoCorpse
        {
            get => GetProperty(PropertyBool.NoCorpse);
            set { if (!value.HasValue) RemoveProperty(PropertyBool.NoCorpse); else SetProperty(PropertyBool.NoCorpse, value.Value); }
        }

        public void CreateCorpse()
        {
            if (!(NoCorpse ?? false))
            {
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
                //    corpse.Translucency = Translucency;

                if (EquippedObjects.Values.Where(x => (x.CurrentWieldedLocation & (EquipMask.Clothing | EquipMask.Armor | EquipMask.Cloak)) != 0).ToList().Count > 0) // If creature is wearing objects, we need to save the appearance
                {
                    var objDesc = CalculateObjDesc();

                    foreach (var animPartChange in objDesc.AnimPartChanges)
                        corpse.Biota.BiotaPropertiesAnimPart.Add(new Database.Models.Shard.BiotaPropertiesAnimPart { ObjectId = corpse.Guid.Full, AnimationId = animPartChange.PartID, Index = animPartChange.PartIndex });

                    foreach (var subPalette in objDesc.SubPalettes)
                        corpse.Biota.BiotaPropertiesPalette.Add(new Database.Models.Shard.BiotaPropertiesPalette { ObjectId = corpse.Guid.Full, SubPaletteId = subPalette.SubID, Length = (ushort)subPalette.NumColors, Offset = (ushort)subPalette.Offset });

                    foreach (var textureChange in objDesc.TextureChanges)
                        corpse.Biota.BiotaPropertiesTextureMap.Add(new Database.Models.Shard.BiotaPropertiesTextureMap { ObjectId = corpse.Guid.Full, Index = textureChange.PartIndex, OldId = textureChange.OldTexture, NewId = textureChange.NewTexture });
                }

                //corpse.Location = Location;
                corpse.Location = DatManager.PortalDat.ReadFromDat<MotionTable>(MotionTableId).GetAnimationFinalPositionFromStart(Location, ObjScale ?? 1, MotionCommand.Dead);
                corpse.Location.PositionZ = corpse.Location.PositionZ - .5f; // Adding BaseDescriptionFlags |= ObjectDescriptionFlag.Corpse to Corpse objects made them immune to gravity.. this seems to fix floating corpse...

                corpse.Name = $"Corpse of {Name}";

                var killerName = CurrentLandblock.GetObject(new ObjectGuid(Killer ?? 0)).Name; // this might need to be rethought....

                if (killerName == "")
                    killerName = "misadventure";

                corpse.LongDesc = $"Killed by {killerName}";
                corpse.SetProperty(PropertyInstanceId.AllowedActivator, Killer.Value); // Think this will be what limits corpses to Killer first.

                // Transfer of generated treasure from creature to corpse here

                var random = new Random((int)DateTime.UtcNow.Ticks);

                foreach (var trophy in Biota.BiotaPropertiesCreateList.Where(x => x.DestinationType == (int)DestinationType.Contain || x.DestinationType == (int)DestinationType.Treasure || x.DestinationType == (int)DestinationType.ContainTreasure || x.DestinationType == (int)DestinationType.ShopTreasure || x.DestinationType == (int)DestinationType.WieldTreasure).OrderBy(x => x.Shade))
                {
                    if (random.NextDouble() < trophy.Shade || trophy.Shade == 1 || trophy.Shade == 0) // Shade in this context is Probability
                                                                                                      // Should there be rolls for each item or one roll to rule them all?
                    {
                        if (trophy.WeenieClassId == 0) // Randomized Loot
                        {
                            //var wo = WorldObjectFactory.CreateNewWorldObject(trophy.WeenieClassId);

                            //corpse.TryAddToInventory(wo);

                            var book = WorldObjectFactory.CreateNewWorldObject("parchment") as Book;

                            if (book == null)
                                continue;

                            book.SetProperties("IOU", "An IOU for a random loot.", "Sorry about that chief...", "ACEmulator", "prewritten");
                            book.AddPage(corpse.Guid.Full, "ACEmulator", "prewritten", false, $"Sorry but at this time we do not have randomized and mutated loot in ACEmulator, you can ignore this item as it's meant only to be placeholder");

                            corpse.TryAddToInventory(book);
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

                LandblockManager.AddObject(corpse);
            }
        }
    }
}
