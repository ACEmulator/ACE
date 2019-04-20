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

            bool showHelm = true;
            bool showCloak = true;

            uint thisSetupId = SetupTableId;

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

            var eo = EquippedObjects.Values.Where(x => (x.CurrentWieldedLocation & (EquipMask.Clothing | EquipMask.Armor | EquipMask.Cloak)) != 0).OrderBy(x => x.ClothingPriority).ToList();

            if (eo.Count == 0)
            {
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
                        continue;

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
                            //AddTexture((byte)t.Index, (ushort)t1.OldTexture, (ushort)t1.NewTexture);
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

                            //if (itemSubPal.Icon > 0 && !(IgnoreCloIcons ?? false))
                            //    IconId = itemSubPal.Icon;

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
                                    //AddPalette(itemPal, (ushort)palOffset, (ushort)numColors);
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

            /*var p = this as Player;
            if (p != null)
            {
                Console.WriteLine("AnimPart changes:");
                Console.WriteLine("PartIndex\tPartID\n====================================");
                foreach (var animPartChange in objDesc.AnimPartChanges)
                    Console.WriteLine(animPartChange.PartIndex + "\t" + animPartChange.PartID.ToString("X8"));

                Console.WriteLine("TextureMap changes:");
                Console.WriteLine("PartIndex\tOldTex\tNewTex\n====================================");
                foreach (var texChange in objDesc.TextureChanges)
                    Console.WriteLine(texChange.PartIndex + "\t" + texChange.OldTexture.ToString("X8") + "\t" + texChange.NewTexture.ToString("X8"));
            }*/

            return objDesc;
        }

        protected static void WriteIdentifyObjectCreatureProfile(BinaryWriter writer, Creature creature, bool success)
        {
            var creatureProfile = new CreatureProfile(creature, success);
            writer.Write(creatureProfile);
        }
    }
}
