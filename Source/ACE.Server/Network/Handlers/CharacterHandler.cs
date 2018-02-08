using System;
using System.Collections.Generic;
using System.Linq;
using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;

namespace ACE.Server.Network.Handlers
{
    public static class CharacterHandler
    {
        [GameMessage(GameMessageOpcode.CharacterEnterWorldRequest, SessionState.AuthConnected)]
        public static void CharacterEnterWorldRequest(ClientMessage message, Session session)
        {
            session.Network.EnqueueSend(new GameMessageCharacterEnterWorldServerReady());
        }

        [GameMessage(GameMessageOpcode.CharacterEnterWorld, SessionState.AuthConnected)]
        public static void CharacterEnterWorld(ClientMessage message, Session session)
        {
            ObjectGuid guid = message.Payload.ReadGuid();

            string clientString = message.Payload.ReadString16L();

            if (clientString != session.Account)
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

        [GameMessage(GameMessageOpcode.CharacterDelete, SessionState.AuthConnected)]
        public static void CharacterDelete(ClientMessage message, Session session)
        {
            string clientString = message.Payload.ReadString16L();
            uint characterSlot = message.Payload.ReadUInt32();
            
            if (clientString != session.Account)
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

        [GameMessage(GameMessageOpcode.CharacterRestore, SessionState.AuthConnected)]
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

        [GameMessage(GameMessageOpcode.CharacterCreate, SessionState.AuthConnected)]
        public static void CharacterCreate(ClientMessage message, Session session)
        {
            string clientString = message.Payload.ReadString16L();
            if (clientString != session.Account)
                return;

            uint id = GuidManager.NewPlayerGuid().Full;

            CharacterCreateEx(message, session, id);
        }

        private static void CharacterCreateEx(ClientMessage message, Session session, uint id)
        {
            var cg = DatManager.PortalDat.CharGen;
            var reader = message.Payload;
            AceCharacter character = new AceCharacter(id);

            reader.Skip(4);   /* Unknown constant (1) */
            character.Heritage = (int)reader.ReadUInt32();

            // Disable OlthoiPlay characters for now. They're not implemented yet.
            // FIXME: Restore OlthoiPlay characters when properly handled.
            if (character.Heritage == (int)HeritageGroup.Olthoi || character.Heritage == (int)HeritageGroup.OlthoiAcid)
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Pending);
                return;
            }

            character.HeritageGroup = cg.HeritageGroups[(uint)character.Heritage].Name;
            character.Gender = (int)reader.ReadUInt32();
            if (character.Gender == 1)
                character.Sex = "Male";
            else
                character.Sex = "Female";
            Appearance appearance = Appearance.FromNetwork(reader);

            // character.IconId = cg.HeritageGroups[(int)character.Heritage].IconImage;

            // pull character data from the dat file
            SexCG sex = cg.HeritageGroups[(uint)character.Heritage].Genders[(int)character.Gender];

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
            var skinPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(sex.SkinPalSet);
            character.SkinPalette = skinPalSet.GetPaletteID(appearance.SkinHue);
            character.Shade = appearance.SkinHue;

            // Hair is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
            var hairPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(sex.HairColorList[Convert.ToInt32(appearance.HairColor)]);
            character.HairPalette = hairPalSet.GetPaletteID(appearance.HairHue);

            // Eye Color
            character.EyesPalette = sex.EyeColorList[Convert.ToInt32(appearance.EyeColor)];

            if (appearance.HeadgearStyle < 0xFFFFFFFF) // No headgear is max UINT
            {
                var hat = GetClothingObject(id, sex.GetHeadgearWeenie(appearance.HeadgearStyle), appearance.HeadgearColor, appearance.HeadgearHue);
                if (hat != null)
                    character.WieldedItems.Add(new ObjectGuid(hat.AceObjectId), hat);
                else
                    CreateIOU(character, sex.GetHeadgearWeenie(appearance.HeadgearStyle));
            }

            var shirt = GetClothingObject(id, sex.GetShirtWeenie(appearance.ShirtStyle), appearance.ShirtColor, appearance.ShirtHue);
            if (shirt != null)
                character.WieldedItems.Add(new ObjectGuid(shirt.AceObjectId), shirt);
            else
                CreateIOU(character, sex.GetShirtWeenie(appearance.ShirtStyle));

            var pants = GetClothingObject(id, sex.GetPantsWeenie(appearance.PantsStyle), appearance.PantsColor, appearance.PantsHue);
            if (pants != null)
                character.WieldedItems.Add(new ObjectGuid(pants.AceObjectId), pants);
            else
                CreateIOU(character, sex.GetPantsWeenie(appearance.PantsStyle));

            var shoes = GetClothingObject(id, sex.GetFootwearWeenie(appearance.FootwearStyle), appearance.FootwearColor, appearance.FootwearHue);
            if (shoes != null)
                character.WieldedItems.Add(new ObjectGuid(shoes.AceObjectId), shoes);
            else
                CreateIOU(character, sex.GetFootwearWeenie(appearance.FootwearStyle));

            // Profession (Adventurer, Bow Hunter, etc)
            // TODO - Add this title to the available titles for this character.
            var templateOption = reader.ReadInt32();
            string templateName = cg.HeritageGroups[(uint)character.Heritage].Templates[templateOption].Name;
            character.Title = templateName;
            character.Template = templateName;
            character.CharacterTitleId = (int)cg.HeritageGroups[(uint)character.Heritage].Templates[templateOption].Title;
            character.NumCharacterTitles = 1;

            // stats
            uint totalAttributeCredits = cg.HeritageGroups[(uint)character.Heritage].AttributeCredits;
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
            character.AvailableSkillCredits = (int)cg.HeritageGroups[(uint)character.Heritage].SkillCredits;

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
                        // Olthoi Spitter is a special case
                        if (character.Heritage == (int)HeritageGroup.OlthoiAcid)
                        {
                            character.SpellIdProperties.Add(new AceObjectPropertiesSpell() { AceObjectId = id, SpellId = spell.SpellId });
                            // Continue to next spell as Olthoi spells do not have the SpecializedOnly field
                            continue;
                        }

                        if (charSkill.Status == SkillStatus.Trained && spell.SpecializedOnly == false)
                        {
                            character.SpellIdProperties.Add(new AceObjectPropertiesSpell() { AceObjectId = id, SpellId = spell.SpellId });
                        }
                        else if (charSkill.Status == SkillStatus.Specialized)
                        {
                            character.SpellIdProperties.Add(new AceObjectPropertiesSpell() { AceObjectId = id, SpellId = spell.SpellId });
                        }
                    }
                }
            }

            character.Name = reader.ReadString16L();
            character.DisplayName = character.Name; // unsure

            // Index used to determine the starting location
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

        [GameMessage(GameMessageOpcode.CharacterLogOff, SessionState.WorldConnected)]
        public static void CharacterLogOff(ClientMessage message, Session session)
        {
            session.LogOffPlayer();
        }

        private static AceObject GetClothingObject(uint playerIID, uint weenieClassId, uint palette, double shade)
        {
            AceObject clothingObj;

            try
            {
                clothingObj = (AceObject)DatabaseManager.World.GetAceObjectByWeenie(weenieClassId).Clone(GuidManager.NewItemGuid().Full);
            }
            catch (NullReferenceException)
            {
                return null;
            }

            clothingObj.IconDID = DatManager.PortalDat.ReadFromDat<ClothingTable>(clothingObj.ClothingBaseDID.Value).GetIcon(palette);
            clothingObj.PaletteBaseDID = palette;
            clothingObj.Shade = shade;
            clothingObj.CurrentWieldedLocation = clothingObj.ValidLocations;
            clothingObj.WielderIID = playerIID;

            //if (shirtCT.ClothingBaseEffects.ContainsKey(sex.SetupID))
            //{
            //    ClothingBaseEffect shirtCBE = shirtCT.ClothingBaseEffects[sex.SetupID];
            //    for (int i = 0; i < shirtCBE.CloObjectEffects.Count; i++)
            //    {
            //        byte partNum = (byte)shirtCBE.CloObjectEffects[i].Index;
            //        shirt.AnimationOverrides.Add(new AnimationOverride()
            //        {
            //            AceObjectId = shirt.AceObjectId,
            //            AnimationId = shirtCBE.CloObjectEffects[i].ModelId,
            //            Index = (byte)shirtCBE.CloObjectEffects[i].Index
            //        });

            //        for (int j = 0; j < shirtCBE.CloObjectEffects[i].CloTextureEffects.Count; j++)
            //        {
            //            shirt.TextureOverrides.Add(new TextureMapOverride()
            //            {
            //                AceObjectId = shirt.AceObjectId,
            //                Index = (byte)shirtCBE.CloObjectEffects[i].Index,
            //                OldId = (ushort)shirtCBE.CloObjectEffects[i].CloTextureEffects[j].OldTexture,
            //                NewId = (ushort)shirtCBE.CloObjectEffects[i].CloTextureEffects[j].NewTexture
            //            });
            //        }
            //    }

            //    // Apply the proper palette(s). Unlike character skin/hair, clothes can have several palette ranges!
            //    if (shirtCT.ClothingSubPalEffects.ContainsKey(appearance.ShirtColor))
            //    {
            //        CloSubPalEffect shirtSubPal = shirtCT.ClothingSubPalEffects[appearance.ShirtColor];
            //        for (int i = 0; i < shirtSubPal.CloSubPalettes.Count; i++)
            //        {
            //            PaletteSet shirtPalSet = PaletteSet.ReadFromDat(shirtSubPal.CloSubPalettes[i].PaletteSet);
            //            ushort shirtPal = (ushort)shirtPalSet.GetPaletteID(appearance.ShirtHue);

            //            if (shirtPal > 0) // shirtPal will be 0 if the palette set is empty/not found
            //            {
            //                for (int j = 0; j < shirtSubPal.CloSubPalettes[i].Ranges.Count; j++)
            //                {
            //                    uint palOffset = shirtSubPal.CloSubPalettes[i].Ranges[j].Offset / 8;
            //                    uint numColors = shirtSubPal.CloSubPalettes[i].Ranges[j].NumColors / 8;
            //                    shirt.PaletteOverrides.Add(new PaletteOverride()
            //                    {
            //                        AceObjectId = shirt.AceObjectId,
            //                        SubPaletteId = shirtPal,
            //                        Offset = (ushort)palOffset,
            //                        Length = (ushort)numColors
            //                    });
            //                }
            //            }
            //        }
            //    }
            //}

            return clothingObj;
        }

        private static void CreateIOU(AceCharacter character, uint missingWeenieId)
        {
            var iouObj = (AceObject)DatabaseManager.World.GetAceObjectByWeenie("parchment").Clone(GuidManager.NewItemGuid().Full);

            iouObj.Name = "IOU";
            iouObj.EncumbranceVal = 0;
            iouObj.Value = 0;            
            iouObj.ShortDesc = "An IOU for a missing database object.";
            iouObj.Inscription = "Sorry about that chief...";
            iouObj.ScribeName = "Ripley";
            iouObj.ScribeAccount = "prewritten";
            iouObj.IgnoreAuthor = false;
            iouObj.AppraisalPages = 1;
            iouObj.AppraisalMaxPages = 1;

            iouObj.ContainerIID = character.AceObjectId;

            // FIXME: This is wrong and should also be unnecessary but we're not handling storing and reading back object placement within a container correctly so this is here to make it work.
            // TODO: fix placement (order or slot) issues within containers.
            iouObj.Placement = 0;

            var bookProperties = new AceObjectPropertiesBook();
            bookProperties.AceObjectId = iouObj.AceObjectId;
            bookProperties.AuthorName = "Ripley";
            bookProperties.AuthorAccount = "prewritten";
            bookProperties.Page = 0;
            bookProperties.PageText = $"{missingWeenieId}\n\nSorry but the database does not have a weenie for weenieClassId #{missingWeenieId} so in lieu of that here is an IOU for that item.";

            iouObj.BookProperties.Add(bookProperties.Page, bookProperties);

            if (iouObj != null)
                character.Inventory.Add(new ObjectGuid(iouObj.AceObjectId), iouObj);
        }
    }
}
