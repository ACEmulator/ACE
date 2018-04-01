using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ACE.Server.WorldObjects;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Database.Models.World;
using log4net;

namespace ACE.Server.Network.Handlers
{
    public static class CharacterHandler
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            var cachedCharacter = session.AccountCharacters.SingleOrDefault(c => c.BiotaId == guid.Full);
            if (cachedCharacter == null)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            session.CharacterRequested = cachedCharacter;

            session.InitSessionForWorldLogin();

            session.Character = cachedCharacter;

            session.State = SessionState.WorldConnected;

            LandblockManager.PlayerEnterWorld(session, new ObjectGuid(cachedCharacter.BiotaId));

            // Save the the LoginTimestamp
            cachedCharacter.LastLoginTimestamp = Time.GetTimestamp();
            DatabaseManager.Shard.SaveCharacter(cachedCharacter, null);
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

            var cachedCharacter = session.AccountCharacters[(int)characterSlot];
            if (cachedCharacter == null)
            {
                session.SendCharacterError(CharacterError.Delete);
                return;
            }

            // TODO: check if character is already pending removal

            session.Network.EnqueueSend(new GameMessageCharacterDelete());

            DatabaseManager.Shard.DeleteOrRestoreCharacter(Time.GetUnixTime() + 3600ul, cachedCharacter.BiotaId, ((bool deleteOrRestoreSuccess) =>
            {
                if (deleteOrRestoreSuccess)
                {
                    DatabaseManager.Shard.GetCharacters(session.Id, ((List<Character> result) =>
                    {
                        session.UpdateCachedCharacters(result);
                        session.Network.EnqueueSend(new GameMessageCharacterList(result, session));
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

            var cachedCharacter = session.AccountCharacters.SingleOrDefault(c => c.BiotaId == guid.Full);
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
                    DatabaseManager.Shard.DeleteOrRestoreCharacter(0, cachedCharacter.BiotaId, ((bool deleteOrRestoreSuccess) =>
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

            CharacterCreateEx(message, session);
        }

        private static void CharacterCreateEx(ClientMessage message, Session session)
        {
            var characterCreateInfo = new CharacterCreateInfo();
            characterCreateInfo.Unpack(message.Payload);

            // TODO: Check for Banned Name Here
            //DatabaseManager.Shard.IsCharacterNameBanned(characterCreateInfo.Name, isBanned =>
            //{
            //    if (!isBanned)
            //    {
            //        SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameBanned);
            //        return;
            //    }
            //});

            // Disable OlthoiPlay characters for now. They're not implemented yet.
            // FIXME: Restore OlthoiPlay characters when properly handled.
            if (characterCreateInfo.Heritage == (int)HeritageGroup.Olthoi || characterCreateInfo.Heritage == (int)HeritageGroup.OlthoiAcid)
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Pending);
                return;
            }

            var cg = DatManager.PortalDat.CharGen;

            var isAdmin = characterCreateInfo.IsAdmin && (session.AccessLevel >= AccessLevel.Developer);
            var isEnvoy = characterCreateInfo.IsEnvoy && (session.AccessLevel >= AccessLevel.Sentinel);

            Weenie weenie;
            if (ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions)
            {
                if (session.AccessLevel >= AccessLevel.Developer && session.AccessLevel <= AccessLevel.Admin)
                    weenie = DatabaseManager.World.GetCachedWeenie("admin");
                else if (session.AccessLevel >= AccessLevel.Sentinel && session.AccessLevel <= AccessLevel.Envoy)
                    weenie = DatabaseManager.World.GetCachedWeenie("sentinel");
                else
                    weenie = DatabaseManager.World.GetCachedWeenie("human");

                if (characterCreateInfo.Heritage == (int)HeritageGroup.Olthoi && weenie.Type == (int)WeenieType.Admin)
                    weenie = DatabaseManager.World.GetCachedWeenie("olthoiadmin");

                if (characterCreateInfo.Heritage == (int)HeritageGroup.OlthoiAcid && weenie.Type == (int)WeenieType.Admin)
                    weenie = DatabaseManager.World.GetCachedWeenie("olthoiacidadmin");
            }
            else
                weenie = DatabaseManager.World.GetCachedWeenie("human");

            if (characterCreateInfo.Heritage == (int)HeritageGroup.Olthoi && weenie.Type == (int)WeenieType.Creature)
                weenie = DatabaseManager.World.GetCachedWeenie("olthoiplayer");

            if (characterCreateInfo.Heritage == (int)HeritageGroup.OlthoiAcid && weenie.Type == (int)WeenieType.Creature)
                weenie = DatabaseManager.World.GetCachedWeenie("olthoiacidplayer");

            if (isEnvoy)
                weenie = DatabaseManager.World.GetCachedWeenie("sentinel");

            if (isAdmin)
                weenie = DatabaseManager.World.GetCachedWeenie("admin");

            if (weenie == null)
                weenie = DatabaseManager.World.GetCachedWeenie("human"); // Default catch-all

            if (weenie == null) // If it is STILL null after the above catchall, the database is missing critical data and cannot continue with character creation.
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.DatabaseDown);
                log.Error($"Database does not contain the weenie for human (1). Characters cannot be created until the missing weenie is restored.");
                return;
            }


            var guid = GuidManager.NewPlayerGuid();

            // If Database didn't have Sentinel/Admin weenies, alter the weenietype coming in.
            if (ConfigManager.Config.Server.Accounts.OverrideCharacterPermissions)
            {
                if (session.AccessLevel >= AccessLevel.Developer && session.AccessLevel <= AccessLevel.Admin && weenie.Type != (int)WeenieType.Admin)
                    weenie.Type = (int)WeenieType.Admin;
                else if (session.AccessLevel >= AccessLevel.Sentinel && session.AccessLevel <= AccessLevel.Envoy && weenie.Type != (int)WeenieType.Sentinel)
                    weenie.Type = (int)WeenieType.Sentinel;
            }

            var player = new Player(weenie, guid, session);

            player.SetProperty(PropertyInt.HeritageGroup, (int)characterCreateInfo.Heritage);
            player.SetProperty(PropertyString.HeritageGroup, cg.HeritageGroups[characterCreateInfo.Heritage].Name);
            player.SetProperty(PropertyInt.Gender, (int)characterCreateInfo.Gender);
            player.SetProperty(PropertyString.Sex, characterCreateInfo.Gender == 1 ? "Male" : "Female");

            //player.SetProperty(PropertyDataId.Icon, cg.HeritageGroups[characterCreateInfo.Heritage].IconImage); // I don't believe this is used anywhere in the client, but it might be used by a future custom launcher

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
                player.SetProperty(PropertyFloat.DefaultScale, (sex.Scale / 100f)); // Scale is stored as a percentage

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
            player.SetProperty(PropertyFloat.Shade, characterCreateInfo.Apperance.SkinHue);

            // Hair is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
            var hairPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(sex.HairColorList[Convert.ToInt32(characterCreateInfo.Apperance.HairColor)]);
            player.SetProperty(PropertyDataId.HairPalette, hairPalSet.GetPaletteID(characterCreateInfo.Apperance.HairHue));

            // Eye Color
            player.SetProperty(PropertyDataId.EyesPalette, sex.EyeColorList[Convert.ToInt32(characterCreateInfo.Apperance.EyeColor)]);

            if (characterCreateInfo.Apperance.HeadgearStyle < 0xFFFFFFFF) // No headgear is max UINT
            {
                var hat = GetClothingObject(sex.GetHeadgearWeenie(characterCreateInfo.Apperance.HeadgearStyle), characterCreateInfo.Apperance.HeadgearColor, characterCreateInfo.Apperance.HeadgearHue);
                if (hat != null)
                    player.TryEquipObject(hat, hat.GetProperty(PropertyInt.ValidLocations) ?? 0);
                else
                    CreateIOU(player, sex.GetHeadgearWeenie(characterCreateInfo.Apperance.HeadgearStyle));
            }

            var shirt = GetClothingObject(sex.GetShirtWeenie(characterCreateInfo.Apperance.ShirtStyle), characterCreateInfo.Apperance.ShirtColor, characterCreateInfo.Apperance.ShirtHue);
            if (shirt != null)
                player.TryEquipObject(shirt, shirt.GetProperty(PropertyInt.ValidLocations) ?? 0);
            else
                CreateIOU(player, sex.GetShirtWeenie(characterCreateInfo.Apperance.ShirtStyle));

            var pants = GetClothingObject(sex.GetPantsWeenie(characterCreateInfo.Apperance.PantsStyle), characterCreateInfo.Apperance.PantsColor, characterCreateInfo.Apperance.PantsHue);
            if (pants != null)
                player.TryEquipObject(pants, pants.GetProperty(PropertyInt.ValidLocations) ?? 0);
            else
                CreateIOU(player, sex.GetPantsWeenie(characterCreateInfo.Apperance.PantsStyle));

            var shoes = GetClothingObject(sex.GetFootwearWeenie(characterCreateInfo.Apperance.FootwearStyle), characterCreateInfo.Apperance.FootwearColor, characterCreateInfo.Apperance.FootwearHue);
            if (shoes != null)
                player.TryEquipObject(shoes, shoes.GetProperty(PropertyInt.ValidLocations) ?? 0);
            else
                CreateIOU(player, sex.GetFootwearWeenie(characterCreateInfo.Apperance.FootwearStyle));

            string templateName = cg.HeritageGroups[characterCreateInfo.Heritage].Templates[characterCreateInfo.TemplateOption].Name;
            //player.SetProperty(PropertyString.Title, templateName);
            player.SetProperty(PropertyString.Template, templateName);
            player.AddTitle(cg.HeritageGroups[characterCreateInfo.Heritage].Templates[characterCreateInfo.TemplateOption].Title, true);

            // stats
            uint totalAttributeCredits = cg.HeritageGroups[characterCreateInfo.Heritage].AttributeCredits;
            uint usedAttributeCredits = 0;

            player.Strength.StartingValue = ValidateAttributeCredits(characterCreateInfo.StrengthAbility, usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += player.Strength.StartingValue;

            player.Endurance.StartingValue = ValidateAttributeCredits(characterCreateInfo.EnduranceAbility, usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += player.Endurance.StartingValue;

            player.Coordination.StartingValue = ValidateAttributeCredits(characterCreateInfo.CoordinationAbility, usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += player.Coordination.StartingValue;

            player.Quickness.StartingValue = ValidateAttributeCredits(characterCreateInfo.QuicknessAbility, usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += player.Quickness.StartingValue;

            player.Focus.StartingValue = ValidateAttributeCredits(characterCreateInfo.FocusAbility, usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += player.Focus.StartingValue;

            player.Self.StartingValue = ValidateAttributeCredits(characterCreateInfo.SelfAbility, usedAttributeCredits, totalAttributeCredits);
            usedAttributeCredits += player.Self.StartingValue;

            // Validate this is equal to actual attribute credits. 330 for all but "Olthoi", which have 60
            if (usedAttributeCredits > 330 || ((characterCreateInfo.Heritage == (int)HeritageGroup.Olthoi || characterCreateInfo.Heritage == (int)HeritageGroup.OlthoiAcid) && usedAttributeCredits > 60))
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Corrupt);
                return;
            }

            // data we don't care about
            //characterCreateInfo.CharacterSlot;
            //characterCreateInfo.ClassId;

            // characters start with max vitals
            player.Health.Current = player.Health.Base;
            player.Stamina.Current = player.Stamina.Base;
            player.Mana.Current = player.Mana.Base;

            // set initial skill credit amount. 52 for all but "Olthoi", which have 68
            player.SetProperty(PropertyInt.AvailableSkillCredits, (int)cg.HeritageGroups[characterCreateInfo.Heritage].SkillCredits);

            for (int i = 0; i < characterCreateInfo.SkillStatuses.Count; i++)
            {
                var skill = (Skill)i;
                var skillCost = skill.GetCost();
                var skillStatus = characterCreateInfo.SkillStatuses[i];

                if (skillStatus == SkillStatus.Specialized)
                {
                    player.TrainSkill(skill, skillCost.TrainingCost);
                    player.SpecializeSkill(skill, skillCost.SpecializationCost);
                    // oddly enough, specialized skills don't get any free ranks like trained do
                }
                else if (skillStatus == SkillStatus.Trained)
                {
                    player.TrainSkill(skill, skillCost.TrainingCost);
                    player.GetCreatureSkill(skill).Ranks = 5;
                    player.GetCreatureSkill(skill).ExperienceSpent = 526;
                }
                else if (skillCost != null && skillStatus == SkillStatus.Untrained)
                    player.UntrainSkill(skill, skillCost.TrainingCost);
            }

            // grant starter items based on skills
            var starterGearConfig = StarterGearFactory.GetStarterGearConfiguration();
            var grantedWeenies = new List<uint>();

            foreach (var skillGear in starterGearConfig.Skills)
            {
                var charSkill = player.Skills[(Skill)skillGear.SkillId];
                if (charSkill.Status == SkillStatus.Trained || charSkill.Status == SkillStatus.Specialized)
                {
                    foreach (var item in skillGear.Gear)
                    {
                        if (grantedWeenies.Contains(item.WeenieId))
                        {
                            var existingItem = player.Inventory.Values.FirstOrDefault(i => i.WeenieClassId == item.WeenieId);
                            if (existingItem == null || (existingItem.MaxStackSize ?? 1) <= 1)
                                continue;

                            existingItem.StackSize += item.StackSize;
                            continue;
                        }

                        var loot = WorldObjectFactory.CreateNewWorldObject(item.WeenieId);
                        if (loot != null)
                            if (loot.StackSize.HasValue && loot.MaxStackSize.HasValue)
                                loot.StackSize = (item.StackSize <= loot.MaxStackSize) ? item.StackSize : loot.MaxStackSize;
                        if (loot == null)
                        {
                            CreateIOU(player, item.WeenieId);
                            continue;
                        }

                        if (player.TryAddToInventory(loot))
                            grantedWeenies.Add(item.WeenieId);
                    }

                    var heritageLoot = skillGear.Heritage.FirstOrDefault(sh => sh.HeritageId == characterCreateInfo.Heritage);
                    if (heritageLoot != null)
                    {
                        foreach (var item in heritageLoot.Gear)
                        {
                            if (grantedWeenies.Contains(item.WeenieId))
                            {
                                var existingItem = player.Inventory.Values.FirstOrDefault(i => i.WeenieClassId == item.WeenieId);
                                if (existingItem == null || (existingItem.MaxStackSize ?? 1) <= 1)
                                    continue;

                                existingItem.StackSize += item.StackSize;
                                continue;
                            }

                            var loot = WorldObjectFactory.CreateNewWorldObject(item.WeenieId);
                            if (loot != null)
                                if (loot.StackSize.HasValue && loot.MaxStackSize.HasValue)
                                    loot.StackSize = (item.StackSize <= loot.MaxStackSize) ? item.StackSize : loot.MaxStackSize;
                            if (loot == null)
                            {
                                CreateIOU(player, item.WeenieId);
                                continue;
                            }
                            if (player.TryAddToInventory(loot))
                                grantedWeenies.Add(item.WeenieId);
                        }
                    }
                    
                    foreach (var spell in skillGear.Spells)
                    {
                        // Olthoi Spitter is a special case
                        if (characterCreateInfo.Heritage == (int)HeritageGroup.OlthoiAcid)
                        {
                            player.AddKnownSpell(spell.SpellId);
                            // Continue to next spell as Olthoi spells do not have the SpecializedOnly field
                            continue;
                        }

                        if (charSkill.Status == SkillStatus.Trained && spell.SpecializedOnly == false)
                            player.AddKnownSpell(spell.SpellId);
                        else if (charSkill.Status == SkillStatus.Specialized)
                            player.AddKnownSpell(spell.SpellId);
                    }
                }
            }

            player.Name = characterCreateInfo.Name;
            //player.SetProperty(PropertyString.DisplayName, characterCreateInfo.Name); // unsure

            // Index used to determine the starting location
            uint startArea = characterCreateInfo.StartArea;

            player.SetProperty(PropertyBool.Attackable, true);

            player.SetProperty(PropertyFloat.CreationTimestamp, Time.GetTimestamp());
            player.SetProperty(PropertyInt.CreationTimestamp, (int)player.GetProperty(PropertyFloat.CreationTimestamp));
            player.SetProperty(PropertyString.DateOfBirth, $"{DateTime.UtcNow:dd MMMM yyyy}");

            DatabaseManager.Shard.IsCharacterNameAvailable(characterCreateInfo.Name, isAvailable =>
            {
                if (!isAvailable)
                {
                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);
                    return;
                }

                // player.SetProperty(PropertyInstanceId.Account, (int)session.Id);

                var character = new Character();
                character.AccountId = session.Id;
                character.Name = player.GetProperty(PropertyString.Name);
                character.BiotaId = player.Guid.Full;
                character.IsDeleted = false;

                CharacterCreateSetDefaultCharacterOptions(player);

                player.Location = new Position(cg.StarterAreas[(int)startArea].Locations[0].ObjCellID,
                    cg.StarterAreas[(int)startArea].Locations[0].Frame.Origin.X, cg.StarterAreas[(int)startArea].Locations[0].Frame.Origin.Y, cg.StarterAreas[(int)startArea].Locations[0].Frame.Origin.Z,
                    cg.StarterAreas[(int)startArea].Locations[0].Frame.Orientation.X, cg.StarterAreas[(int)startArea].Locations[0].Frame.Orientation.Y, cg.StarterAreas[(int)startArea].Locations[0].Frame.Orientation.Z, cg.StarterAreas[(int)startArea].Locations[0].Frame.Orientation.W);

                player.Instantiation = player.Location;
                player.Sanctuary = player.Location;

                var possessions = player.GetAllPossessions();
                var possessedBiotas = new Collection<Biota>();
                foreach (var possession in possessions)
                    possessedBiotas.Add(possession.Biota);

                // We must await here -- 
                DatabaseManager.Shard.AddCharacter(character, player.Biota, possessedBiotas, saveSuccess =>
                {
                    if (!saveSuccess)
                    {
                        SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.DatabaseDown);
                        return;
                    }

                    session.AccountCharacters.Add(character);

                    SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Ok, player.Guid, characterCreateInfo.Name);
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
            player.SetCharacterOption(CharacterOption.VividTargetingIndicator, true);
            player.SetCharacterOption(CharacterOption.Display3dTooltips, true);
            player.SetCharacterOption(CharacterOption.ShowCoordinatesByTheRadar, true);
            player.SetCharacterOption(CharacterOption.DisplaySpellDurations, true);
            player.SetCharacterOption(CharacterOption.IgnoreFellowshipRequests, true);
            player.SetCharacterOption(CharacterOption.ShareFellowshipExpAndLuminance, true);
            player.SetCharacterOption(CharacterOption.LetOtherPlayersGiveYouItems, true);
            player.SetCharacterOption(CharacterOption.RunAsDefaultMovement, true);
            player.SetCharacterOption(CharacterOption.AutoTarget, true);
            player.SetCharacterOption(CharacterOption.AutoRepeatAttacks, true);
            player.SetCharacterOption(CharacterOption.UseChargeAttack, true);
            player.SetCharacterOption(CharacterOption.LeadMissileTargets, true);
            player.SetCharacterOption(CharacterOption.ListenToAllegianceChat, true);
            player.SetCharacterOption(CharacterOption.ListenToGeneralChat, true);
            player.SetCharacterOption(CharacterOption.ListenToTradeChat, true);
            player.SetCharacterOption(CharacterOption.ListenToLFGChat, true);

            // Not official client defaults, might have been creation defaults however to avoid initial confusion about helm/cloak equipping
            player.SetCharacterOption(CharacterOption.ShowYourHelmOrHeadGear, true);
            player.SetCharacterOption(CharacterOption.ShowYourCloak, true);
        }

        public static void CharacterCreateSetDefaultCharacterPositions(Player player, uint startArea)
        {
            player.Location = CharacterPositionExtensions.StartingPosition(startArea);
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

            var worldObject = (Clothing)WorldObjectFactory.CreateNewWorldObject(weenie);

            worldObject.SetProperties((int)palette, shade);

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
            var book = (Book)WorldObjectFactory.CreateNewWorldObject("parchment");

            book.SetProperties("IOU", "An IOU for a missing database object.", "Sorry about that chief...", "ACEmulator", "prewritten");
            book.AddPage(player.Guid.Full, "ACEmulator", "prewritten", false, $"{missingWeenieId}\n\nSorry but the database does not have a weenie for weenieClassId #{missingWeenieId} so in lieu of that here is an IOU for that item.");

            player.TryAddToInventory(book);
        }
    }
}
