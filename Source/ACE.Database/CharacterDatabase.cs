using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using MySql.Data.MySqlClient;

namespace ACE.Database
{
    using System.Runtime.CompilerServices;

    public class CharacterDatabase : Database, ICharacterDatabase
    {
        private enum CharacterPreparedStatement
        {
            CharacterDeleteOrRestore,
            CharacterInsert,
            CharacterSelect,
            CharacterSelectByName,
            CharacterRename,
            CharacterAppearanceInsert,
            CharacterStatsInsert,
            CharacterSkillsInsert,
            CharacterStartupGearInsert,
            CharacterListSelect,
            CharacterMaxIndex,

            CharacterUniqueNameSelect,
            CharacterSkillsSelect,
            CharacterStatsSelect,
            CharacterAppearanceSelect,
            CharacterFriendsSelect,
            CharacterFriendInsert,
            CharacterFriendDelete,
            CharacterFriendsRemoveAll,
            CharacterOptionsUpdate,

            CharacterPositionSelect,
            CharacterPositionInsert,
            CharacterPositionUpdate,
            CharacterPositionList,

            CharacterPropertiesBoolSelect,
            CharacterPropertiesIntSelect,
            CharacterPropertiesBigIntSelect,
            CharacterPropertiesDoubleSelect,
            CharacterPropertiesStringSelect,

            CharacterPropertiesBoolInsert,
            CharacterPropertiesIntInsert,
            CharacterPropertiesBigIntInsert,
            CharacterPropertiesDoubleInsert,
            CharacterPropertiesStringInsert,

            CharacterPropertiesBoolUpdate,
            CharacterPropertiesIntUpdate,
            CharacterPropertiesBigIntUpdate,
            CharacterPropertiesDoubleUpdate,
            CharacterPropertiesStringUpdate,

            CharacterStatsUpdate,
            CharacterSkillsUpdate,

            CharacterStartupGearSelect,
            GetAceCharacter
        }

        protected override Type PreparedStatementType => typeof(CharacterPreparedStatement);

        protected override void InitializePreparedStatements()
        {
            AddPreparedStatement(CharacterPreparedStatement.CharacterMaxIndex, "SELECT MAX(`guid`) FROM `character`;");
            AddPreparedStatement(CharacterPreparedStatement.CharacterUniqueNameSelect, "SELECT COUNT(`name`) as cnt FROM `character` WHERE NOT `deleted` = 1 AND `deleteTime` = 0 AND BINARY `name` = ?;", MySqlDbType.VarString);
            AddPreparedStatement(CharacterPreparedStatement.CharacterInsert, "INSERT INTO `character` (`guid`, `accountId`, `name`, `templateOption`, `startArea`) VALUES (?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.VarString, MySqlDbType.UByte, MySqlDbType.UByte);
            AddPreparedStatement(CharacterPreparedStatement.CharacterAppearanceInsert, "INSERT INTO `character_appearance` (`id`, `eyes`, `nose`, `mouth`, `eyeColor`, `hairColor`, `hairStyle`, `hairHue`, `skinHue`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.Double);
            AddPreparedStatement(CharacterPreparedStatement.CharacterStatsInsert, "INSERT INTO `character_stats` (`id`, `strength`, `endurance`, `coordination`, `quickness`, `focus`, `self`, `healthCurrent`, `staminaCurrent`, `manaCurrent`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte);
            AddPreparedStatement(CharacterPreparedStatement.CharacterSkillsInsert, "INSERT INTO `character_skills` (`id`, `skillId`, `skillStatus`, `skillPoints`) VALUES (?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UInt16);
            AddPreparedStatement(CharacterPreparedStatement.CharacterStartupGearInsert, "INSERT INTO `character_startup_gear` (`id`, `headgearStyle`, `headgearColor`, `headgearHue`, `shirtStyle`, `shirtColor`, `shirtHue`, `pantsStyle`, `pantsColor`, `pantsHue`, `footwearStyle`, `footwearColor`, `footwearHue`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double);
            AddPreparedStatement(CharacterPreparedStatement.CharacterDeleteOrRestore, "UPDATE `character` SET `deleteTime` = ?, `deleted` = 0 WHERE `guid` = ?;", MySqlDbType.UInt64, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterListSelect, "SELECT `guid`, `name`, `deleteTime` FROM `character` WHERE `accountId` = ? AND `deleted` = 0 ORDER BY `lastUpdate` DESC;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterFriendInsert, "INSERT INTO `character_friends` (`id`, `friendId`) VALUES (?, ?);", MySqlDbType.UInt32, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterFriendDelete, "DELETE FROM  `character_friends` WHERE `id` = ? AND `friendId` = ?;", MySqlDbType.UInt32, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterFriendsRemoveAll, "DELETE FROM  `character_friends` WHERE `id` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterSelectByName, "SELECT `guid`, `accountId`, `name`, `templateOption`, `startArea` FROM `character` WHERE `deleted` = 0 AND `deleteTime` = 0 AND `name` = ?;", MySqlDbType.VarString);
            AddPreparedStatement(CharacterPreparedStatement.CharacterOptionsUpdate, "UPDATE `character` SET `totalLogins` = ?, `characterOptions1` = ?, `characterOptions2` = ? WHERE guid = ?", MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterRename, "UPDATE `character` SET `name` = ? WHERE `guid` = ?;", MySqlDbType.VarString, MySqlDbType.UInt32);
            ConstructStatement(CharacterPreparedStatement.GetAceCharacter, typeof(AceCharacter), ConstructedStatementType.Get);

            // world entry

            AddPreparedStatement(CharacterPreparedStatement.CharacterSelect, "SELECT `guid`, `accountId`, `name`, `templateOption`, `startArea`, `totalLogins`, `characterOptions1`, `characterOptions2` FROM `character` WHERE `guid` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterSkillsSelect, "SELECT `skillId`, `skillStatus`, `skillPoints`, `skillXpSpent` FROM `character_skills` WHERE `id` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterStatsSelect, "SELECT `strength`, `strengthXpSpent`, `strengthRanks`, `endurance`, `enduranceXpSpent`, `enduranceRanks`, `coordination`, `coordinationXpSpent`, `coordinationRanks`, `quickness`,  `quicknessXpSpent`, `quicknessRanks`, `focus`, `focusXpSpent`, `focusRanks`, `self`, `selfXpSpent`, `selfRanks`, `healthRanks`, `healthXpSpent`, `healthCurrent`, `staminaRanks`, `staminaXpSpent`, `staminaCurrent`, `manaRanks`, `manaXpSpent`, `manaCurrent` FROM `character_stats` WHERE `id` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterAppearanceSelect, "SELECT  `eyes`, `nose`, `mouth`, `eyeColor`, `hairColor`, `hairStyle`, `hairHue`, `skinHue` FROM `character_appearance` WHERE `id` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterFriendsSelect, "SELECT cf.`friendId`, c.`name` FROM `character_friends` cf JOIN `character` c ON (cf.`friendId` = c.`guid`) WHERE cf.`id` = ?;", MySqlDbType.UInt32);

            ConstructStatement(CharacterPreparedStatement.CharacterPositionSelect, typeof(Position), ConstructedStatementType.Get);
            ConstructStatement(CharacterPreparedStatement.CharacterPositionInsert, typeof(Position), ConstructedStatementType.Insert);
            ConstructStatement(CharacterPreparedStatement.CharacterPositionUpdate, typeof(Position), ConstructedStatementType.Update);
            ConstructStatement(CharacterPreparedStatement.CharacterPositionList, typeof(Position), ConstructedStatementType.GetList);

            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolSelect, "SELECT `propertyId`, `propertyValue` FROM `character_properties_bool` WHERE `guid` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesIntSelect, "SELECT `propertyId`, `propertyValue` FROM `character_properties_int` WHERE `guid` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesBigIntSelect, "SELECT `propertyId`, `propertyValue` FROM `character_properties_bigint` WHERE `guid` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesDoubleSelect, "SELECT `propertyId`, `propertyValue` FROM `character_properties_double` WHERE `guid` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesStringSelect, "SELECT `propertyId`, `propertyValue` FROM `character_properties_string` WHERE `guid` = ?;", MySqlDbType.UInt32);

            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolInsert, "INSERT INTO `character_properties_bool` (`guid`, `propertyId`, `propertyValue`) VALUES (?, ?, ?) ON DUPLICATE KEY UPDATE `propertyValue` = VALUES(`propertyValue`);", MySqlDbType.UInt32, MySqlDbType.UInt16, MySqlDbType.Bit);
            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesIntInsert, "INSERT INTO `character_properties_int` (`guid`, `propertyId`, `propertyValue`) VALUES (?, ?, ?) ON DUPLICATE KEY UPDATE `propertyValue` = VALUES(`propertyValue`);", MySqlDbType.UInt32, MySqlDbType.UInt16, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesBigIntInsert, "INSERT INTO `character_properties_bigint` (`guid`, `propertyId`, `propertyValue`) VALUES (?, ?, ?) ON DUPLICATE KEY UPDATE `propertyValue` = VALUES(`propertyValue`);", MySqlDbType.UInt32, MySqlDbType.UInt16, MySqlDbType.UInt64);
            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesDoubleInsert, "INSERT INTO `character_properties_double` (`guid`, `propertyId`, `propertyValue`) VALUES (?, ?, ?) ON DUPLICATE KEY UPDATE `propertyValue` = VALUES(`propertyValue`);", MySqlDbType.UInt32, MySqlDbType.UInt16, MySqlDbType.Double);
            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesStringInsert, "INSERT INTO `character_properties_string` (`guid`, `propertyId`, `propertyValue`) VALUES (?, ?, ?) ON DUPLICATE KEY UPDATE `propertyValue` = VALUES(`propertyValue`);", MySqlDbType.UInt32, MySqlDbType.UInt16, MySqlDbType.VarChar);

            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolUpdate, "UPDATE `character_properties_bool` SET `propertyValue`=? WHERE `propertyId`=? AND `guid`=?;", MySqlDbType.Bit, MySqlDbType.UInt16, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesIntUpdate, "UPDATE `character_properties_int` SET `propertyValue`=? WHERE `propertyId`=? AND `guid`=?;", MySqlDbType.UInt32, MySqlDbType.UInt16, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesBigIntUpdate, "UPDATE `character_properties_bigint` SET `propertyValue`=? WHERE `propertyId`=? AND `guid`=?;", MySqlDbType.UInt64, MySqlDbType.UInt16, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesDoubleUpdate, "UPDATE `character_properties_double` SET `propertyValue`=? WHERE `propertyId`=? AND `guid`=?;", MySqlDbType.Double, MySqlDbType.UInt16, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesStringUpdate, "UPDATE `character_properties_string` SET `propertyValue`=? WHERE `propertyId`=? AND `guid`=?;", MySqlDbType.VarChar, MySqlDbType.UInt16, MySqlDbType.UInt32);

            AddPreparedStatement(CharacterPreparedStatement.CharacterStatsUpdate, "UPDATE `character_stats` SET `strengthXpSpent` = ?, `strengthRanks` = ?, `enduranceXpSpent` = ?, `enduranceRanks` = ?, `coordinationXpSpent` = ?, `coordinationRanks` = ?, `quicknessXpSpent` = ?, `quicknessRanks` = ?, `focusXpSpent` = ?, `focusRanks` = ?, `selfXpSpent` = ?, `selfRanks` = ?, `healthCurrent`= ?, `healthXpSpent` = ?, `healthRanks` = ?, `staminaCurrent` = ?, `staminaXpSpent` = ?, `staminaRanks` = ?, `manaCurrent` = ?, `manaXpSpent` = ?, `manaRanks` = ? WHERE `id` = ?;", MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterSkillsUpdate, "UPDATE `character_skills` SET `skillStatus` = ?, `skillPoints` = ?, `skillXpSpent` = ? WHERE `id` = ? AND `skillId` = ?;", MySqlDbType.UByte, MySqlDbType.UInt16, MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UByte);

            AddPreparedStatement(CharacterPreparedStatement.CharacterStartupGearSelect, "SELECT `headgearStyle`, `headgearColor`, `headgearHue`, `shirtStyle`, `shirtColor`, `shirtHue`, `pantsStyle`, `pantsColor`, `pantsHue`, `footwearStyle`, `footwearColor`, `footwearHue` FROM `character_startup_gear` WHERE `id` = ?;", MySqlDbType.UInt32);
        }

        public AceCharacter GetAceCharacter(uint guid)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object> { { "guid", guid } };
            AceCharacter aceCharacter = new AceCharacter();

            if (ExecuteConstructedGetStatement(CharacterPreparedStatement.GetAceCharacter, typeof(AceCharacter), criteria, aceCharacter))
            {
                return aceCharacter;
            }
            return null;
        }

        public uint GetMaxId()
        {
            MySqlResult result = SelectPreparedStatement(CharacterPreparedStatement.CharacterMaxIndex);
            Debug.Assert(result != null, "Invalid prepared statement value.");
            return result.Read<uint>(0, "MAX(`guid`)") + 1;
        }

        /// <summary>
        /// Returns a CharacterPosition object containing the physical location of a player/character.
        /// </summary>
        public Position GetLocation(uint id)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>
                                                      {
                                                          { "character_id", id },
                                                          { "positionType", PositionType.Location }
                                                      };

            Position newCharacterPosition = CharacterPositionExtensions.StartingPosition(id);

            if (ExecuteConstructedGetStatement(CharacterPreparedStatement.CharacterPositionSelect, typeof(Position), criteria, newCharacterPosition))
                return newCharacterPosition;

            newCharacterPosition = CharacterPositionExtensions.StartingPosition(id);

            // Save new position
            ExecuteConstructedInsertStatement(CharacterPreparedStatement.CharacterPositionInsert, typeof(Position), newCharacterPosition);

            return newCharacterPosition;
        }

        public async Task<List<CachedCharacter>> GetByAccount(uint accountId)
        {
            MySqlResult result = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterListSelect, accountId);
            List<CachedCharacter> characters = new List<CachedCharacter>();

            for (byte i = 0; i < result.Count; i++)
            {
                uint lowGuid     = result.Read<uint>(i, "guid");
                string name      = result.Read<string>(i, "name");
                ulong deleteTime = result.Read<ulong>(i, "deleteTime");

                characters.Add(new CachedCharacter(new ObjectGuid(lowGuid, GuidType.Player), i, name, deleteTime));
            }

            return characters;
        }

        public void DeleteOrRestore(ulong unixTime, uint id)
        {
            ExecutePreparedStatement(CharacterPreparedStatement.CharacterDeleteOrRestore, unixTime, id);
        }

        public bool IsNameAvailable(string name)
        {
            MySqlResult result = SelectPreparedStatement(CharacterPreparedStatement.CharacterUniqueNameSelect, name);
            Debug.Assert(result != null, "Invalid prepared statement value.");

            uint charsWithName = result.Read<uint>(0, "cnt");
            return (charsWithName == 0);
        }

        public async Task UpdateCharacter(Character character)
        {
            // Save all of the player positions
            // TODO: Remove this after allowing positions to be saved on demand
            foreach (KeyValuePair<PositionType, Position> pos in character.Positions)
            {
                ExecuteConstructedUpdateStatement(CharacterPreparedStatement.CharacterPositionUpdate, typeof(Position), pos.Value);
            }

            DatabaseTransaction transaction = BeginTransaction();
            UpdateCharacterProperties(character, transaction);

            UpdateCharacterStats(character, transaction);

            UpdateCharacterSkills(character, transaction);

            await transaction.Commit();
        }

        public async Task<bool> CreateCharacter(Character character)
        {
            DatabaseTransaction transaction = BeginTransaction();
            transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterInsert,
                character.Id,
                character.AccountId,
                character.Name,
                character.TemplateOption,
                character.StartArea);

            transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterAppearanceInsert,
                character.Id,
                character.Appearance.Eyes,
                character.Appearance.Nose,
                character.Appearance.Mouth,
                character.Appearance.EyeColor,
                character.Appearance.HairColor,
                character.Appearance.HairStyle,
                character.Appearance.HairHue,
                character.Appearance.SkinHue);

            transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterStatsInsert,
                character.Id,
                character.StrengthAbility.Base,
                character.EnduranceAbility.Base,
                character.CoordinationAbility.Base,
                character.QuicknessAbility.Base,
                character.FocusAbility.Base,
                character.SelfAbility.Base,
                character.Health.Current,
                character.Stamina.Current,
                character.Mana.Current);

            foreach (CharacterSkill skill in character.Skills.Values)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterSkillsInsert,
                    character.Id,
                    (uint)skill.Skill,
                    (uint)skill.Status,
                    skill.Ranks);

            transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterStartupGearInsert,
                character.Id,
                character.Appearance.HeadgearStyle,
                character.Appearance.HeadgearColor,
                character.Appearance.HeadgearHue,
                character.Appearance.ShirtStyle,
                character.Appearance.ShirtColor,
                character.Appearance.ShirtHue,
                character.Appearance.PantsStyle,
                character.Appearance.PantsColor,
                character.Appearance.PantsHue,
                character.Appearance.FootwearStyle,
                character.Appearance.FootwearColor,
                character.Appearance.FootwearHue);

            SaveCharacterProperties(character, transaction);

            return await transaction.Commit();
        }

        public async Task<Character> LoadCharacter(uint id)
        {
            MySqlResult result = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterSelect, id);
            Character c = null;

            if (result?.Count > 0)
            {
                uint guid = result.Read<uint>(0, "guid");
                uint accountId = result.Read<uint>(0, "accountId");

                c = new Character(guid, accountId);
                c.Name = result.Read<string>(0, "name");
                c.TemplateOption = result.Read<uint>(0, "templateOption");
                c.StartArea = result.Read<uint>(0, "startArea");
                c.TotalLogins = result.Read<uint>(0, "totalLogins");

                // Loads the positions into the player object and resets the landlock id
                LoadCharacterPositions(c);

                // Load the best location from the database
                c.Location = c.Positions[PositionType.Location];

                uint characterOptions1Flag = result.Read<uint>(0, "characterOptions1");
                uint characterOptions2Flag = result.Read<uint>(0, "characterOptions2");
                LoadCharacterOptions(characterOptions1Flag, characterOptions2Flag, c);

                result = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterSkillsSelect, id);

                for (uint i = 0; i < result?.Count; i++)
                {
                    Skill s = result.Read<Skill>(i, "skillId");
                    SkillStatus ss = result.Read<SkillStatus>(i, "skillStatus");
                    uint ranks = result.Read<uint>(i, "skillPoints");
                    uint xpSpent = result.Read<uint>(i, "skillXpSpent");
                    c.Skills.Add(s, new CharacterSkill(c, s, ss, ranks, xpSpent));
                }

                result = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterAppearanceSelect, id);

                if (result?.Count > 0)
                {
                    c.Appearance = new Appearance();
                    c.Appearance.Eyes = result.Read<uint>(0, "eyes");
                    c.Appearance.Nose = result.Read<uint>(0, "nose");
                    c.Appearance.Mouth = result.Read<uint>(0, "mouth");
                    c.Appearance.EyeColor = result.Read<uint>(0, "eyeColor");
                    c.Appearance.HairColor = result.Read<uint>(0, "hairColor");
                    c.Appearance.HairStyle = result.Read<uint>(0, "hairStyle");
                    c.Appearance.HairHue = result.Read<double>(0, "hairHue");
                    c.Appearance.SkinHue = result.Read<double>(0, "skinHue");
                }

                // Get the character's startup gear.
                // TODO: This is a temporary solution. Ultimately, this table should be removed entirely and Startup gear should be created/saved in that system once available.
                result = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterStartupGearSelect, id);
                if (result?.Count > 0)
                {
                    c.Appearance.HeadgearStyle = result.Read<uint>(0, "headgearStyle");
                    c.Appearance.HeadgearColor = result.Read<uint>(0, "headgearColor");
                    c.Appearance.HeadgearHue = result.Read<float>(0, "headgearHue");
                    c.Appearance.ShirtStyle = result.Read<uint>(0, "shirtStyle");
                    c.Appearance.ShirtColor = result.Read<uint>(0, "shirtColor");
                    c.Appearance.ShirtHue = result.Read<float>(0, "shirtHue");
                    c.Appearance.PantsStyle = result.Read<uint>(0, "pantsStyle");
                    c.Appearance.PantsColor = result.Read<uint>(0, "pantsColor");
                    c.Appearance.PantsHue = result.Read<float>(0, "pantsHue");
                    c.Appearance.FootwearStyle = result.Read<uint>(0, "footwearStyle");
                    c.Appearance.FootwearColor = result.Read<uint>(0, "footwearColor");
                    c.Appearance.FootwearHue = result.Read<float>(0, "footwearHue");
                }

                result = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterStatsSelect, id);

                if (result?.Count > 0)
                {
                    c.StrengthAbility.Base = result.Read<uint>(0, "strength");
                    c.StrengthAbility.ExperienceSpent = result.Read<uint>(0, "strengthXpSpent");
                    c.StrengthAbility.Ranks = result.Read<uint>(0, "strengthRanks");
                    c.EnduranceAbility.Base = result.Read<uint>(0, "endurance");
                    c.EnduranceAbility.ExperienceSpent = result.Read<uint>(0, "enduranceXpSpent");
                    c.EnduranceAbility.Ranks = result.Read<uint>(0, "enduranceRanks");
                    c.CoordinationAbility.Base = result.Read<uint>(0, "coordination");
                    c.CoordinationAbility.ExperienceSpent = result.Read<uint>(0, "coordinationXpSpent");
                    c.CoordinationAbility.Ranks = result.Read<uint>(0, "coordinationRanks");
                    c.QuicknessAbility.Base = result.Read<uint>(0, "quickness");
                    c.QuicknessAbility.ExperienceSpent = result.Read<uint>(0, "quicknessXpSpent");
                    c.QuicknessAbility.Ranks = result.Read<uint>(0, "quicknessRanks");
                    c.FocusAbility.Base = result.Read<uint>(0, "focus");
                    c.FocusAbility.ExperienceSpent = result.Read<uint>(0, "focusXpSpent");
                    c.FocusAbility.Ranks = result.Read<uint>(0, "focusRanks");
                    c.SelfAbility.Base = result.Read<uint>(0, "self");
                    c.SelfAbility.ExperienceSpent = result.Read<uint>(0, "selfXpSpent");
                    c.SelfAbility.Ranks = result.Read<uint>(0, "selfRanks");

                    c.Health.Ranks = result.Read<uint>(0, "healthRanks");
                    c.Health.ExperienceSpent = result.Read<uint>(0, "healthXpSpent");
                    c.Health.Current = result.Read<uint>(0, "healthCurrent");
                    c.Stamina.Ranks = result.Read<uint>(0, "staminaRanks");
                    c.Stamina.ExperienceSpent = result.Read<uint>(0, "staminaXpSpent");
                    c.Stamina.Current = result.Read<uint>(0, "staminaCurrent");
                    c.Mana.Ranks = result.Read<uint>(0, "manaRanks");
                    c.Mana.ExperienceSpent = result.Read<uint>(0, "manaXpSpent");
                    c.Mana.Current = result.Read<uint>(0, "manaCurrent");
                }

                result = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterFriendsSelect, id);

                for (uint i = 0; i < result?.Count; i++)
                {
                    Friend f = new Friend();
                    f.Id = new ObjectGuid(result.Read<uint>(i, "friendId"), GuidType.Player);
                    f.Name = result.Read<string>(i, "name");

                    // Not sure we want the actually load these next two values.  They are passed in the packet, but their purpose is unknown.
                    f.FriendIdList = new List<ObjectGuid>();
                    f.FriendOfIdList = new List<ObjectGuid>();
                    c.AddFriend(f);
                }

                await LoadCharacterProperties(c);
            }

            return c;
        }

        private void LoadCharacterOptions(uint characterOptions1Flag, uint characterOptions2Flag, Character character)
        {
            List<CharacterOption> optionsToSetToTrue = new List<CharacterOption>(); // Need to use this since I can't change the collection while I enumerate over it.
            foreach (KeyValuePair<CharacterOption, bool> option in character.CharacterOptions)
            {
                if (option.Key.GetCharacterOptions1Attribute() != null)
                {
                    if ((characterOptions1Flag & (uint)option.Key.GetCharacterOptions1Attribute().Option) != 0)
                        optionsToSetToTrue.Add(option.Key);
                }
                else if (option.Key.GetCharacterOptions2Attribute() != null)
                {
                    if ((characterOptions2Flag & (uint)option.Key.GetCharacterOptions2Attribute().Option) != 0)
                        optionsToSetToTrue.Add(option.Key);
                }
            }

            foreach (CharacterOption option in optionsToSetToTrue)
            {
                character.SetCharacterOption(option, true);
            }
        }

        public void SaveCharacterOptions(Character character)
        {
            ExecutePreparedStatement(CharacterPreparedStatement.CharacterOptionsUpdate, character.TotalLogins, character.CharacterOptions.GetCharacterOptions1Flag(), character.CharacterOptions.GetCharacterOptions2Flag(), character.Id);
        }

        /// <summary>
        /// Saves a CharacterPosition, when the character is first created
        /// </summary>
        public void InitCharacterPositions(Character character)
        {
            foreach (Position position in character.Positions.Values)
            {
                ExecuteConstructedInsertStatement(CharacterPreparedStatement.CharacterPositionInsert, typeof(Position), position);
            }
        }

        public async Task LoadCharacterProperties(DbObject dbObject)
        {
            MySqlResult results = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterPropertiesBoolSelect, dbObject.Id);
            for (uint i = 0; i < results.Count; i++)
                dbObject.SetPropertyBool(results.Read<PropertyBool>(i, "propertyId"), results.Read<bool>(i, "propertyValue"));

            results = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterPropertiesIntSelect, dbObject.Id);
            for (uint i = 0; i < results.Count; i++)
                dbObject.SetPropertyInt(results.Read<PropertyInt>(i, "propertyId"), results.Read<uint>(i, "propertyValue"));

            results = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterPropertiesBigIntSelect, dbObject.Id);
            for (uint i = 0; i < results.Count; i++)
                dbObject.SetPropertyInt64(results.Read<PropertyInt64>(i, "propertyId"), results.Read<ulong>(i, "propertyValue"));

            results = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterPropertiesDoubleSelect, dbObject.Id);
            for (uint i = 0; i < results.Count; i++)
                dbObject.SetPropertyDouble(results.Read<PropertyDouble>(i, "propertyId"), results.Read<double>(i, "propertyValue"));

            results = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterPropertiesStringSelect, dbObject.Id);
            for (uint i = 0; i < results.Count; i++)
                dbObject.SetPropertyString(results.Read<PropertyString>(i, "propertyId"), results.Read<string>(i, "propertyValue"));
        }

        /// <summary>
        /// Returns all positions from the database in a dictionary. Positions for each character should always be unique.
        /// </summary>
        public Dictionary<PositionType, Position> GetCharacterPositions(Character character)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("character_id", character.Id);
            List<Position> availablePositions = ExecuteConstructedGetListStatement<CharacterPreparedStatement, Position>(CharacterPreparedStatement.CharacterPositionList, criteria);
            Dictionary<PositionType, Position> characterPositions = new Dictionary<PositionType, Position>();
            foreach (Position position in availablePositions)
            {
                characterPositions.Add(position.PositionType, position);
            }

            return characterPositions;
        }

        /// <summary>
        /// Returns an available character positions from the database that match the PositionType.
        /// If no position is found, an invalid position is returned.
        /// </summary>
        public Position GetCharacterPosition(Character character, PositionType positionType)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("character_id", character.Id);
            criteria.Add("positionType", positionType);
            Position newPosition = CharacterPositionExtensions.InvalidPosition(character.Id, positionType);
            if (ExecuteConstructedGetStatement(CharacterPreparedStatement.CharacterPositionSelect, typeof(Position), criteria, newPosition))
                return newPosition;
            return newPosition = CharacterPositionExtensions.InvalidPosition(character.Id, PositionType.Undef);
        }

        /// <summary>
        /// Attempts to load characters positions from the database. If no position is available, a new position will be created to allow the player to spawn.
        /// </summary>
        /// <remarks>
        /// TODO:
        ///     1. Confirm the order/position usage when restoring a position from database. Should this always be the lifestone if "Location" is unavailable?
        /// </remarks>
        public void LoadCharacterPositions(Character character)
        {
            // get a list of positions from the vw_character_positions view
            Dictionary<PositionType, Position> dbPositions = GetCharacterPositions(character);

            // Check for positions and insert position for the Position type Locaiont, if missing:
            if (dbPositions.Count == 0 || !dbPositions.ContainsKey(PositionType.Location))
            {
                Position swapToWorkingLocation = new Position();

                // This checks for the best location available from the database
                // When the first suitable position is found, it will be set as the current location.
                if (dbPositions.ContainsKey(PositionType.LastPortal))

                    // Use the lastportal position
                    swapToWorkingLocation = dbPositions[PositionType.LastPortal];
                else if (dbPositions.ContainsKey(PositionType.Sanctuary))

                    // Use lifestone recall position
                    swapToWorkingLocation = dbPositions[PositionType.Sanctuary];
                else
                {
                    // No suitable database position is available
                    swapToWorkingLocation = CharacterPositionExtensions.StartingPosition(character.Id);

                        // Use default position
                }

                // force the position type to location
                swapToWorkingLocation.PositionType = PositionType.Location;
                dbPositions.Add(swapToWorkingLocation.PositionType, swapToWorkingLocation);
                ExecuteConstructedInsertStatement(CharacterPreparedStatement.CharacterPositionInsert, typeof(Position), swapToWorkingLocation);
            }

            // This will load each available position from the database, into the object's positions
            foreach (KeyValuePair<PositionType, Position> position in dbPositions)
            {
                character.SetCharacterPosition(position.Value);
            }
        }

        /// <summary>
        /// Updates a position in the database. If no position is present, a new row will be created.
        /// </summary>
        public void SaveCharacterPosition(Character character, Position characterPosition)
        {
            Position currentPosition = GetCharacterPosition(character, characterPosition.PositionType);

            if (currentPosition.PositionType != PositionType.Undef)
            {
                ExecuteConstructedUpdateStatement(CharacterPreparedStatement.CharacterPositionUpdate, typeof(Position), characterPosition);
            }
            else
            {
                ExecuteConstructedInsertStatement(CharacterPreparedStatement.CharacterPositionInsert, typeof(Position), characterPosition);
            }
        }

        public void SaveCharacterProperties(DbObject dbObject, DatabaseTransaction transaction)
        {
            // known issue: properties that were removed from the bucket will not updated.  this is a problem if we
            // ever need to straight up "delete" a property.

            foreach (KeyValuePair<PropertyBool, bool> prop in dbObject.PropertiesBool)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolInsert, dbObject.Id, (ushort)prop.Key, prop.Value);

            foreach (KeyValuePair<PropertyInt, uint> prop in dbObject.PropertiesInt)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesIntInsert, dbObject.Id, (ushort)prop.Key, prop.Value);

            foreach (KeyValuePair<PropertyInt64, ulong> prop in dbObject.PropertiesInt64)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesBigIntInsert, dbObject.Id, (ushort)prop.Key, prop.Value);

            foreach (KeyValuePair<PropertyDouble, double> prop in dbObject.PropertiesDouble)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesDoubleInsert, dbObject.Id, (ushort)prop.Key, prop.Value);

            foreach (KeyValuePair<PropertyString, string> prop in dbObject.PropertiesString)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesStringInsert, dbObject.Id, (ushort)prop.Key, prop.Value);
        }

        public void UpdateCharacterProperties(DbObject dbObject, DatabaseTransaction transaction)
        {
            foreach (KeyValuePair<PropertyBool, bool> prop in dbObject.PropertiesBool)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolUpdate, prop.Value, (ushort)prop.Key, dbObject.Id);

            foreach (KeyValuePair<PropertyInt, uint> prop in dbObject.PropertiesInt)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesIntUpdate, prop.Value, (ushort)prop.Key, dbObject.Id);

            foreach (KeyValuePair<PropertyInt64, ulong> prop in dbObject.PropertiesInt64)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesBigIntUpdate, prop.Value, (ushort)prop.Key, dbObject.Id);

            foreach (KeyValuePair<PropertyDouble, double> prop in dbObject.PropertiesDouble)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesDoubleUpdate, prop.Value, (ushort)prop.Key, dbObject.Id);

            foreach (KeyValuePair<PropertyString, string> prop in dbObject.PropertiesString)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesStringUpdate, prop.Value, (ushort)prop.Key, dbObject.Id);
        }

        public void UpdateCharacterStats(Character character, DatabaseTransaction transaction)
        {
            transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterStatsUpdate,
                    character.StrengthAbility.ExperienceSpent, character.StrengthAbility.Ranks,
                    character.EnduranceAbility.ExperienceSpent, character.EnduranceAbility.Ranks,
                    character.CoordinationAbility.ExperienceSpent, character.CoordinationAbility.Ranks,
                    character.QuicknessAbility.ExperienceSpent, character.QuicknessAbility.Ranks,
                    character.FocusAbility.ExperienceSpent, character.FocusAbility.Ranks,
                    character.SelfAbility.ExperienceSpent, character.SelfAbility.Ranks,
                    character.Health.Current, character.Health.ExperienceSpent, character.Health.Ranks,
                    character.Stamina.Current, character.Stamina.ExperienceSpent, character.Stamina.Ranks,
                    character.Mana.Current, character.Mana.ExperienceSpent, character.Mana.Ranks,
                    character.Id);
        }

        public void UpdateCharacterSkills(Character character, DatabaseTransaction transaction)
        {
            foreach (KeyValuePair<Skill, CharacterSkill> skill in character.Skills)
            {
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterSkillsUpdate, (uint)skill.Value.Status, (ushort)skill.Value.Ranks, skill.Value.ExperienceSpent, character.Id, (uint)skill.Value.Skill);
            }
        }

        public async Task<Character> GetCharacterByName(string name)
        {
            MySqlResult result = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterSelectByName, name);
            Character c = null;

            if (result?.Count > 0)
            {
                uint guid = result.Read<uint>(0, "guid");
                uint accountId = result.Read<uint>(0, "accountId");

                c = new Character(guid, accountId);
                c.Name = result.Read<string>(0, "name");
                c.TemplateOption = result.Read<uint>(0, "templateOption");
                c.StartArea = result.Read<uint>(0, "startArea");
            }

            return c;
        }

        public async Task DeleteFriend(uint characterId, uint friendCharacterId)
        {
            await ExecutePreparedStatementAsync(CharacterPreparedStatement.CharacterFriendDelete, characterId, friendCharacterId);
        }

        public async Task AddFriend(uint characterId, uint friendCharacterId)
        {
            await ExecutePreparedStatementAsync(CharacterPreparedStatement.CharacterFriendInsert, characterId, friendCharacterId);
        }

        public async Task RemoveAllFriends(uint characterId)
        {
            await ExecutePreparedStatementAsync(CharacterPreparedStatement.CharacterFriendsRemoveAll, characterId);
        }

        public uint SetCharacterAccessLevelByName(string characterName, AccessLevel accessLevel)
        {
            Task<MySqlResult> result = SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterSelectByName, characterName);
            Debug.Assert(result != null, "Invalid prepared statement value.");

            uint boolId;
            switch (accessLevel)
            {
                case AccessLevel.Advocate:
                    boolId = (uint)PropertyBool.IsAdvocate;
                    break;
                case AccessLevel.Sentinel:
                    boolId = (uint)PropertyBool.IsSentinel;
                    break;
                case AccessLevel.Envoy:
                    boolId = (uint)PropertyBool.IsPsr;
                    break;
                case AccessLevel.Developer:
                    boolId = (uint)PropertyBool.IsArch;
                    break;
                case AccessLevel.Admin:
                    boolId = (uint)PropertyBool.IsAdmin;
                    break;
                default:
                    boolId = 0;
                    break;
            }

            try
            {
                uint lowGuid = result.Result.Read<uint>(0, "guid");

                if (boolId > 0)
                {
                    ExecutePreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolInsert, lowGuid, (uint)PropertyBool.IsAdvocate, 0);
                    ExecutePreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolInsert, lowGuid, (uint)PropertyBool.IsSentinel, 0);
                    ExecutePreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolInsert, lowGuid, (uint)PropertyBool.IsPsr, 0);
                    ExecutePreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolInsert, lowGuid, (uint)PropertyBool.IsArch, 0);
                    ExecutePreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolInsert, lowGuid, (uint)PropertyBool.IsAdmin, 0);

                    ExecutePreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolInsert, lowGuid, boolId, 1);
                }
                else
                {
                    ExecutePreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolInsert, lowGuid, (uint)PropertyBool.IsAdvocate, 0);
                    ExecutePreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolInsert, lowGuid, (uint)PropertyBool.IsSentinel, 0);
                    ExecutePreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolInsert, lowGuid, (uint)PropertyBool.IsPsr, 0);
                    ExecutePreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolInsert, lowGuid, (uint)PropertyBool.IsArch, 0);
                    ExecutePreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolInsert, lowGuid, (uint)PropertyBool.IsAdmin, 0);
                }

                return lowGuid;
            }
            catch (IndexOutOfRangeException)
            {
                return 0;
            }
        }

        public uint RenameCharacter(string oldName, string newName)
        {
            Task<MySqlResult> result = SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterSelectByName, newName);
            Debug.Assert(result != null, "Invalid prepared statement value.");

            if (IsNameAvailable(newName))
            {
                result = SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterSelectByName, oldName);
                Debug.Assert(result != null, "Invalid prepared statement value.");

                try
                {
                    uint lowGuid = result.Result.Read<uint>(0, "guid");

                    ExecutePreparedStatement(CharacterPreparedStatement.CharacterRename, newName, lowGuid);

                    return lowGuid;
                }
                catch (IndexOutOfRangeException)
                {
                    return 0;
                }
            }
            else
                return 0;
        }
    }
}
