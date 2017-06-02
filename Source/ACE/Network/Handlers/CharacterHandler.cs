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

            // check the value of the welcome message. Only display it if it is not empty
            if (!String.IsNullOrEmpty(ConfigManager.Config.Server.Welcome))
            {
                session.Network.EnqueueSend(new GameEventPopupString(session, ConfigManager.Config.Server.Welcome));
            }

            LandblockManager.PlayerEnterWorld(session);
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

            DatabaseManager.Shard.DeleteOrRestore(Time.GetUnixTime() + 3600ul, cachedCharacter.Guid.Low);

            var result = await DatabaseManager.Shard.GetCharacters(session.Id);
            session.UpdateCachedCharacters(result);
            session.Network.EnqueueSend(new GameMessageCharacterList(result, session.Account));
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterRestore, SessionState.AuthConnected)]
        public static void CharacterRestore(ClientMessage message, Session session)
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

            DatabaseManager.Shard.DeleteOrRestore(0, guid.Low);

            session.Network.EnqueueSend(new GameMessageCharacterRestore(guid, cachedCharacter.Name, 0u));
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterCreate, SessionState.AuthConnected)]
        public static async void CharacterCreate(ClientMessage message, Session session)
        {
            string account = message.Payload.ReadString16L();
            if (account != session.Account)
                return;

            var reader = message.Payload;
            Appearance appearance = Appearance.FromNetowrk(reader);
            CharGen cg = CharGen.ReadFromDat();
            AceCharacter character = new AceCharacter(DatabaseManager.Shard.GetNextCharacterId());
            
            reader.Skip(4);   /* Unknown constant (1) */
            character.Heritage = reader.ReadUInt32();
            character.Gender = reader.ReadUInt32();

            // pull character data from the dat file
            SexCG sex = cg.HeritageGroups[(int)character.Heritage].SexList[(int)character.Gender];
            
            character.MotionTableId = sex.MotionTable;
            character.SoundTableId = sex.SoundTable;
            character.PhysicsTableId = sex.PhysicsTable;
            character.ModelTableId = sex.SetupID;
            character.PaletteId = sex.BasePalette;
            character.CombatTableId = sex.CombatTable;
            
            // not sure how to set these.  Optim says they're in the dat
            // character.SetDataIdProperty(PropertyDataId.EyesTexture, appearance.Nose);
            // character.SetDataIdProperty(PropertyDataId.NoseTexture, appearance.Nose);
            // character.SetDataIdProperty(PropertyDataId.MouthTexture, appearance.Mouth);

            // character.SetDataIdProperty(PropertyDataId.HairPalette, appearance.HairColor);
            // character.SetDataIdProperty(PropertyDataId.EyesPalette, appearance.EyeColor);
            // character.SetDataIdProperty(PropertyDataId.SkinPalette, appearance.EyeColor);

            // character.SetDataIdProperty(PropertyDataId.HeadObject, appearance.HairStyle);

            // junk data point
            var templateOption = reader.ReadUInt32();

            // stats
            character.StrengthAbility.Base = reader.ReadUInt32();
            character.EnduranceAbility.Base = reader.ReadUInt32();
            character.CoordinationAbility.Base = reader.ReadUInt32();
            character.QuicknessAbility.Base = reader.ReadUInt32();
            character.FocusAbility.Base = reader.ReadUInt32();
            character.SelfAbility.Base = reader.ReadUInt32();
            
            // data we don't care about
            uint characterSlot = reader.ReadUInt32();
            uint classId = reader.ReadUInt32();

            // characters start with max vitals
            character.Health.Current = character.Health.UnbuffedValue;
            character.Stamina.Current = character.Stamina.UnbuffedValue;
            character.Mana.Current = character.Mana.UnbuffedValue;

            character.TotalSkillCredits = 52;
            character.AvailableSkillCredits = 52;

            uint numOfSkills = reader.ReadUInt32();
            Skill skill;
            SkillStatus skillStatus;
            SkillCostAttribute skillCost;
            for (uint i = 0; i < numOfSkills; i++)
            {
                skill = (Skill)i;
                skillCost = skill.GetCost();
                skillStatus = (SkillStatus)reader.ReadUInt32();
                character.TrainSkill(skill, skillCost.TrainingCost);
                if (skillStatus == SkillStatus.Specialized)
                    character.SpecializeSkill(skill, skillCost.SpecializationCost);
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

            uint lowGuid = DatabaseManager.Shard.GetNextCharacterId();
            character.AceObjectId = lowGuid;
            character.AccountId = session.Id;

            if (!DatabaseManager.Shard.SaveObject(character))
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.DatabaseDown);
                return;
            }

            CharacterCreateSetDefaultCharacterOptions(character);
            CharacterCreateSetDefaultCharacterPositions(character);
            DatabaseManager.Shard.SaveCharacterOptions(character);
            // DatabaseManager.Shard.InitCharacterPositions(character);

            var guid = new ObjectGuid(lowGuid, GuidType.Player);
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
            character.Location = CharacterPositionExtensions.StartingPosition(character.AceObjectId);
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
