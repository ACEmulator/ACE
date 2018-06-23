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

namespace ACE.Server.WorldObjects
{
    partial class Creature
    {
        public override void SerializeIdentifyObjectResponse(BinaryWriter writer, bool success, IdentifyResponseFlags flags = IdentifyResponseFlags.None)
        {
            bool hideCreatureProfile = NpcLooksLikeObject ?? false;

            if (!hideCreatureProfile)
                flags |= IdentifyResponseFlags.CreatureProfile;

            base.SerializeIdentifyObjectResponse(writer, success, flags);

            if (!hideCreatureProfile)
                WriteIdentifyObjectCreatureProfile(writer, this, success);
        }

        protected static void WriteIdentifyObjectCreatureProfile(BinaryWriter writer, Creature creature, bool success)
        {
            uint header = 0;

            // TODO: for now, we are always succeeding - will need to set this to 0 header for failure.   Og II
            if (success)
                header = 8;

            writer.Write(header);
            writer.Write(creature.Health.Current);
            writer.Write(creature.Health.MaxValue);
            if (header == 0)
            {
                for (int i = 0; i < 10; i++)
                    writer.Write(0u);
            }
            else
            {
                // TODO: we probably need buffed values here  it may be set my the last flag I don't understand yet. - will need to revisit. Og II
                writer.Write(creature.Strength.Base);
                writer.Write(creature.Endurance.Base);
                writer.Write(creature.Quickness.Base);
                writer.Write(creature.Coordination.Base);
                writer.Write(creature.Focus.Base);
                writer.Write(creature.Self.Base);
                writer.Write(creature.Stamina.Current);
                writer.Write(creature.Mana.Current);
                writer.Write(creature.Stamina.MaxValue);
                writer.Write(creature.Mana.MaxValue);
                // this only gets sent if the header can be masked with 1
                // Writer.Write(0u);
            }
        }

        public void HandleActionWorldBroadcast(string message, ChatMessageType messageType)
        {
            ActionChain chain = new ActionChain();
            chain.AddAction(this, () => DoWorldBroadcast(message, messageType));
            chain.EnqueueChain();
        }

        public void DoWorldBroadcast(string message, ChatMessageType messageType)
        {
            GameMessageSystemChat sysMessage = new GameMessageSystemChat(message, messageType);

            WorldManager.BroadcastToAll(sysMessage);
        }

        public override ACE.Entity.ObjDesc CalculateObjDesc()
        {
            ACE.Entity.ObjDesc objDesc = new ACE.Entity.ObjDesc();
            ClothingTable item;

            AddBaseModelData(objDesc);

            var coverage = new List<uint>();

            bool showHelm = true;
            bool showCloak = true;

            if (this is Player player)
            { 
                var characterOptions2 = player.CharacterOptions2Mapping ?? 0;

                showHelm = (characterOptions2 & (int)CharacterOptions2.ShowYourHelmOrHeadGear) == (int)CharacterOptions2.ShowYourHelmOrHeadGear;
                showCloak = (characterOptions2 & (int)CharacterOptions2.ShowYourCloak) == (int)CharacterOptions2.ShowYourCloak;
            }

            foreach (var w in EquippedObjects.Values.Where(x => (x.CurrentWieldedLocation & (EquipMask.Clothing | EquipMask.Armor | EquipMask.Cloak)) != 0).OrderBy(x => x.Priority))
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

                    if (item.ClothingBaseEffects.ContainsKey(SetupTableId))
                    // Check if the player model has data. Gear Knights, this is usually you.
                    {
                        // Add the model and texture(s)
                        ClothingBaseEffect clothingBaseEffect = item.ClothingBaseEffects[SetupTableId];
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
    }
}
