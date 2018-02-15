using System;
using System.Collections.Generic;
using System.Linq;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.Database.Models.Shard;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity;
using ACE.Server.Entity.WorldObjects;
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

            var guid = GuidManager.NewPlayerGuid();

            CharacterCreateEx(message, session, guid);
        }

        private static void CharacterCreateEx(ClientMessage message, Session session, ObjectGuid guid)
        {
            var characterCreateInfo = new CharacterCreateInfo();
            characterCreateInfo.Unpack(message.Payload);

            // Disable OlthoiPlay characters for now. They're not implemented yet.
            // FIXME: Restore OlthoiPlay characters when properly handled.
            if (characterCreateInfo.Heritage == (int)HeritageGroup.Olthoi || characterCreateInfo.Heritage == (int)HeritageGroup.OlthoiAcid)
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Pending);
                return;
            }

            var cg = DatManager.PortalDat.CharGen;

            var weenie = DatabaseManager.World.GetCachedWeenie(1);

            var player = new Player(weenie, null, session);

            player.Guid = guid;

            player.SetProperty(PropertyInt.HeritageGroup, (int)characterCreateInfo.Heritage);
            player.SetProperty(PropertyString.HeritageGroup, cg.HeritageGroups[characterCreateInfo.Heritage].Name);
            player.SetProperty(PropertyInt.Gender, (int)characterCreateInfo.Gender);
            player.SetProperty(PropertyString.Sex, characterCreateInfo.Gender == 1 ? "Male" : "Female");

            player.SetProperty(PropertyDataId.Icon, cg.HeritageGroups[characterCreateInfo.Heritage].IconImage); // I don't believe this is used anywhere in the client, but it might be used by a future custom launcher

            // pull character data from the dat file
            var sex = cg.HeritageGroups[characterCreateInfo.Heritage].Genders[(int)characterCreateInfo.Gender];

            player.SetProperty(PropertyDataId.MotionTable, sex.MotionTable);
            player.SetProperty(PropertyDataId.SoundTable, sex.SoundTable);
            player.SetProperty(PropertyDataId.PhysicsEffectTable, sex.PhysicsTable);
            player.SetProperty(PropertyDataId.Setup, sex.SetupID);
            player.SetProperty(PropertyDataId.PaletteBase, sex.BasePalette);
            player.SetProperty(PropertyDataId.CombatTable, sex.CombatTable);

            // Check the character scale
            if (sex.Scale != 100u)
                player.SetProperty(PropertyDouble.DefaultScale, (sex.Scale / 100f)); // Scale is stored as a percentage

            // Get the hair first, because we need to know if you're bald, and that's the name of that tune!
            var hairstyle = sex.HairStyleList[Convert.ToInt32(characterCreateInfo.Apperance.HairStyle)];

            // Certain races (Undead, Tumeroks, Others?) have multiple body styles available. This is controlled via the "hair style".
            if (hairstyle.AlternateSetup > 0)
                player.SetProperty(PropertyDataId.Setup, hairstyle.AlternateSetup);

            player.SetProperty(PropertyDataId.EyesTexture, sex.GetEyeTexture(characterCreateInfo.Apperance.Eyes, hairstyle.Bald));
            player.SetProperty(PropertyDataId.DefaultEyesTexture, sex.GetDefaultEyeTexture(characterCreateInfo.Apperance.Eyes, hairstyle.Bald));
            player.SetProperty(PropertyDataId.NoseTexture, sex.GetNoseTexture(characterCreateInfo.Apperance.Nose));
            player.SetProperty(PropertyDataId.DefaultNoseTexture, sex.GetDefaultNoseTexture(characterCreateInfo.Apperance.Nose));
            player.SetProperty(PropertyDataId.MouthTexture, sex.GetMouthTexture(characterCreateInfo.Apperance.Mouth));
            player.SetProperty(PropertyDataId.DefaultMouthTexture, sex.GetDefaultMouthTexture(characterCreateInfo.Apperance.Mouth));
            player.SetProperty(PropertyDataId.HairTexture, sex.GetHairTexture(characterCreateInfo.Apperance.HairStyle));
            player.SetProperty(PropertyDataId.DefaultHairTexture, sex.GetDefaultHairTexture(characterCreateInfo.Apperance.HairStyle));
            player.SetProperty(PropertyDataId.HeadObject, sex.GetHeadObject(characterCreateInfo.Apperance.HairStyle));

            // Skin is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
            var skinPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(sex.SkinPalSet);
            player.SetProperty(PropertyDataId.SkinPalette, skinPalSet.GetPaletteID(characterCreateInfo.Apperance.SkinHue));
            player.SetProperty(PropertyDouble.Shade, characterCreateInfo.Apperance.SkinHue);

            // Hair is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
            var hairPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(sex.HairColorList[Convert.ToInt32(characterCreateInfo.Apperance.HairColor)]);
            player.SetProperty(PropertyDataId.HairPalette, hairPalSet.GetPaletteID(characterCreateInfo.Apperance.HairHue));

            // Eye Color
            player.SetProperty(PropertyDataId.EyesPalette, sex.EyeColorList[Convert.ToInt32(characterCreateInfo.Apperance.EyeColor)]);

            if (characterCreateInfo.Apperance.HeadgearStyle < 0xFFFFFFFF) // No headgear is max UINT
            {
                var hat = GetClothingObject(sex.GetHeadgearWeenie(characterCreateInfo.Apperance.HeadgearStyle), characterCreateInfo.Apperance.HeadgearColor, characterCreateInfo.Apperance.HeadgearHue);
                if (hat != null)
                    player.EquipObject(hat);
                else
                    CreateIOU(player, sex.GetHeadgearWeenie(characterCreateInfo.Apperance.HeadgearStyle));
            }

            var shirt = GetClothingObject(sex.GetShirtWeenie(characterCreateInfo.Apperance.ShirtStyle), characterCreateInfo.Apperance.ShirtColor, characterCreateInfo.Apperance.ShirtHue);
            if (shirt != null)
                player.EquipObject(shirt);
            else
                CreateIOU(player, sex.GetShirtWeenie(characterCreateInfo.Apperance.ShirtStyle));

            var pants = GetClothingObject(sex.GetPantsWeenie(characterCreateInfo.Apperance.PantsStyle), characterCreateInfo.Apperance.PantsColor, characterCreateInfo.Apperance.PantsHue);
            if (pants != null)
                player.EquipObject(pants);
            else
                CreateIOU(player, sex.GetPantsWeenie(characterCreateInfo.Apperance.PantsStyle));

            var shoes = GetClothingObject(sex.GetFootwearWeenie(characterCreateInfo.Apperance.FootwearStyle), characterCreateInfo.Apperance.FootwearColor, characterCreateInfo.Apperance.FootwearHue);
            if (shoes != null)
                player.EquipObject(shoes);
            else
                CreateIOU(player, sex.GetFootwearWeenie(characterCreateInfo.Apperance.FootwearStyle));

            // Profession (Adventurer, Bow Hunter, etc)
            // TODO - Add this title to the available titles for this character.
            string templateName = cg.HeritageGroups[characterCreateInfo.Heritage].Templates[characterCreateInfo.TemplateOption].Name;
            player.SetProperty(PropertyString.Title, templateName);
            player.SetProperty(PropertyString.Template, templateName);
            player.SetProperty(PropertyInt.CharacterTitleId, (int)cg.HeritageGroups[characterCreateInfo.Heritage].Templates[characterCreateInfo.TemplateOption].Title);
            player.SetProperty(PropertyInt.NumCharacterTitles, 1);

            // stats
            /* todo fix this .. need to init the attribute properties for a new biota
            uint totalAttributeCredits = cg.HeritageGroups[characterCreateInfo.Heritage].AttributeCredits;
            uint usedAttributeCredits = 0;

            var strength = player.Biota.GetAttribute(Ability.Strength);
            strength.InitLevel = ValidateAttributeCredits(characterCreateInfo.StrengthAbility, usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += strength.InitLevel;

            var endurance = player.Biota.GetAttribute(Ability.Endurance);
            endurance.InitLevel = ValidateAttributeCredits(characterCreateInfo.EnduranceAbility, usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += endurance.InitLevel;

            var coordination = player.Biota.GetAttribute(Ability.Coordination);
            coordination.InitLevel = ValidateAttributeCredits(characterCreateInfo.CoordinationAbility, usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += coordination.InitLevel;

            var quickness = player.Biota.GetAttribute(Ability.Quickness);
            quickness.InitLevel = ValidateAttributeCredits(characterCreateInfo.QuicknessAbility, usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += quickness.InitLevel;

            var focus = player.Biota.GetAttribute(Ability.Focus);
            focus.InitLevel = ValidateAttributeCredits(characterCreateInfo.FocusAbility, usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += focus.InitLevel;

            var self = player.Biota.GetAttribute(Ability.Self);
            self.InitLevel = ValidateAttributeCredits(characterCreateInfo.SelfAbility, usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += self.InitLevel;*/

            // Validate this is equal to actual attribute credits (330 for all but "Olthoi", which have 60
            // todo if (usedAttributeCredits > .....

            // data we don't care about
            //characterCreateInfo.CharacterSlot;
            //characterCreateInfo.ClassId;

            // characters start with max vitals
            // TODO for the new format
            /*character.Health.Current = character.Health.MaxValue;
            character.Stamina.Current = character.Stamina.MaxValue;
            character.Mana.Current = character.Mana.MaxValue;*/

            // set initial skill credit amount. 52 for all but "Olthoi", which have 68
            player.SetProperty(PropertyInt.AvailableSkillCredits, (int)cg.HeritageGroups[characterCreateInfo.Heritage].SkillCredits);

            // TODO for the new format
            /*for (int i = 0; i < characterCreateInfo.SkillStatuses.Count; i++)
            {
                var skill = (Skill)i;
                var skillCost = skill.GetCost();
                var skillStatus = characterCreateInfo.SkillStatuses[i];

                if (skillStatus == SkillStatus.Specialized)
                {
                    character.TrainSkill(skill, skillCost.TrainingCost);
                    character.SpecializeSkill(skill, skillCost.SpecializationCost);
                    // oddly enough, specialized skills don't get any free ranks like trained do
                }
                else if (skillStatus == SkillStatus.Trained)
                {
                    character.TrainSkill(skill, skillCost.TrainingCost);
                    character.AceObjectPropertiesSkills[skill].Ranks = 5;
                    character.AceObjectPropertiesSkills[skill].ExperienceSpent = 526;
                }
                else if (skillCost != null && skillStatus == SkillStatus.Untrained)
                    character.UntrainSkill(skill, skillCost.TrainingCost);
            }*/

            // grant starter items based on skills
            var starterGearConfig = StarterGearFactory.GetStarterGearConfiguration();
            List<uint> grantedItems = new List<uint>();

            foreach (var skillGear in starterGearConfig.Skills)
            {
                // TODO for the new format
                /*
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

                        var loot = WorldObjectFactory.CreateNewWorldObject(item.WeenieId);
                        loot.SetProperty(PropertyInt.Placement, 0);
                        loot.SetProperty(PropertyInstanceId.Container, (int)guid.Full);
                        if (item.StackSize > 1) loot.SetProperty(PropertyInt.StackSize, item.StackSize);
                        //character.Inventory.Add(loot.Guid, loot); TODO FIX THIS FOR NEW MODEL
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

                            var loot = WorldObjectFactory.CreateNewWorldObject(item.WeenieId);
                            loot.SetProperty(PropertyInt.Placement, 0);
                            loot.SetProperty(PropertyInstanceId.Container, (int)guid.Full);
                            if (item.StackSize > 1) loot.SetProperty(PropertyInt.StackSize, item.StackSize);
                            //character.Inventory.Add(loot.Guid, loot); TODO FIX THIS FOR NEW MODEL
                            grantedItems.Add(item.WeenieId);
                        }
                    }
                    
                    foreach (var spell in skillGear.Spells)
                    {
                        // Olthoi Spitter is a special case
                        if (character.Heritage == (int)HeritageGroup.OlthoiAcid)
                        {
                            character.SpellIdProperties.Add(new AceObjectPropertiesSpell() { AceObjectId = guid.Full, SpellId = spell.SpellId });
                            // Continue to next spell as Olthoi spells do not have the SpecializedOnly field
                            continue;
                        }

                        if (charSkill.Status == SkillStatus.Trained && spell.SpecializedOnly == false)
                            character.SpellIdProperties.Add(new AceObjectPropertiesSpell() { AceObjectId = guid.Full, SpellId = spell.SpellId });
                        else if (charSkill.Status == SkillStatus.Specialized)
                            character.SpellIdProperties.Add(new AceObjectPropertiesSpell() { AceObjectId = guid.Full, SpellId = spell.SpellId });
                    }
                }*/
            }

            player.SetProperty(PropertyString.Name, characterCreateInfo.Name);
            player.SetProperty(PropertyString.DisplayName, characterCreateInfo.Name); // unsure

            // Index used to determine the starting location
            uint startArea = characterCreateInfo.StartArea;

            player.SetProperty(PropertyBool.IsAdmin, characterCreateInfo.IsAdmin);
            player.SetProperty(PropertyBool.IsSentinel, characterCreateInfo.IsEnvoy);

            DatabaseManager.Shard.IsCharacterNameAvailable(characterCreateInfo.Name, isAvailable =>
            {
                if (!isAvailable)
                {
                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);
                    return;
                }

                player.SetProperty(PropertyInstanceId.Account, (int)session.Id);

                CharacterCreateSetDefaultCharacterOptions(player);
                CharacterCreateSetDefaultCharacterPositions(player, startArea);

                // We must await here -- 
                DatabaseManager.Shard.AddBiota(player.Biota, saveSuccess =>
                {
                    if (!saveSuccess)
                    {
                        SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.DatabaseDown);
                        return;
                    }

                    session.AccountCharacters.Add(new CachedCharacter(guid, (byte)session.AccountCharacters.Count, characterCreateInfo.Name, 0));

                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Ok, guid, characterCreateInfo.Name);
                });
            });
        }

        /// <summary>
        /// Checks if the total credits is more than this class is allowed.
        /// </summary>
        /// <returns>The original value or the max allowed.</returns>
        private static ushort ValidateAttributeCredits(uint attributeValue, uint allAttributes, uint maxAttributes)
        {
            if ((attributeValue + allAttributes) > maxAttributes)
                return (ushort)(maxAttributes - allAttributes);

            return (ushort)attributeValue;
        }

        private static void CharacterCreateSetDefaultCharacterOptions(Player player)
        {
            // Todo fix for new model that doesn't use AceCharacter
            AceCharacter character = new AceCharacter(0); // temp
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

        public static void CharacterCreateSetDefaultCharacterPositions(Player player, uint startArea)
        {
            AceCharacter character = new AceCharacter(0); // temp
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

        private static WorldObject GetClothingObject(uint weenieClassId, uint palette, double shade)
        {
            var weenie = DatabaseManager.World.GetCachedWeenie(weenieClassId);

            if (weenie == null)
                return null;

            var worldObject = WorldObjectFactory.CreateNewWorldObject(weenie);

            var icon = DatManager.PortalDat.ReadFromDat<ClothingTable>(worldObject.GetProperty(PropertyDataId.ClothingBase) ?? 0).GetIcon(palette);

            worldObject.SetProperty(PropertyDataId.Icon, icon);
            worldObject.SetProperty(PropertyDataId.PaletteBase, palette);
            worldObject.SetProperty(PropertyDouble.Shade, shade);
            worldObject.SetProperty(PropertyInt.CurrentWieldedLocation, worldObject.GetProperty(PropertyInt.ValidLocations) ?? 0);

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

            return worldObject;
        }

        private static void CreateIOU(Player player, uint missingWeenieId)
        {
            throw new NotImplementedException();/*
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
                character.Inventory.Add(new ObjectGuid(iouObj.AceObjectId), iouObj);*/
        }
    }
}
