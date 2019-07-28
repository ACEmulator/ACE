using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using ACE.Server.Entity.Actions;
using ACE.Server.Managers;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Structure;

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public void HandleActionWorldBroadcast(string message, ChatMessageType messageType)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(this, () => DoWorldBroadcast(message, messageType));
            chain.EnqueueChain();
        }

        public void DoWorldBroadcast(string message, ChatMessageType messageType)
        {
            GameMessageSystemChat sysMessage = new GameMessageSystemChat(message, messageType);

            PlayerManager.BroadcastToAll(sysMessage);
        }

        public override ACE.Entity.ObjDesc CalculateObjDesc()
        {
            ACE.Entity.ObjDesc objDesc = new ACE.Entity.ObjDesc();
            ClothingTable item;

            AddBaseModelData(objDesc);

            var coverage = new List<uint>();

            uint thisSetupId = SetupTableId;
            bool showHelm;
            bool showCloak;
            if (this is Player player)
            {
                showHelm = player.GetCharacterOption(CharacterOption.ShowYourHelmOrHeadGear);
                showCloak = player.GetCharacterOption(CharacterOption.ShowYourCloak);

                // Some player races use an AlternateSetupDid, either at creation of via Barber options.
                // BUT -- those values do not correspond with entries in the Clothing Table.
                // So, we need to make some adjustments to look up something that DOES exist and is appropriate for the AlternateSetup model.
                switch (thisSetupId)
                {
                    case (uint)SetupConst.UndeadMaleSkeleton:
                    case (uint)SetupConst.UndeadMaleSkeletonNoflame:
                    case (uint)SetupConst.UndeadMaleZombie:
                    case (uint)SetupConst.UndeadMaleZombieNoflame:
                        thisSetupId = (uint)SetupConst.UndeadMaleUndead;
                        break;
                    case (uint)SetupConst.UndeadFemaleSkeleton:
                    case (uint)SetupConst.UndeadFemaleSkeletonNoflame:
                    case (uint)SetupConst.UndeadFemaleZombie:
                    case (uint)SetupConst.UndeadFemaleZombieNoflame:
                        thisSetupId = (uint)SetupConst.UndeadFemaleUndead;
                        break;
                    case (uint)SetupConst.PenumbraenMaleNocrown:
                        thisSetupId = (uint)SetupConst.PenumbraenMaleCrown;
                        break;
                    case (uint)SetupConst.PenumbraenFemaleNocrown:
                        thisSetupId = (uint)SetupConst.PenumbraenFemaleCrown;
                        break;
                }
            }
            else
            {
                showHelm = true;
                showCloak = true;
            }

            // get all the Armor Items so we can calculate their priority
            var armorItems = EquippedObjects.Values.Where(x => (x.CurrentWieldedLocation & EquipMask.Armor) != 0).ToList();
            foreach (var w in armorItems)
                w.setVisualClothingPriority();

            // sort the armor into the proper order... TopLayerPriority first, then no priority, then TopLayerPriority=false.
            // Secondary sort field is the calculated "VisualClothingPriority"
            var top = armorItems.Where(x => x.TopLayerPriority == true).OrderBy(x => x.VisualClothingPriority);
            var noLayer = armorItems.Where(x => x.TopLayerPriority == null).OrderBy(x => x.VisualClothingPriority);
            var bottom = armorItems.Where(x => x.TopLayerPriority == false).OrderBy(x => x.VisualClothingPriority);
            var sortedArmorItems = bottom.Concat(noLayer).Concat(top).ToList();

            // Clothing and Cloaks do not have a tailoring/reduction issue to worry about. What you see is what you get, so we can use ClothingPriority only to get this data.
            var clothesAndCloaks = EquippedObjects.Values
                                .Where(x => (x.CurrentWieldedLocation & (EquipMask.Clothing | EquipMask.Cloak)) != 0)
                                .Where(x => (x.CurrentWieldedLocation & (EquipMask.FootWear)) == 0) // FootWear is included in the ArmorItems above
                                .OrderBy(x => x.ClothingPriority);

            var eo = clothesAndCloaks.Concat(sortedArmorItems).ToList();

            if (eo.Count == 0)
            {
                // Check if there is any defined ObjDesc in the Biota and, if so, apply them
                if (Biota.BiotaPropertiesAnimPart.Count > 0 || Biota.BiotaPropertiesPalette.Count > 0 || Biota.BiotaPropertiesTextureMap.Count > 0)
                {
                    foreach (var animPart in Biota.BiotaPropertiesAnimPart.OrderBy(b => b.Order))
                        objDesc.AnimPartChanges.Add(new ACE.Entity.AnimationPartChange { PartIndex = animPart.Index, PartID = animPart.AnimationId });

                    foreach (var subPalette in Biota.BiotaPropertiesPalette)
                        objDesc.SubPalettes.Add(new ACE.Entity.SubPalette { SubID = subPalette.SubPaletteId, Offset = subPalette.Offset, NumColors = subPalette.Length });

                    foreach (var textureMap in Biota.BiotaPropertiesTextureMap.OrderBy(b => b.Order))
                        objDesc.TextureChanges.Add(new ACE.Entity.TextureMapChange { PartIndex = textureMap.Index, OldTexture = textureMap.OldId, NewTexture = textureMap.NewId });

                    return objDesc;
                }
            }

            foreach (var w in eo)
            {
                if ((w.CurrentWieldedLocation == EquipMask.HeadWear) && !showHelm && (this is Player))
                    continue;

                if ((w.CurrentWieldedLocation == EquipMask.Cloak) && !showCloak && (this is Player))
                    continue;

                // We can wield things that are not part of our model, only use those items that can cover our model.
                if ((w.CurrentWieldedLocation & (EquipMask.Clothing | EquipMask.Armor | EquipMask.Cloak)) != 0)
                {
                    if (w.ClothingBase.HasValue)
                        item = DatManager.PortalDat.ReadFromDat<ClothingTable>((uint)w.ClothingBase);
                    else
                    {
                        objDesc = AddSetupAsClothingBase(objDesc, w);
                        // Add any potentially added parts back into the coverage list
                        foreach(var a in objDesc.AnimPartChanges)
                            if (!coverage.Contains(a.PartIndex))
                                coverage.Add(a.PartIndex);
                        continue;
                    }

                    if (item.ClothingBaseEffects.ContainsKey(thisSetupId))
                    // Check if the player model has data. Gear Knights, this is usually you.
                    {
                        // Add the model and texture(s)
                        ClothingBaseEffect clothingBaseEffect = item.ClothingBaseEffects[thisSetupId];
                        foreach (CloObjectEffect t in clothingBaseEffect.CloObjectEffects)
                        {
                            byte partNum = (byte)t.Index;
                            if (objDesc.AnimPartChanges.FirstOrDefault(c => c.PartIndex == (byte)t.Index && c.PartID == t.ModelId) == null)
                                objDesc.AnimPartChanges.Add(new ACE.Entity.AnimationPartChange { PartIndex = (byte)t.Index, PartID = t.ModelId });
                            //AddModel((byte)t.Index, (ushort)t.ModelId);
                            coverage.Add(partNum);
                            foreach (CloTextureEffect t1 in t.CloTextureEffects)
                            {
                                if (objDesc.TextureChanges.FirstOrDefault(c => c.PartIndex == (byte)t.Index && c.OldTexture == t1.OldTexture && c.NewTexture == t1.NewTexture) == null)
                                    objDesc.TextureChanges.Add(new ACE.Entity.TextureMapChange { PartIndex = (byte)t.Index, OldTexture = t1.OldTexture, NewTexture = t1.NewTexture });
                            }
                        }

                        if (item.ClothingSubPalEffects.Count > 0)
                        {
                            int size = item.ClothingSubPalEffects.Count;
                            int palCount = size;

                            CloSubPalEffect itemSubPal;
                            int palOption = 0;
                            if (w.PaletteTemplate.HasValue)
                                palOption = (int)w.PaletteTemplate;
                            if (item.ClothingSubPalEffects.ContainsKey((uint)palOption))
                            {
                                itemSubPal = item.ClothingSubPalEffects[(uint)palOption];
                            }
                            else
                            {
                                itemSubPal = item.ClothingSubPalEffects[item.ClothingSubPalEffects.Keys.ElementAt(0)];
                            }

                            float shade = 0;
                            if (w.Shade.HasValue)
                                shade = (float)w.Shade;
                            for (int i = 0; i < itemSubPal.CloSubPalettes.Count; i++)
                            {
                                var itemPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(itemSubPal.CloSubPalettes[i].PaletteSet);
                                ushort itemPal = (ushort)itemPalSet.GetPaletteID(shade);

                                for (int j = 0; j < itemSubPal.CloSubPalettes[i].Ranges.Count; j++)
                                {
                                    uint palOffset = itemSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
                                    uint numColors = itemSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
                                    objDesc.SubPalettes.Add(new ACE.Entity.SubPalette { SubID = itemPal, Offset = palOffset, NumColors = numColors });
                                }
                            }
                        }
                    }
                }
            }

            // Add the "naked" body parts. These are the ones not already covered.
            // Note that this is the original SetupTableId, not thisSetupId.
            if (SetupTableId > 0)
            {
                var baseSetup = DatManager.PortalDat.ReadFromDat<SetupModel>(SetupTableId);
                for (byte i = 0; i < baseSetup.Parts.Count; i++)
                {
                    if (!coverage.Contains(i) && i != 0x10) // Don't add body parts for those that are already covered. Also don't add the head, that was already covered by AddCharacterBaseModelData()
                        objDesc.AnimPartChanges.Add(new ACE.Entity.AnimationPartChange { PartIndex = i, PartID = baseSetup.Parts[i] });
                    //AddModel(i, baseSetup.Parts[i]);
                }
            }

            if (coverage.Count == 0 && ClothingBase.HasValue)
                return base.CalculateObjDesc();

            return objDesc;
        }

        /// <summary>
        /// Certain items do not contain a ClothingBase. Ursuin Guise, WCID 32155 is one of them. This function will use the Setup of the weenie as a pseudo-ClothingBase.
        /// </summary>
        protected ACE.Entity.ObjDesc AddSetupAsClothingBase(ACE.Entity.ObjDesc objDesc, WorldObject wo)
        {
            // Loop over the parts in the Setup of the WorldObject
            for (var i = 0; i < wo.CSetup.Parts.Count; i++)
            {
                if(wo.CSetup.Parts[i] != 0x010001EC || i != 16) // This is essentially a "null" part, so do not add it for the head
                    objDesc.AnimPartChanges.Add(new ACE.Entity.AnimationPartChange { PartIndex = (byte)i, PartID = wo.CSetup.Parts[i] });
            }

            return objDesc;
        }


        protected static void WriteIdentifyObjectCreatureProfile(BinaryWriter writer, Creature creature, bool success)
        {
            var creatureProfile = new CreatureProfile(creature, success);
            writer.Write(creatureProfile);
        }
    }
}
