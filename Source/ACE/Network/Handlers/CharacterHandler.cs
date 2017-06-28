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
        public static async void CharacterDelete(ClientMessage message, Session session)
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

            if (await DatabaseManager.Shard.DeleteOrRestore(Time.GetUnixTime() + 3600ul, cachedCharacter.Guid.Full))
            {
                var result = await DatabaseManager.Shard.GetCharacters(session.Id);
                session.UpdateCachedCharacters(result);
                session.Network.EnqueueSend(new GameMessageCharacterList(result, session.Account));
                return;
            }
            session.SendCharacterError(CharacterError.Delete);
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterRestore, SessionState.AuthConnected)]
        public static async void CharacterRestore(ClientMessage message, Session session)
        {
            ObjectGuid guid = message.Payload.ReadGuid();

            var cachedCharacter = session.AccountCharacters.SingleOrDefault(c => c.Guid.Full == guid.Full);
            if (cachedCharacter == null)
                return;

            bool isAvailable = DatabaseManager.Shard.IsCharacterNameAvailable(cachedCharacter.Name);
            if (!isAvailable)
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);    /* Name already in use. */
                return;
            }

            if (await DatabaseManager.Shard.DeleteOrRestore(0, guid.Full))
            {
                session.Network.EnqueueSend(new GameMessageCharacterRestore(guid, cachedCharacter.Name, 0u));
                return;
            }
            SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Corrupt);
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterCreate, SessionState.AuthConnected)]
        public static async void CharacterCreate(ClientMessage message, Session session)
        {
            string account = message.Payload.ReadString16L();
            if (account != session.Account)
                return;

            var reader = message.Payload;
            CharGen cg = CharGen.ReadFromDat();
            AceCharacter character = new AceCharacter(DatabaseManager.Shard.GetNextCharacterId());

            // FIXME(ddevec): These should go in AceCharacter constructor, but the Network enum is not available there
            character.Radar = (byte)Network.Enum.RadarBehavior.ShowAlways;
            character.BlipColor = (byte)Network.Enum.RadarColor.White;
            character.ItemUseable = (uint)Network.Enum.Usable.UsableObjectSelf;

            reader.Skip(4);   /* Unknown constant (1) */
            character.Heritage = reader.ReadUInt32();
            character.Gender = reader.ReadUInt32();
            Appearance appearance = Appearance.FromNetwork(reader);

            // pull character data from the dat file
            SexCG sex = cg.HeritageGroups[(int)character.Heritage].SexList[(int)character.Gender];

            character.MotionTableId = sex.MotionTable;
            character.SoundTableId = sex.SoundTable;
            character.PhysicsTableId = sex.PhysicsTable;
            character.ModelTableId = sex.SetupID;
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
                character.ModelTableId = hairstyle.AlternateSetup;

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
            ushort skinPal = (ushort)skinPalSet.GetPaletteID(appearance.SkinHue);
            character.SkinPalette = skinPal;
            // AddPalette(skinPal, 0x0, 0x18); // for reference on how to apply

            // Hair is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
            PaletteSet hairPalSet = PaletteSet.ReadFromDat(sex.HairColorList[Convert.ToInt32(appearance.HairColor)]);
            ushort hairPal = (ushort)hairPalSet.GetPaletteID(appearance.HairHue);

            character.HairPalette = hairPal;
            // AddPalette(hairPal, 0x18, 0x8); // for reference on how to apply

            // Eye Color
            character.EyesPalette = sex.EyeColorList[Convert.ToInt32(appearance.EyeColor)];
            // AddPalette(PropertyDataId.EyesPalette, 0x20, 0x8); // for reference on how to apply

            if (appearance.HeadgearStyle < 0xFFFFFFFF) // No headgear is max UINT
            {
                // TODO - Create Inventory Item
                uint headgearWeenie = sex.GetHeadgearWeenie(appearance.HeadgearStyle);
                ClothingTable headCT = ClothingTable.ReadFromDat(sex.GetHeadgearClothingTable(appearance.HeadgearStyle));
                uint headgearIconId = headCT.GetIcon(appearance.HeadgearColor);
                // TODO - Apply the chosen color palette(s) (read from the ClothingTable)
            }

            // TODO - Create Inventory Item
            uint shirtWeenie = sex.GetShirtWeenie(appearance.ShirtStyle);
            ClothingTable shirtCT = ClothingTable.ReadFromDat(sex.GetShirtClothingTable(appearance.ShirtStyle));
            uint shirtIconId = shirtCT.GetIcon(appearance.ShirtColor);
            // TODO - Apply the chosen color palette(s) (read from the ClothingTable)

            // TODO - Create Inventory Item
            uint pantsWeenie = sex.GetPantsWeenie(appearance.PantsStyle);
            ClothingTable pantsCT = ClothingTable.ReadFromDat(sex.GetPantsClothingTable(appearance.PantsStyle));
            uint pantsIconId = pantsCT.GetIcon(appearance.PantsColor);
            // TODO - Apply the chosen color palette(s) (read from the ClothingTable)

            // TODO - Create Inventory Item
            uint footwearWeenie = sex.GetFootwearWeenie(appearance.FootwearStyle);
            ClothingTable footwearCT = ClothingTable.ReadFromDat(sex.GetFootwearClothingTable(appearance.FootwearStyle));
            uint footwearIconId = footwearCT.GetIcon(appearance.FootwearColor);
            // TODO - Apply the chosen color palette(s) (read from the ClothingTable)

            // Profession (Adventurer, Bow Hunter, etc)
            // TODO - Add this title to the available titles for this character.
            var templateOption = reader.ReadInt32();
            string templateName = cg.HeritageGroups[(int)character.Heritage].TemplateList[templateOption].Name;
            character.SetStringProperty(PropertyString.Title, templateName);

            // stats
            // TODO - Validate this is equal to 330 (Total Attribute Credits)
            character.StrengthAbility.Base = (ushort)reader.ReadUInt32();
            character.EnduranceAbility.Base = (ushort)reader.ReadUInt32();
            character.CoordinationAbility.Base = (ushort)reader.ReadUInt32();
            character.QuicknessAbility.Base = (ushort)reader.ReadUInt32();
            character.FocusAbility.Base = (ushort)reader.ReadUInt32();
            character.SelfAbility.Base = (ushort)reader.ReadUInt32();

            // data we don't care about
            uint characterSlot = reader.ReadUInt32();
            uint classId = reader.ReadUInt32();

            // characters start with max vitals
            character.Health.Current = AbilityExtensions.GetFormula(Entity.Enum.Ability.Health).CalcBase(character);
            character.Stamina.Current = AbilityExtensions.GetFormula(Entity.Enum.Ability.Stamina).CalcBase(character);
            character.Mana.Current = AbilityExtensions.GetFormula(Entity.Enum.Ability.Mana).CalcBase(character);

            uint numOfSkills = reader.ReadUInt32();
            Skill skill;
            SkillStatus skillStatus;
            SkillCostAttribute skillCost;
            for (uint i = 0; i < numOfSkills; i++)
            {
                skill = (Skill)i;
                skillCost = skill.GetCost();
                skillStatus = (SkillStatus)reader.ReadUInt32();
                // character.TrainSkill(skill, skillCost.TrainingCost);
                // if (skillStatus == SkillStatus.Specialized)
                //     character.SpecializeSkill(skill, skillCost.SpecializationCost);
                if (skillStatus == SkillStatus.Specialized)
                {
                    character.TrainSkill(skill, skillCost.TrainingCost);
                    character.SpecializeSkill(skill, skillCost.SpecializationCost);
                }
                if (skillStatus == SkillStatus.Trained)
                    character.TrainSkill(skill, skillCost.TrainingCost);
            }

            character.Name = reader.ReadString16L();

            // currently not used
            uint startArea = reader.ReadUInt32();

            character.IsAdmin = Convert.ToBoolean(reader.ReadUInt32());
            character.IsEnvoy = Convert.ToBoolean(reader.ReadUInt32());

            bool isAvailable = DatabaseManager.Shard.IsCharacterNameAvailable(character.Name);
            if (!isAvailable)
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);
                return;
            }

            character.AccountId = session.Id;

            CharacterCreateSetDefaultCharacterOptions(character);
            CharacterCreateSetDefaultCharacterPositions(character);

            // We must await here -- 
            bool saveSuccess = await DbManager.SaveObject(character);

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

        public static void CharacterCreateSetDefaultCharacterPositions(AceCharacter character)
        {
            character.Location = CharacterPositionExtensions.StartingPosition();
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
