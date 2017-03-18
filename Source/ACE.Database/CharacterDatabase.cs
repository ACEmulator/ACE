﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

using MySql.Data.MySqlClient;

namespace ACE.Database
{
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
            CharacterPositionSelect,
            CharacterUniqueNameSelect,
            CharacterSkillsSelect,
            CharacterStatsSelect,
            CharacterAppearanceSelect,
            CharacterFriendsSelect,
            CharacterFriendInsert,
            CharacterFriendDelete,
            CharacterFriendsRemoveAll,
            CharacterOptionsUpdate,

            CharacterPositionInsert,

            CharacterPropertiesBoolSelect,
            CharacterPropertiesIntSelect,
            CharacterPropertiesBigIntSelect,
            CharacterPropertiesDoubleSelect,
            CharacterPropertiesStringSelect,

            CharacterPropertiesBoolInsert,
            CharacterPropertiesIntInsert,
            CharacterPropertiesBigIntInsert,
            CharacterPropertiesDoubleInsert,
            CharacterPropertiesStringInsert
        }

        protected override Type preparedStatementType => typeof(CharacterPreparedStatement);

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(CharacterPreparedStatement.CharacterMaxIndex, "SELECT MAX(`guid`) FROM `character`;");
            AddPreparedStatement(CharacterPreparedStatement.CharacterUniqueNameSelect, "SELECT COUNT(`name`) as cnt FROM `character` WHERE NOT `deleted` = 1 AND `deleteTime` = 0 AND BINARY `name` = ?;", MySqlDbType.VarString);
            AddPreparedStatement(CharacterPreparedStatement.CharacterInsert, "INSERT INTO `character` (`guid`, `accountId`, `name`, `templateOption`, `startArea`) VALUES (?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.VarString, MySqlDbType.UByte, MySqlDbType.UByte);
            AddPreparedStatement(CharacterPreparedStatement.CharacterAppearanceInsert, "INSERT INTO `character_appearance` (`id`, `eyes`, `nose`, `mouth`, `eyeColor`, `hairColor`, `hairStyle`, `hairHue`, `skinHue`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.Double);
            AddPreparedStatement(CharacterPreparedStatement.CharacterStatsInsert, "INSERT INTO `character_stats` (`id`, `strength`, `endurance`, `coordination`, `quickness`, `focus`, `self`, `healthCurrent`, `staminaCurrent`, `manaCurrent`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte);
            AddPreparedStatement(CharacterPreparedStatement.CharacterSkillsInsert, "INSERT INTO `character_skills` (`id`, `skillId`, `skillStatus`, `skillPoints`) VALUES (?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UInt16);
            AddPreparedStatement(CharacterPreparedStatement.CharacterStartupGearInsert, "INSERT INTO `character_startup_gear` (`id`, `headgearStyle`, `headgearColor`, `headgearHue`, `shirtStyle`, `shirtColor`, `shirtHue`, `pantsStyle`, `pantsColor`, `pantsHue`, `footwearStyle`, `footwearColor`, `footwearHue`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double);
            AddPreparedStatement(CharacterPreparedStatement.CharacterDeleteOrRestore, "UPDATE `character` SET `deleteTime` = ?, `deleted` = 0 WHERE `guid` = ?;", MySqlDbType.UInt64, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterListSelect, "SELECT `guid`, `name`, `deleteTime` FROM `character` WHERE `accountId` = ? AND `deleted` = 0 ORDER BY `name` ASC;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterFriendInsert, "INSERT INTO `character_friends` (`id`, `friendId`) VALUES (?, ?);", MySqlDbType.UInt32, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterFriendDelete, "DELETE FROM  `character_friends` WHERE `id` = ? AND `friendId` = ?;", MySqlDbType.UInt32, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterFriendsRemoveAll, "DELETE FROM  `character_friends` WHERE `id` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterSelectByName, "SELECT `guid`, `accountId`, `name`, `templateOption`, `startArea` FROM `character` WHERE `deleted` = 0 AND `deleteTime` = 0 AND `name` = ?;", MySqlDbType.VarString);
            AddPreparedStatement(CharacterPreparedStatement.CharacterOptionsUpdate, "UPDATE `character` SET `characterOptions1` = ?, `characterOptions2` = ? WHERE guid = ?", MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterRename, "UPDATE `character` SET `name` = ? WHERE `guid` = ?;", MySqlDbType.VarString, MySqlDbType.UInt32);

            // world entry
            AddPreparedStatement(CharacterPreparedStatement.CharacterSelect, "SELECT `guid`, `accountId`, `name`, `templateOption`, `startArea`, `characterOptions1`, `characterOptions2` FROM `character` WHERE `guid` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterPositionSelect, "SELECT `cell`, `positionX`, `positionY`, `positionZ`, `rotationX`, `rotationY`, `rotationZ`, `rotationW` FROM `character_position` WHERE `id` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterSkillsSelect, "SELECT `skillId`, `skillStatus`, `skillPoints` FROM `character_skills` WHERE `id` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterStatsSelect, "SELECT `strength`, `strengthRanks`, `endurance`, `enduranceRanks`, `coordination`, `coordinationRanks`, `quickness`, `quicknessRanks`, `focus`, `focusRanks`, `self`, `selfRanks`, `healthRanks`, `healthCurrent`, `staminaRanks`, `staminaCurrent`, `manaRanks`, `manaCurrent` FROM `character_stats` WHERE `id` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterAppearanceSelect, "SELECT  `eyes`, `nose`, `mouth`, `eyeColor`, `hairColor`, `hairStyle`, `hairHue`, `skinHue` FROM `character_appearance` WHERE `id` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterFriendsSelect, "SELECT cf.`friendId`, c.`name` FROM `character_friends` cf JOIN `character` c ON (cf.`friendId` = c.`guid`) WHERE cf.`id` = ?;", MySqlDbType.UInt32);

            AddPreparedStatement(CharacterPreparedStatement.CharacterPositionInsert, "REPLACE INTO character_position (id, cell, positionX, positionY, positionZ, rotationX, rotationY, rotationZ, rotationW) VALUES (?, ?, ?, ?, ?, ?, ? ,? ,?);", MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.Float, MySqlDbType.Float, MySqlDbType.Float, MySqlDbType.Float, MySqlDbType.Float, MySqlDbType.Float, MySqlDbType.Float);

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
        }

        public uint GetMaxId()
        {
            var result = SelectPreparedStatement(CharacterPreparedStatement.CharacterMaxIndex);
            Debug.Assert(result != null);
            return result.Read<uint>(0, "MAX(`guid`)") + 1;
        }

        public async Task<Position> GetPosition(uint id)
        {
            MySqlResult result = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterPositionSelect, id);
            Position pos;
            if (result.Count > 0)
            {
                pos = new Position(result.Read<uint>(0, "cell"), result.Read<float>(0, "positionX"), result.Read<float>(0, "positionY"), result.Read<float>(0, "positionZ"),
                    result.Read<float>(0, "rotationX"), result.Read<float>(0, "rotationY"), result.Read<float>(0, "rotationZ"), result.Read<float>(0, "rotationW"));
            }
            else
            {
                // use fallback position if position information doesn't exist in the DB, show error in the future
                pos = new Position(0x7F0401AD, 12.3199f, -28.482f, 0.0049999995f, 0.0f, 0.0f, -0.9408059f, -0.3389459f);
            }

            return pos;
        }

        public async Task<List<CachedCharacter>> GetByAccount(uint accountId)
        {
            var result = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterListSelect, accountId);
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
            var result = SelectPreparedStatement(CharacterPreparedStatement.CharacterUniqueNameSelect, name);
            Debug.Assert(result != null);

            uint charsWithName = result.Read<uint>(0, "cnt");
            return (charsWithName == 0);
        }

        public Task UpdateCharacter(Character character)
        {
            ExecutePreparedStatement(CharacterPreparedStatement.CharacterPositionInsert, character.Id, character.Position.LandblockId.Raw, character.Position.Offset.X, character.Position.Offset.Y, character.Position.Offset.Z, character.Position.Facing.X, character.Position.Facing.Y, character.Position.Facing.Z, character.Position.Facing.W);

            // TODO: implement saving a character
            return Task.Delay(0);
        }

        public async Task<bool> CreateCharacter(Character character)
        {
            var transaction = BeginTransaction();
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
                character.Strength.Base,
                character.Endurance.Base,
                character.Coordination.Base,
                character.Quickness.Base,
                character.Focus.Base,
                character.Self.Base,
                character.Health.Current,
                character.Stamina.Current,
                character.Mana.Current);

            foreach (var skill in character.Skills.Values)
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

                c.Position = await this.GetPosition(guid);

                uint characterOptions1Flag = result.Read<uint>(0, "characterOptions1");
                uint characterOptions2Flag = result.Read<uint>(0, "characterOptions2");
                LoadCharacterOptions(characterOptions1Flag, characterOptions2Flag, c);

                result = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterSkillsSelect, id);

                for (uint i = 0; i < result?.Count; i++)
                {
                    Skill s = result.Read<Skill>(i, "skillId");
                    SkillStatus ss = result.Read<SkillStatus>(i, "skillStatus");
                    uint ranks = result.Read<uint>(i, "skillPoints");
                    c.Skills.Add(s, new CharacterSkill(c, s, ss, ranks));
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
                    c.Appearance.HairHue = result.Read<uint>(0, "hairHue");
                    c.Appearance.SkinHue = result.Read<uint>(0, "skinHue");
                }

                result = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterStatsSelect, id);

                if (result?.Count > 0)
                {
                    c.Strength.Base = result.Read<uint>(0, "strength");
                    c.Strength.Ranks = result.Read<uint>(0, "strengthRanks");
                    c.Endurance.Base = result.Read<uint>(0, "endurance");
                    c.Endurance.Ranks = result.Read<uint>(0, "enduranceRanks");
                    c.Coordination.Base = result.Read<uint>(0, "coordination");
                    c.Coordination.Ranks = result.Read<uint>(0, "coordinationRanks");
                    c.Quickness.Base = result.Read<uint>(0, "quickness");
                    c.Quickness.Ranks = result.Read<uint>(0, "quicknessRanks");
                    c.Focus.Base = result.Read<uint>(0, "focus");
                    c.Focus.Ranks = result.Read<uint>(0, "focusRanks");
                    c.Self.Base = result.Read<uint>(0, "self");
                    c.Self.Ranks = result.Read<uint>(0, "selfRanks");
                    
                    c.Health.Ranks = result.Read<uint>(0, "healthRanks");
                    c.Health.Current = result.Read<uint>(0, "healthCurrent");
                    c.Stamina.Ranks = result.Read<uint>(0, "staminaRanks");
                    c.Stamina.Current = result.Read<uint>(0, "staminaCurrent");
                    c.Mana.Ranks = result.Read<uint>(0, "manaRanks");
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
            foreach (var option in character.CharacterOptions)
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

            foreach(var option in optionsToSetToTrue)
            {
                character.SetCharacterOption(option, true);
            }
        }

        public void SaveCharacterOptions(Character character)
        {
            ExecutePreparedStatement(CharacterPreparedStatement.CharacterOptionsUpdate, character.CharacterOptions.GetCharacterOptions1Flag(), character.CharacterOptions.GetCharacterOptions2Flag(), character.Id);
        }

        public async Task LoadCharacterProperties(DbObject dbObject)
        {
            var results = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterPropertiesBoolSelect, dbObject.Id);
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

        public void SaveCharacterProperties(DbObject dbObject, DatabaseTransaction transaction)
        {
            // known issue: properties that were removed from the bucket will not updated.  this is a problem if we
            // ever need to straight up "delete" a property.

            foreach (var prop in dbObject.PropertiesBool)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesBoolInsert, dbObject.Id, (ushort)prop.Key, prop.Value);

            foreach (var prop in dbObject.PropertiesInt)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesIntInsert, dbObject.Id, (ushort)prop.Key, prop.Value);

            foreach (var prop in dbObject.PropertiesInt64)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesBigIntInsert, dbObject.Id, (ushort)prop.Key, prop.Value);

            foreach (var prop in dbObject.PropertiesDouble)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesDoubleInsert, dbObject.Id, (ushort)prop.Key, prop.Value);

            foreach (var prop in dbObject.PropertiesString)
                transaction.AddPreparedStatement(CharacterPreparedStatement.CharacterPropertiesStringInsert, dbObject.Id, (ushort)prop.Key, prop.Value);
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
            var result = SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterSelectByName, characterName);
            Debug.Assert(result != null);

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
            var result = SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterSelectByName, newName);
            Debug.Assert(result != null);

            if (IsNameAvailable(newName))
            {
                result = SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterSelectByName, oldName);
                Debug.Assert(result != null);

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
