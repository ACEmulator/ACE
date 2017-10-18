using System;
using System.Linq;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.GameEvent.Events;
using ACE.Managers;
using ACE.Entity.Enum.Properties;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader.Entity;
using System.Collections.Generic;
using ACE.Factories;

namespace ACE.Network.Handlers
{
    public static class CharacterHandler
    {
        [GameMessageAttribute(GameMessageOpcode.CharacterEnterWorldRequest, SessionState.AuthConnected)]
        public static void CharacterEnterWorldRequest(ClientMessage message, Session session)
        {
            session.Network.EnqueueSend(new GameMessageCharacterEnterWorldServerReady());
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterEnterWorld, SessionState.AuthConnected)]
        public static void CharacterEnterWorld(ClientMessage message, Session session)
        {
            ObjectGuid guid = message.Payload.ReadGuid();

            string account = message.Payload.ReadString16L();

            if (account != session.Account)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            var cachedCharacter = session.AccountCharacters.SingleOrDefault(c => c.Guid.Full == guid.Full);
            if (cachedCharacter == null)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            session.CharacterRequested = cachedCharacter;

            session.InitSessionForWorldLogin();

            session.State = SessionState.WorldConnected;

            LandblockManager.PlayerEnterWorld(session, cachedCharacter.Guid);
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterDelete, SessionState.AuthConnected)]
        public static void CharacterDelete(ClientMessage message, Session session)
        {
            string account = message.Payload.ReadString16L();
            uint characterSlot = message.Payload.ReadUInt32();

            if (account != session.Account)
            {
                session.SendCharacterError(CharacterError.Delete);
                return;
            }

            var cachedCharacter = session.AccountCharacters.SingleOrDefault(c => c.SlotId == characterSlot);
            if (cachedCharacter == null)
            {
                session.SendCharacterError(CharacterError.Delete);
                return;
            }

            // TODO: check if character is already pending removal

            session.Network.EnqueueSend(new GameMessageCharacterDelete());

            DatabaseManager.Shard.DeleteOrRestore(Time.GetUnixTime() + 3600ul, cachedCharacter.Guid.Full, ((bool deleteOrRestoreSuccess) =>
            {
                if (deleteOrRestoreSuccess)
                {
                    DatabaseManager.Shard.GetCharacters(session.Id, ((List<CachedCharacter> result) =>
                    {
                        session.UpdateCachedCharacters(result);
                        session.Network.EnqueueSend(new GameMessageCharacterList(result, session.Account));
                    }));
                }
                else
                {
                    session.SendCharacterError(CharacterError.Delete);
                }
            }));
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterRestore, SessionState.AuthConnected)]
        public static void CharacterRestore(ClientMessage message, Session session)
        {
            ObjectGuid guid = message.Payload.ReadGuid();

            var cachedCharacter = session.AccountCharacters.SingleOrDefault(c => c.Guid.Full == guid.Full);
            if (cachedCharacter == null)
                return;

            DatabaseManager.Shard.IsCharacterNameAvailable(cachedCharacter.Name, ((bool isAvailable) =>
            {
                if (!isAvailable)
                {
                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);
                }
                else
                {
                    DatabaseManager.Shard.DeleteOrRestore(0, cachedCharacter.Guid.Full, ((bool deleteOrRestoreSuccess) =>
                    {
                        if (deleteOrRestoreSuccess)
                            session.Network.EnqueueSend(new GameMessageCharacterRestore(guid, cachedCharacter.Name, 0u));
                        else
                            SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Corrupt);
                    }));
                }
            }));
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterCreate, SessionState.AuthConnected)]
        public static void CharacterCreate(ClientMessage message, Session session)
        {
            string account = message.Payload.ReadString16L();
            if (account != session.Account)
                return;

            uint id = GuidManager.NewPlayerGuid().Full;

            CharacterCreateEx(message, session, id);
        }

        private static void CharacterCreateEx(ClientMessage message, Session session, uint id)
        {
            CharGen cg = CharGen.ReadFromDat();
            var reader = message.Payload;
            AceCharacter character = new AceCharacter(id);

            reader.Skip(4);   /* Unknown constant (1) */
            character.Heritage = reader.ReadUInt32();
            character.HeritageGroup = cg.HeritageGroups[(int)character.Heritage].Name;
            character.Gender = reader.ReadUInt32();
            if (character.Gender == 1)
                character.Sex = "Male";
            else
                character.Sex = "Female";
            Appearance appearance = Appearance.FromNetwork(reader);

            // character.IconId = cg.HeritageGroups[(int)character.Heritage].IconImage;

            // pull character data from the dat file
            SexCG sex = cg.HeritageGroups[(int)character.Heritage].SexList[(int)character.Gender];

            character.MotionTableId = sex.MotionTable;
            character.SoundTableId = sex.SoundTable;
            character.PhysicsTableId = sex.PhysicsTable;
            character.SetupTableId = sex.SetupID;
            character.PaletteId = sex.BasePalette;
            character.CombatTableId = sex.CombatTable;

            // Check the character scale
            if (sex.Scale != 100u)
            {
                character.DefaultScale = (sex.Scale / 100f); // Scale is stored as a percentage
            }

            // Get the hair first, because we need to know if you're bald, and that's the name of that tune!
            HairStyleCG hairstyle = sex.HairStyleList[Convert.ToInt32(appearance.HairStyle)];
            bool isBald = hairstyle.Bald;

            // Certain races (Undead, Tumeroks, Others?) have multiple body styles available. This is controlled via the "hair style".
            if (hairstyle.AlternateSetup > 0)
                character.SetupTableId = hairstyle.AlternateSetup;

            character.EyesTexture = sex.GetEyeTexture(appearance.Eyes, isBald);
            character.DefaultEyesTexture = sex.GetDefaultEyeTexture(appearance.Eyes, isBald);
            character.NoseTexture = sex.GetNoseTexture(appearance.Nose);
            character.DefaultNoseTexture = sex.GetDefaultNoseTexture(appearance.Nose);
            character.MouthTexture = sex.GetMouthTexture(appearance.Mouth);
            character.DefaultMouthTexture = sex.GetDefaultMouthTexture(appearance.Mouth);
            character.HairTexture = sex.GetHairTexture(appearance.HairStyle);
            character.DefaultHairTexture = sex.GetDefaultHairTexture(appearance.HairStyle);
            character.HeadObject = sex.GetHeadObject(appearance.HairStyle);

            // Skin is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
            PaletteSet skinPalSet = PaletteSet.ReadFromDat(sex.SkinPalSet);
            character.SkinPalette = skinPalSet.GetPaletteID(appearance.SkinHue);

            // Hair is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
            PaletteSet hairPalSet = PaletteSet.ReadFromDat(sex.HairColorList[Convert.ToInt32(appearance.HairColor)]);
            character.HairPalette = hairPalSet.GetPaletteID(appearance.HairHue);

            // Eye Color
            character.EyesPalette = sex.EyeColorList[Convert.ToInt32(appearance.EyeColor)];

            if (appearance.HeadgearStyle < 0xFFFFFFFF) // No headgear is max UINT
            {
                uint headgearWeenie = sex.GetHeadgearWeenie(appearance.HeadgearStyle);
                ClothingTable headCT = ClothingTable.ReadFromDat(sex.GetHeadgearClothingTable(appearance.HeadgearStyle));
                uint headgearIconId = headCT.GetIcon(appearance.HeadgearColor);

                var hat = (AceObject)DatabaseManager.World.GetAceObjectByWeenie(headgearWeenie).Clone(GuidManager.NewItemGuid().Full);
                hat.PaletteOverrides = new List<PaletteOverride>(); // wipe any existing overrides
                hat.TextureOverrides = new List<TextureMapOverride>();
                hat.AnimationOverrides = new List<AnimationOverride>();
                hat.SpellIdProperties = new List<AceObjectPropertiesSpell>();
                hat.IconDID = headgearIconId;
                hat.Placement = 0;
                hat.CurrentWieldedLocation = hat.ValidLocations;
                hat.WielderIID = id;

                if (headCT.ClothingBaseEffects.ContainsKey(sex.SetupID))
                {
                    // Add the model and texture(s)
                    ClothingBaseEffect headCBE = headCT.ClothingBaseEffects[sex.SetupID];
                    for (int i = 0; i < headCBE.CloObjectEffects.Count; i++)
                    {
                        byte partNum = (byte)headCBE.CloObjectEffects[i].Index;
                        hat.AnimationOverrides.Add(new AnimationOverride()
                        {
                            AceObjectId = hat.AceObjectId,
                            AnimationId = headCBE.CloObjectEffects[i].ModelId,
                            Index = (byte)headCBE.CloObjectEffects[i].Index
                        });

                        for (int j = 0; j < headCBE.CloObjectEffects[i].CloTextureEffects.Count; j++)
                        {
                            hat.TextureOverrides.Add(new TextureMapOverride()
                            {
                                AceObjectId = hat.AceObjectId,
                                Index = (byte)headCBE.CloObjectEffects[i].Index,
                                OldId = (ushort)headCBE.CloObjectEffects[i].CloTextureEffects[j].OldTexture,
                                NewId = (ushort)headCBE.CloObjectEffects[i].CloTextureEffects[j].NewTexture
                            });
                        }
                    }

                    // Apply the proper palette(s). Unlike character skin/hair, clothes can have several palette ranges!
                    CloSubPalEffect headSubPal = headCT.ClothingSubPalEffects[appearance.HeadgearColor];
                    for (int i = 0; i < headSubPal.CloSubPalettes.Count; i++)
                    {
                        PaletteSet headgearPalSet = PaletteSet.ReadFromDat(headSubPal.CloSubPalettes[i].PaletteSet);
                        ushort headgearPal = (ushort)headgearPalSet.GetPaletteID(appearance.HeadgearHue);

                        for (int j = 0; j < headSubPal.CloSubPalettes[i].Ranges.Count; j++)
                        {
                            uint palOffset = headSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
                            uint numColors = headSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
                            hat.PaletteOverrides.Add(new PaletteOverride()
                            {
                                AceObjectId = hat.AceObjectId,
                                SubPaletteId = headgearPal,
                                Length = (ushort)(numColors),
                                Offset = (ushort)(palOffset)
                            });
                        }
                    }
                }

                character.WieldedItems.Add(new ObjectGuid(hat.AceObjectId), hat);
            }

            uint shirtWeenie = sex.GetShirtWeenie(appearance.ShirtStyle);
            ClothingTable shirtCT = ClothingTable.ReadFromDat(sex.GetShirtClothingTable(appearance.ShirtStyle));
            uint shirtIconId = shirtCT.GetIcon(appearance.ShirtColor);

            var shirt = (AceObject)DatabaseManager.World.GetAceObjectByWeenie(shirtWeenie).Clone(GuidManager.NewItemGuid().Full);
            shirt.PaletteOverrides = new List<PaletteOverride>(); // wipe any existing overrides
            shirt.TextureOverrides = new List<TextureMapOverride>();
            shirt.AnimationOverrides = new List<AnimationOverride>();
            shirt.SpellIdProperties = new List<AceObjectPropertiesSpell>();
            shirt.IconDID = shirtIconId;
            shirt.Placement = 0;
            shirt.CurrentWieldedLocation = shirt.ValidLocations;
            shirt.WielderIID = id;

            if (shirtCT.ClothingBaseEffects.ContainsKey(sex.SetupID))
            {
                ClothingBaseEffect shirtCBE = shirtCT.ClothingBaseEffects[sex.SetupID];
                for (int i = 0; i < shirtCBE.CloObjectEffects.Count; i++)
                {
                    byte partNum = (byte)shirtCBE.CloObjectEffects[i].Index;
                    shirt.AnimationOverrides.Add(new AnimationOverride()
                    {
                        AceObjectId = shirt.AceObjectId,
                        AnimationId = shirtCBE.CloObjectEffects[i].ModelId,
                        Index = (byte)shirtCBE.CloObjectEffects[i].Index
                    });

                    for (int j = 0; j < shirtCBE.CloObjectEffects[i].CloTextureEffects.Count; j++)
                    {
                        shirt.TextureOverrides.Add(new TextureMapOverride()
                        {
                            AceObjectId = shirt.AceObjectId,
                            Index = (byte)shirtCBE.CloObjectEffects[i].Index,
                            OldId = (ushort)shirtCBE.CloObjectEffects[i].CloTextureEffects[j].OldTexture,
                            NewId = (ushort)shirtCBE.CloObjectEffects[i].CloTextureEffects[j].NewTexture
                        });
                    }
                }

                // Apply the proper palette(s). Unlike character skin/hair, clothes can have several palette ranges!
                if (shirtCT.ClothingSubPalEffects.ContainsKey(appearance.ShirtColor))
                {
                    CloSubPalEffect shirtSubPal = shirtCT.ClothingSubPalEffects[appearance.ShirtColor];
                    for (int i = 0; i < shirtSubPal.CloSubPalettes.Count; i++)
                    {
                        PaletteSet shirtPalSet = PaletteSet.ReadFromDat(shirtSubPal.CloSubPalettes[i].PaletteSet);
                        ushort shirtPal = (ushort)shirtPalSet.GetPaletteID(appearance.ShirtHue);

                        if (shirtPal > 0) // shirtPal will be 0 if the palette set is empty/not found
                        {
                            for (int j = 0; j < shirtSubPal.CloSubPalettes[i].Ranges.Count; j++)
                            {
                                uint palOffset = shirtSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
                                uint numColors = shirtSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
                                shirt.PaletteOverrides.Add(new PaletteOverride()
                                {
                                    AceObjectId = shirt.AceObjectId,
                                    SubPaletteId = shirtPal,
                                    Offset = (ushort)palOffset,
                                    Length = (ushort)numColors
                                });
                            }
                        }
                    }
                }
            }

            character.WieldedItems.Add(new ObjectGuid(shirt.AceObjectId), shirt);

            uint pantsWeenie = sex.GetPantsWeenie(appearance.PantsStyle);
            ClothingTable pantsCT = ClothingTable.ReadFromDat(sex.GetPantsClothingTable(appearance.PantsStyle));
            uint pantsIconId = pantsCT.GetIcon(appearance.PantsColor);

            var pants = (AceObject)DatabaseManager.World.GetAceObjectByWeenie(pantsWeenie).Clone(GuidManager.NewItemGuid().Full);
            pants.PaletteOverrides = new List<PaletteOverride>(); // wipe any existing overrides
            pants.TextureOverrides = new List<TextureMapOverride>();
            pants.AnimationOverrides = new List<AnimationOverride>();
            pants.SpellIdProperties = new List<AceObjectPropertiesSpell>();
            pants.IconDID = pantsIconId;
            pants.Placement = 0;
            pants.CurrentWieldedLocation = pants.ValidLocations;
            pants.WielderIID = id;

            // Get the character's initial pants
            if (pantsCT.ClothingBaseEffects.ContainsKey(sex.SetupID))
            {
                ClothingBaseEffect pantsCBE = pantsCT.ClothingBaseEffects[sex.SetupID];
                for (int i = 0; i < pantsCBE.CloObjectEffects.Count; i++)
                {
                    byte partNum = (byte)pantsCBE.CloObjectEffects[i].Index;
                    pants.AnimationOverrides.Add(new AnimationOverride()
                    {
                        AceObjectId = pants.AceObjectId,
                        AnimationId = pantsCBE.CloObjectEffects[i].ModelId,
                        Index = (byte)pantsCBE.CloObjectEffects[i].Index
                    });

                    for (int j = 0; j < pantsCBE.CloObjectEffects[i].CloTextureEffects.Count; j++)
                    {
                        pants.TextureOverrides.Add(new TextureMapOverride()
                        {
                            AceObjectId = pants.AceObjectId,
                            Index = (byte)pantsCBE.CloObjectEffects[i].Index,
                            OldId = (ushort)pantsCBE.CloObjectEffects[i].CloTextureEffects[j].OldTexture,
                            NewId = (ushort)pantsCBE.CloObjectEffects[i].CloTextureEffects[j].NewTexture
                        });
                    }
                }

                // Apply the proper palette(s). Unlike character skin/hair, clothes can have several palette ranges!
                CloSubPalEffect pantsSubPal = pantsCT.ClothingSubPalEffects[appearance.PantsColor];
                for (int i = 0; i < pantsSubPal.CloSubPalettes.Count; i++)
                {
                    PaletteSet pantsPalSet = PaletteSet.ReadFromDat(pantsSubPal.CloSubPalettes[i].PaletteSet);
                    ushort pantsPal = (ushort)pantsPalSet.GetPaletteID(appearance.PantsHue);

                    for (int j = 0; j < pantsSubPal.CloSubPalettes[i].Ranges.Count; j++)
                    {
                        uint palOffset = pantsSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
                        uint numColors = pantsSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
                        pants.PaletteOverrides.Add(new PaletteOverride()
                        {
                            AceObjectId = pants.AceObjectId,
                            SubPaletteId = pantsPal,
                            Offset = (ushort)palOffset,
                            Length = (ushort)numColors
                        });
                    }
                }
            } // end pants

            character.WieldedItems.Add(new ObjectGuid(pants.AceObjectId), pants);

            uint footwearWeenie = sex.GetFootwearWeenie(appearance.FootwearStyle);
            ClothingTable footwearCT = ClothingTable.ReadFromDat(sex.GetFootwearClothingTable(appearance.FootwearStyle));
            uint footwearIconId = footwearCT.GetIcon(appearance.FootwearColor);

            var shoes = (AceObject)DatabaseManager.World.GetAceObjectByWeenie(footwearWeenie).Clone(GuidManager.NewItemGuid().Full);
            shoes.PaletteOverrides = new List<PaletteOverride>(); // wipe any existing overrides
            shoes.TextureOverrides = new List<TextureMapOverride>();
            shoes.AnimationOverrides = new List<AnimationOverride>();
            shoes.SpellIdProperties = new List<AceObjectPropertiesSpell>();
            shoes.IconDID = footwearIconId;
            shoes.Placement = 0;
            shoes.CurrentWieldedLocation = shoes.ValidLocations;
            shoes.WielderIID = id;

            if (footwearCT.ClothingBaseEffects.ContainsKey(sex.SetupID))
            {
                ClothingBaseEffect footwearCBE = footwearCT.ClothingBaseEffects[sex.SetupID];
                for (int i = 0; i < footwearCBE.CloObjectEffects.Count; i++)
                {
                    byte partNum = (byte)footwearCBE.CloObjectEffects[i].Index;
                    shoes.AnimationOverrides.Add(new AnimationOverride()
                    {
                        AceObjectId = shoes.AceObjectId,
                        AnimationId = footwearCBE.CloObjectEffects[i].ModelId,
                        Index = (byte)footwearCBE.CloObjectEffects[i].Index
                    });

                    for (int j = 0; j < footwearCBE.CloObjectEffects[i].CloTextureEffects.Count; j++)
                    {
                        shoes.TextureOverrides.Add(new TextureMapOverride()
                        {
                            AceObjectId = shoes.AceObjectId,
                            Index = (byte)footwearCBE.CloObjectEffects[i].Index,
                            OldId = (ushort)footwearCBE.CloObjectEffects[i].CloTextureEffects[j].OldTexture,
                            NewId = (ushort)footwearCBE.CloObjectEffects[i].CloTextureEffects[j].NewTexture
                        });
                    }
                }

                // Apply the proper palette(s). Unlike character skin/hair, clothes can have several palette ranges!
                CloSubPalEffect footwearSubPal = footwearCT.ClothingSubPalEffects[appearance.FootwearColor];
                for (int i = 0; i < footwearSubPal.CloSubPalettes.Count; i++)
                {
                    PaletteSet footwearPalSet = PaletteSet.ReadFromDat(footwearSubPal.CloSubPalettes[i].PaletteSet);
                    ushort footwearPal = (ushort)footwearPalSet.GetPaletteID(appearance.FootwearHue);

                    for (int j = 0; j < footwearSubPal.CloSubPalettes[i].Ranges.Count; j++)
                    {
                        uint palOffset = footwearSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
                        uint numColors = footwearSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
                        pants.PaletteOverrides.Add(new PaletteOverride()
                        {
                            AceObjectId = shoes.AceObjectId,
                            SubPaletteId = footwearPal,
                            Offset = (ushort)palOffset,
                            Length = (ushort)numColors
                        });
                    }
                }
            } // end footwear

            character.WieldedItems.Add(new ObjectGuid(shoes.AceObjectId), shoes);

            // Profession (Adventurer, Bow Hunter, etc)
            // TODO - Add this title to the available titles for this character.
            var templateOption = reader.ReadInt32();
            string templateName = cg.HeritageGroups[(int)character.Heritage].TemplateList[templateOption].Name;
            character.Title = templateName;
            character.Template = templateName;
            character.CharacterTitleId = cg.HeritageGroups[(int)character.Heritage].TemplateList[templateOption].Title;
            character.NumCharacterTitles = 1;

            // stats
            uint totalAttributeCredits = cg.HeritageGroups[(int)character.Heritage].AttributeCredits;
            uint usedAttributeCredits = 0;
            // Validate this is equal to actual attribute credits (330 for all but "Olthoi", which have 60
            character.StrengthAbility.Base = ValidateAttributeCredits(reader.ReadUInt32(), usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += character.StrengthAbility.Base;

            character.EnduranceAbility.Base = ValidateAttributeCredits(reader.ReadUInt32(), usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += character.EnduranceAbility.Base;

            character.CoordinationAbility.Base = ValidateAttributeCredits(reader.ReadUInt32(), usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += character.CoordinationAbility.Base;

            character.QuicknessAbility.Base = ValidateAttributeCredits(reader.ReadUInt32(), usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += character.QuicknessAbility.Base;

            character.FocusAbility.Base = ValidateAttributeCredits(reader.ReadUInt32(), usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += character.FocusAbility.Base;

            character.SelfAbility.Base = ValidateAttributeCredits(reader.ReadUInt32(), usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += character.SelfAbility.Base;

            // data we don't care about
            uint characterSlot = reader.ReadUInt32();
            uint classId = reader.ReadUInt32();

            // characters start with max vitals
            character.Health.Current = character.Health.MaxValue;
            character.Stamina.Current = character.Stamina.MaxValue;
            character.Mana.Current = character.Mana.MaxValue;

            // set initial skill credit amount. 52 for all but "Olthoi", which have 68
            character.AvailableSkillCredits = cg.HeritageGroups[(int)character.Heritage].SkillCredits;

            uint numOfSkills = reader.ReadUInt32();
            Skill skill;
            SkillStatus skillStatus;
            SkillCostAttribute skillCost;
            for (uint i = 0; i < numOfSkills; i++)
            {
                skill = (Skill)i;
                skillCost = skill.GetCost();
                skillStatus = (SkillStatus)reader.ReadUInt32();

                if (skillStatus == SkillStatus.Specialized)
                {
                    character.TrainSkill(skill, skillCost.TrainingCost);
                    character.SpecializeSkill(skill, skillCost.SpecializationCost);
                    // oddly enough, specialized skills don't get any free ranks like trained do
                }
                if (skillStatus == SkillStatus.Trained)
                {
                    character.TrainSkill(skill, skillCost.TrainingCost);
                    character.AceObjectPropertiesSkills[skill].Ranks = 5;
                    character.AceObjectPropertiesSkills[skill].ExperienceSpent = 526;
                }
                if (skillCost != null && skillStatus == SkillStatus.Untrained)
                    character.UntrainSkill(skill, skillCost.TrainingCost);
            }

            // grant starter items based on skills
            var starterGearConfig = StarterGearFactory.GetStarterGearConfiguration();
            List<uint> grantedItems = new List<uint>();

            foreach (var skillGear in starterGearConfig.Skills)
            {
                var charSkill = character.AceObjectPropertiesSkills[(Skill)skillGear.SkillId];
                if (charSkill.Status == SkillStatus.Trained || charSkill.Status == SkillStatus.Specialized)
                {
                    foreach (var item in skillGear.Gear)
                    {
                        if (grantedItems.Contains(item.WeenieId))
                        {
                            var existingItem = character.Inventory.Values.FirstOrDefault(i => i.WeenieClassId == item.WeenieId);
                            if ((existingItem?.MaxStackSize ?? 1) <= 1)
                                continue;

                            existingItem.StackSize += item.StackSize;
                            continue;
                        }

                        var loot = (AceObject)DatabaseManager.World.GetAceObjectByWeenie(item.WeenieId).Clone(GuidManager.NewItemGuid().Full);
                        loot.Placement = 0;
                        loot.ContainerIID = id;
                        loot.StackSize = item.StackSize > 1 ? (ushort?)item.StackSize : null;
                        character.Inventory.Add(new ObjectGuid(loot.AceObjectId), loot);
                        grantedItems.Add(item.WeenieId);
                    }

                    var heritageLoot = skillGear.Heritage.FirstOrDefault(sh => sh.HeritageId == character.Heritage);
                    if (heritageLoot != null)
                    {
                        foreach (var item in heritageLoot.Gear)
                        {
                            if (grantedItems.Contains(item.WeenieId))
                            {
                                var existingItem = character.Inventory.Values.FirstOrDefault(i => i.WeenieClassId == item.WeenieId);
                                if ((existingItem?.MaxStackSize ?? 1) <= 1)
                                    continue;

                                existingItem.StackSize += item.StackSize;
                                continue;
                            }

                            var loot = (AceObject)DatabaseManager.World.GetAceObjectByWeenie(item.WeenieId).Clone(GuidManager.NewItemGuid().Full);
                            loot.Placement = 0;
                            loot.ContainerIID = id;
                            loot.StackSize = item.StackSize > 1 ? (ushort?)item.StackSize : null;
                            character.Inventory.Add(new ObjectGuid(loot.AceObjectId), loot);
                            grantedItems.Add(item.WeenieId);
                        }
                    }

                    foreach (var spell in skillGear.Spells)
                    {
                        character.SpellIdProperties.Add(new AceObjectPropertiesSpell() { AceObjectId = id, SpellId = spell.SpellId });
                    }
                }
            }

            character.Name = reader.ReadString16L();
            character.DisplayName = character.Name; // unsure

            // currently not used
            uint startArea = reader.ReadUInt32();

            character.IsAdmin = Convert.ToBoolean(reader.ReadUInt32());
            character.IsEnvoy = Convert.ToBoolean(reader.ReadUInt32());

            DatabaseManager.Shard.IsCharacterNameAvailable(character.Name, ((bool isAvailable) =>
            {
                if (!isAvailable)
                {
                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);
                    return;
                }

                character.AccountId = session.Id;

                CharacterCreateSetDefaultCharacterOptions(character);
                CharacterCreateSetDefaultCharacterPositions(character, startArea);

                // We must await here -- 
                DatabaseManager.Shard.SaveObject(character, ((bool saveSuccess) =>
                {
                    if (!saveSuccess)
                    {
                        SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.DatabaseDown);
                        return;
                    }
                    // DatabaseManager.Shard.SaveCharacterOptions(character);
                    // DatabaseManager.Shard.InitCharacterPositions(character);

                    var guid = new ObjectGuid(character.AceObjectId);
                    session.AccountCharacters.Add(new CachedCharacter(guid, (byte)session.AccountCharacters.Count, character.Name, 0));

                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Ok, guid, character.Name);
                }));
            }));
        }

        /// <summary>
        /// Checks if the total credits is more than this class is allowed.
        /// </summary>
        /// <returns>The original value or the max allowed.</returns>
        private static ushort ValidateAttributeCredits(uint attributeValue, uint allAttributes, uint maxAttributes)
        {
            if ((attributeValue + allAttributes) > maxAttributes)
            {
                return (ushort)(maxAttributes - allAttributes);
            }
            else
                return (ushort)attributeValue;
        }

        private static void CharacterCreateSetDefaultCharacterOptions(AceCharacter character)
        {
            character.SetCharacterOption(CharacterOption.VividTargetingIndicator, true);
            character.SetCharacterOption(CharacterOption.Display3dTooltips, true);
            character.SetCharacterOption(CharacterOption.ShowCoordinatesByTheRadar, true);
            character.SetCharacterOption(CharacterOption.DisplaySpellDurations, true);
            character.SetCharacterOption(CharacterOption.IgnoreFellowshipRequests, true);
            character.SetCharacterOption(CharacterOption.ShareFellowshipExpAndLuminance, true);
            character.SetCharacterOption(CharacterOption.LetOtherPlayersGiveYouItems, true);
            character.SetCharacterOption(CharacterOption.RunAsDefaultMovement, true);
            character.SetCharacterOption(CharacterOption.AutoTarget, true);
            character.SetCharacterOption(CharacterOption.AutoRepeatAttacks, true);
            character.SetCharacterOption(CharacterOption.UseChargeAttack, true);
            character.SetCharacterOption(CharacterOption.LeadMissileTargets, true);
            character.SetCharacterOption(CharacterOption.ListenToAllegianceChat, true);
            character.SetCharacterOption(CharacterOption.ListenToGeneralChat, true);
            character.SetCharacterOption(CharacterOption.ListenToTradeChat, true);
            character.SetCharacterOption(CharacterOption.ListenToLFGChat, true);
        }

        public static void CharacterCreateSetDefaultCharacterPositions(AceCharacter character, uint startArea)
        {
            character.Location = CharacterPositionExtensions.StartingPosition(startArea);
        }

        private static void SendCharacterCreateResponse(Session session, CharacterGenerationVerificationResponse response, ObjectGuid guid = default(ObjectGuid), string charName = "")
        {
            session.Network.EnqueueSend(new GameMessageCharacterCreateResponse(response, guid, charName));
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterLogOff, SessionState.WorldConnected)]
        public static void CharacterLogOff(ClientMessage message, Session session)
        {
            session.LogOffPlayer();
        }
    }
}
