using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using log4net;

using ACE.Database;
using ACE.Database.Models.World;
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.WorldObjects;

namespace ACE.Server.Factories
{
    public static class PlayerFactory
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public enum CreateResult
        {
            Success,
            TooManySkillCreditsUsed,
            InvalidSkillRequested,
            FailedToTrainSkill,
            FailedToSpecializeSkill,
        }

        public static CreateResult Create(CharacterCreateInfo characterCreateInfo, Weenie weenie, ObjectGuid guid, uint accountId, out Player player)
        {
            var heritageGroup = DatManager.PortalDat.CharGen.HeritageGroups[characterCreateInfo.Heritage];

            player = new Player(weenie, guid, accountId);

            player.SetProperty(PropertyInt.HeritageGroup, (int)characterCreateInfo.Heritage);
            player.SetProperty(PropertyString.HeritageGroup, heritageGroup.Name);
            player.SetProperty(PropertyInt.Gender, (int)characterCreateInfo.Gender);
            player.SetProperty(PropertyString.Sex, characterCreateInfo.Gender == 1 ? "Male" : "Female");

            //player.SetProperty(PropertyDataId.Icon, cgh.IconImage); // I don't believe this is used anywhere in the client, but it might be used by a future custom launcher

            // pull character data from the dat file
            var sex = heritageGroup.Genders[(int)characterCreateInfo.Gender];

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
            player.Character.HairTexture = sex.GetHairTexture(characterCreateInfo.Apperance.HairStyle);
            player.Character.DefaultHairTexture = sex.GetDefaultHairTexture(characterCreateInfo.Apperance.HairStyle);
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
                    player.TryEquipObject(hat, hat.ValidLocations ?? 0);
                else
                    CreateIOU(player, sex.GetHeadgearWeenie(characterCreateInfo.Apperance.HeadgearStyle));
            }

            var shirt = GetClothingObject(sex.GetShirtWeenie(characterCreateInfo.Apperance.ShirtStyle), characterCreateInfo.Apperance.ShirtColor, characterCreateInfo.Apperance.ShirtHue);
            if (shirt != null)
                player.TryEquipObject(shirt, shirt.ValidLocations ?? 0);
            else
                CreateIOU(player, sex.GetShirtWeenie(characterCreateInfo.Apperance.ShirtStyle));

            var pants = GetClothingObject(sex.GetPantsWeenie(characterCreateInfo.Apperance.PantsStyle), characterCreateInfo.Apperance.PantsColor, characterCreateInfo.Apperance.PantsHue);
            if (pants != null)
                player.TryEquipObject(pants, pants.ValidLocations ?? 0);
            else
                CreateIOU(player, sex.GetPantsWeenie(characterCreateInfo.Apperance.PantsStyle));

            var shoes = GetClothingObject(sex.GetFootwearWeenie(characterCreateInfo.Apperance.FootwearStyle), characterCreateInfo.Apperance.FootwearColor, characterCreateInfo.Apperance.FootwearHue);
            if (shoes != null)
                player.TryEquipObject(shoes, shoes.ValidLocations ?? 0);
            else
                CreateIOU(player, sex.GetFootwearWeenie(characterCreateInfo.Apperance.FootwearStyle));

            string templateName = heritageGroup.Templates[characterCreateInfo.TemplateOption].Name;
            //player.SetProperty(PropertyString.Title, templateName);
            player.SetProperty(PropertyString.Template, templateName);
            player.AddTitle(heritageGroup.Templates[characterCreateInfo.TemplateOption].Title, true);

            // stats
            uint totalAttributeCredits = heritageGroup.AttributeCredits;
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

            if (usedAttributeCredits > heritageGroup.AttributeCredits)
                return CreateResult.TooManySkillCreditsUsed;

            // data we don't care about
            //characterCreateInfo.CharacterSlot;
            //characterCreateInfo.ClassId;

            // characters start with max vitals
            player.Health.Current = player.Health.Base;
            player.Stamina.Current = player.Stamina.Base;
            player.Mana.Current = player.Mana.Base;

            // set initial skill credit amount. 52 for all but "Olthoi", which have 68
            player.SetProperty(PropertyInt.AvailableSkillCredits, (int)heritageGroup.SkillCredits);

            for (int i = 0; i < characterCreateInfo.SkillAdvancementClasses.Count; i++)
            {
                var sac = characterCreateInfo.SkillAdvancementClasses[i];

                if (sac == SkillAdvancementClass.Inactive)
                    continue;

                if (!DatManager.PortalDat.SkillTable.SkillBaseHash.ContainsKey((uint)i))
                {
                    log.ErrorFormat("Character {0} tried to create with skill {1} that was not found in Portal dat.", characterCreateInfo.Name, i);
                    return CreateResult.InvalidSkillRequested;
                }

                var skill = DatManager.PortalDat.SkillTable.SkillBaseHash[(uint)i];

                var trainedCost = skill.TrainedCost;
                var specializedCost = skill.UpgradeCostFromTrainedToSpecialized;

                foreach (var skillGroup in heritageGroup.Skills)
                {
                    if (skillGroup.SkillNum == i)
                    {
                        trainedCost = skillGroup.NormalCost;
                        specializedCost = skillGroup.PrimaryCost;
                        break;
                    }
                }

                if (sac == SkillAdvancementClass.Specialized)
                {
                    if (!player.TrainSkill((Skill)i, trainedCost))
                        return CreateResult.FailedToTrainSkill;
                    if (!player.SpecializeSkill((Skill)i, specializedCost))
                        return CreateResult.FailedToSpecializeSkill;
                }
                else if (sac == SkillAdvancementClass.Trained)
                {
                    if (!player.TrainSkill((Skill)i, trainedCost))
                        return CreateResult.FailedToTrainSkill;
                }
                else if (sac == SkillAdvancementClass.Untrained)
                    player.UntrainSkill((Skill) i, 0);
            }

            // grant starter items based on skills
            var starterGearConfig = StarterGearFactory.GetStarterGearConfiguration();
            var grantedWeenies = new List<uint>();

            foreach (var skillGear in starterGearConfig.Skills)
            {
                var charSkill = player.Skills[(Skill)skillGear.SkillId];
                if (charSkill.AdvancementClass == SkillAdvancementClass.Trained || charSkill.AdvancementClass == SkillAdvancementClass.Specialized)
                {
                    foreach (var item in skillGear.Gear)
                    {
                        if (grantedWeenies.Contains(item.WeenieId))
                        {
                            var existingItem = player.Inventory.Values.FirstOrDefault(i => i.WeenieClassId == item.WeenieId);
                            if (existingItem == null || (existingItem.MaxStackSize ?? 1) <= 1)
                                continue;

                            existingItem.SetStackSize(existingItem.StackSize + item.StackSize);
                            continue;
                        }

                        var loot = WorldObjectFactory.CreateNewWorldObject(item.WeenieId);
                        if (loot != null)
                        {
                            if (loot.StackSize.HasValue && loot.MaxStackSize.HasValue)
                                loot.SetStackSize((item.StackSize <= loot.MaxStackSize) ? item.StackSize : loot.MaxStackSize);
                        }
                        else
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

                                existingItem.SetStackSize(existingItem.StackSize + item.StackSize);
                                continue;
                            }

                            var loot = WorldObjectFactory.CreateNewWorldObject(item.WeenieId);
                            if (loot != null)
                            {
                                if (loot.StackSize.HasValue && loot.MaxStackSize.HasValue)
                                    loot.SetStackSize((item.StackSize <= loot.MaxStackSize) ? item.StackSize : loot.MaxStackSize);
                            }
                            else
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

                        if (charSkill.AdvancementClass == SkillAdvancementClass.Trained && spell.SpecializedOnly == false)
                            player.AddKnownSpell(spell.SpellId);
                        else if (charSkill.AdvancementClass == SkillAdvancementClass.Specialized)
                            player.AddKnownSpell(spell.SpellId);
                    }
                }
            }

            player.Name = characterCreateInfo.Name;
            player.Character.Name = characterCreateInfo.Name;


            // Index used to determine the starting location
            var startArea = characterCreateInfo.StartArea;

            var starterArea = DatManager.PortalDat.CharGen.StarterAreas[(int)startArea];

            player.Location = new Position(starterArea.Locations[0].ObjCellID,
                starterArea.Locations[0].Frame.Origin.X, starterArea.Locations[0].Frame.Origin.Y, starterArea.Locations[0].Frame.Origin.Z,
                starterArea.Locations[0].Frame.Orientation.X, starterArea.Locations[0].Frame.Orientation.Y, starterArea.Locations[0].Frame.Orientation.Z, starterArea.Locations[0].Frame.Orientation.W);

            player.Instantiation = new Position(player.Location);
            player.Sanctuary = new Position(player.Location);

            if (player is Sentinel || player is Admin)
            {
                player.Character.IsPlussed = true;
                player.CloakStatus = CloakStatus.Off;
            }

            CharacterCreateSetDefaultCharacterOptions(player);

            return CreateResult.Success;
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


        /// <summary>
        /// 100 Strength/Cord/Quick
        /// trained Creature/Item/Life/Mana Conversion todo remove creature/item/life because it adds foci. Add it after the player is created. Augmentations will take care of the foci requirements
        /// trained Magic/Melee Defense
        /// </summary>
        private static readonly byte[] baseGearKnight1 =
        {
            0x01, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
            0xFF, 0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0xF0, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0xF0, 0x3F, 0x88, 0x2E, 0x44, 0x17, 0xA2, 0x0B,
            0xD1, 0x3F, 0xC7, 0xBF, 0xE3, 0xDF, 0xF1, 0xEF, 0xE8, 0x3F, 0xD4, 0x1E, 0x6A, 0x0F, 0xB5, 0x87, 0xDA, 0x3F,
            0xAD, 0x76, 0x56, 0x3B, 0xAB, 0x9D, 0xD5, 0x3F, 0x00, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x0A, 0x00,
            0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x64, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00, 0x0A, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x37, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00,
            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00,
            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00,
            0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00,
            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00,
            0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00, 0x07, 0x00,
            0x4E, 0x6F, 0x20, 0x4E, 0x61, 0x6D, 0x65, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00
        };

        /// <summary>
        /// Creates a fully leveled/augmented 275 Heavy Weapons character player
        /// </summary>
        public static Player Create275HeavyWeapons(Weenie weenie, ObjectGuid guid, uint accountId, string name)
        {
            var characterCreateInfo = new CharacterCreateInfo();

            using (var memoryStream = new MemoryStream(baseGearKnight1))
            using (var binaryReader = new BinaryReader(memoryStream))
                characterCreateInfo.Unpack(binaryReader);

            characterCreateInfo.Name = name;

            Create(characterCreateInfo, weenie, guid, accountId, out var player);

            LevelUpPlayer(player);

            // todo add the creature/item/life skills here and remove them from the above base array once we have augmentations added in LevelUpPlayer()

            // Specialize Heavy Weapon specific skills
            player.TrainSkill(Skill.HeavyWeapons, 6);
            player.SpecializeSkill(Skill.HeavyWeapons, 6);
            player.TrainSkill(Skill.Healing, 6);
            player.SpecializeSkill(Skill.Healing, 4);
            player.TrainSkill(Skill.Shield, 2);
            player.SpecializeSkill(Skill.Shield, 2);

            // Raise base skills
            player.TrainSkill(Skill.MissileDefense, 6);
            player.SpecializeSkill(Skill.MeleeDefense, 10);
            player.SpecializeSkill(Skill.MagicDefense, 12);

            // todo 0 skill points. When we add the 4 skill points in LevelUpPlayer, we can spend them here

            // todo aug endurance

            SpendAllXp(player);

            AddCommonEquipment(player);
            AddCommonInventory(player);

            // todo Give the character skill appropriate weapons

            AddAllSpells(player);

            return player;
        }

        private static void LevelUpPlayer(Player player)
        {
            player.AvailableExperience += 191226310247;
            player.TotalExperience += 191226310247;
            player.Level = 275;
            player.AvailableSkillCredits += 46;
            player.TotalSkillCredits += 46;

            // todo add spec arcane lore quest flag + spec arcane lore

            // todo add Hunting Aun Ralirea quest flag + skill credit
            // todo add Chasing Oswald quest flag + skill credit

            // todo add all augmentations except the element protection and attribute raising ones

            // todo add Luminance quest flags + 2 luminance quest flags + skill credits
        }

        private static void SpendAllXp(Player player)
        {
            player.SpendAllXp(false);

            player.Health.Current = player.Health.MaxValue;
            player.Stamina.Current = player.Stamina.MaxValue;
            player.Mana.Current = player.Mana.MaxValue;
        }

        private static void AddCommonEquipment(Player player)
        {
            // todo Armor that covers everything + has all spells
        }

        private static void AddCommonInventory(Player player)
        {
            // MMD
            AddWeeniesToInventory(player, new HashSet<uint> { 20630 });

            // Spell Components
            AddWeeniesToInventory(player, new HashSet<uint> { 691, 689, 686, 688, 687, 690, 8897, 7299, 37155, 20631 });

            // Focusing Stone
            AddWeeniesToInventory(player, new HashSet<uint> { 8904 });

            // todo Drudge Scrying Orb

            // todo Buffing wand that has all defenses maxed
        }

        private static void AddWeeniesToInventory(Player player, HashSet<uint> weenieIds, ushort? stackSize = null)
        {
            foreach (uint weenieId in weenieIds)
            {
                var loot = WorldObjectFactory.CreateNewWorldObject(weenieId);

                if (loot == null) // weenie doesn't exist
                    continue;

                if (stackSize == null)
                    stackSize = loot.MaxStackSize;

                if (stackSize > 1)
                    loot.SetStackSize(stackSize);

                player.TryAddToInventory(loot);
            }
        }

        private static void AddAllSpells(Player player)
        {
            for (uint spellLevel = 1; spellLevel <= 8; spellLevel++)
            {
                player.LearnSpellsInBulk(MagicSchool.CreatureEnchantment, spellLevel, false);
                player.LearnSpellsInBulk(MagicSchool.ItemEnchantment, spellLevel, false);
                player.LearnSpellsInBulk(MagicSchool.LifeMagic, spellLevel, false);
                player.LearnSpellsInBulk(MagicSchool.VoidMagic, spellLevel, false);
                player.LearnSpellsInBulk(MagicSchool.WarMagic, spellLevel, false);
            }
        }
    }
}
