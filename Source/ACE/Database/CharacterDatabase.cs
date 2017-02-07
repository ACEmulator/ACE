using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ACE.Database
{
    public class CharacterDatabase : Database, ICharacterDatabase
    {
        private enum CharacterPreparedStatement
        {
            CharacterDeleteOrRestore,
            CharacterInsert,
            CharacterSelect,
            CharacterAppearanceInsert,
            CharacterStatsInsert,
            CharacterSkillsInsert,
            CharacterStartupGearInsert,
            CharacterListSelect,
            CharacterMaxIndex,
            CharacterPositionSelect,
            CharacterUniqueNameSelect,
            CharacterSkillsSelect,
            CharacterStatsSelect
        }

        protected override Type preparedStatementType { get { return typeof(CharacterPreparedStatement); } }

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(CharacterPreparedStatement.CharacterMaxIndex, "SELECT MAX(`guid`) FROM `character`;");
            AddPreparedStatement(CharacterPreparedStatement.CharacterUniqueNameSelect, "SELECT COUNT(`name`) as cnt FROM `character` WHERE BINARY `name` = ?;", MySqlDbType.VarString);
            AddPreparedStatement(CharacterPreparedStatement.CharacterInsert, "INSERT INTO `character` (`guid`, `accountId`, `name`, `templateOption`, `startArea`, `isAdmin`, `isEnvoy`) VALUES (?, ?, ?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.VarString, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte);
            AddPreparedStatement(CharacterPreparedStatement.CharacterAppearanceInsert, "INSERT INTO `character_appearance` (`id`, `race`, `gender`, `eyes`, `nose`, `mouth`, `eyeColor`, `hairColor`, `hairStyle`, `hairHue`, `skinHue`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.Double);
            AddPreparedStatement(CharacterPreparedStatement.CharacterStatsInsert, "INSERT INTO `character_stats` (`id`, `strength`, `endurance`, `coordination`, `quickness`, `focus`, `self`, `health_current`, `stamina_current`, `mana_current`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte);
            AddPreparedStatement(CharacterPreparedStatement.CharacterSkillsInsert, "INSERT INTO `character_skills` (`id`, `skillId`, `skillStatus`, `skillPoints`) VALUES (?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UInt16);
            AddPreparedStatement(CharacterPreparedStatement.CharacterStartupGearInsert, "INSERT INTO `character_startup_gear` (`id`, `headgearStyle`, `headgearColor`, `headgearHue`, `shirtStyle`, `shirtColor`, `shirtHue`, `pantsStyle`, `pantsColor`, `pantsHue`, `footwearStyle`, `footwearColor`, `footwearHue`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double);
            AddPreparedStatement(CharacterPreparedStatement.CharacterDeleteOrRestore, "UPDATE `character` SET `deleteTime` = ? WHERE `guid` = ?;", MySqlDbType.UInt64, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterListSelect, "SELECT `guid`, `name`, `deleteTime` FROM `character` WHERE `accountId` = ? ORDER BY `name` ASC;", MySqlDbType.UInt32);

            // world entry
            AddPreparedStatement(CharacterPreparedStatement.CharacterSelect, "SELECT `guid`, `accountId`, `name`, `templateOption`, `startArea`, `isAdmin`, `isEnvoy` FROM `character` WHERE `guid` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterPositionSelect, "SELECT `cell`, `positionX`, `positionY`, `positionZ`, `rotationX`, `rotationY`, `rotationZ`, `rotationW` FROM `character_position` WHERE `id` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterSkillsSelect, "SELECT `skillId`, `skillStatus`, `skillPoints` FROM `character_skills` WHERE `id` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterStatsSelect, "SELECT `strength`, `strength_ranks`, `endurance`, `endurance_ranks`, `coordination`, `coordination_ranks`, `quickness`, `quickness_ranks`, `focus`, `focus_ranks`, `self`, `self_ranks`, `health_ranks`, `health_current`, `stamina_ranks`, `stamina_current`, `mana_ranks`, `mana_current` FROM `character_stats` WHERE `id` = ?;", MySqlDbType.UInt32);
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
                uint lowGuid = result.Read<uint>(i, "guid");
                string name = result.Read<string>(i, "name");
                ulong deleteTime = result.Read<ulong>(i, "deleteTime");

                characters.Add(new CachedCharacter(lowGuid, i, name, deleteTime));
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

        public async Task CreateCharacter(Character character)
        {
            // first one can't be awaited
            ExecutePreparedStatement(CharacterPreparedStatement.CharacterInsert, 
                character.Id, 
                character.AccountId, 
                character.Name, 
                character.TemplateOption, 
                character.StartArea, 
                character.IsAdmin, 
                character.IsEnvoy);

            await ExecutePreparedStatementAsync(CharacterPreparedStatement.CharacterAppearanceInsert,
                character.Id,
                character.Appearance.Race,
                character.Appearance.Gender,
                character.Appearance.Eyes,
                character.Appearance.Nose,
                character.Appearance.Mouth,
                character.Appearance.EyeColor,
                character.Appearance.HairColor,
                character.Appearance.HairStyle,
                character.Appearance.HairHue,
                character.Appearance.SkinHue);

            await ExecutePreparedStatementAsync(CharacterPreparedStatement.CharacterStatsInsert, 
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

            foreach(var skill in character.Skills.Values)
            {
                await ExecutePreparedStatementAsync(CharacterPreparedStatement.CharacterSkillsInsert, 
                    character.Id, 
                    (uint)skill.Skill, 
                    (uint)skill.Status, 
                    skill.Ranks);
            }

            await ExecutePreparedStatementAsync(CharacterPreparedStatement.CharacterStartupGearInsert, 
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
                c.IsAdmin = result.Read<bool>(0, "isAdmin");
                c.IsEnvoy = result.Read<bool>(0, "isEnvoy");

                c.Position = await this.GetPosition(guid);

                result = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterSkillsSelect, id);

                for(uint i = 0; i < result?.Count; i++)
                {
                    Skill s = result.Read<Skill>(i, "skillId");
                    SkillStatus ss = result.Read<SkillStatus>(i, "skillStatus");
                    uint ranks = result.Read<uint>(i, "skillPoints");
                    c.Skills.Add(s, new CharacterSkill(c, s, ss, ranks));
                }

                result = await SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterStatsSelect, id);

                if (result?.Count > 0)
                {
                    c.Strength.Base = result.Read<uint>(0, "strength");
                    c.Strength.Ranks = result.Read<uint>(0, "strength_ranks");
                    c.Endurance.Base = result.Read<uint>(0, "endurance");
                    c.Endurance.Ranks = result.Read<uint>(0, "endurance_ranks");
                    c.Coordination.Base = result.Read<uint>(0, "coordination");
                    c.Coordination.Ranks = result.Read<uint>(0, "coordination_ranks");
                    c.Quickness.Base = result.Read<uint>(0, "quickness");
                    c.Quickness.Ranks = result.Read<uint>(0, "quickness_ranks");
                    c.Focus.Base = result.Read<uint>(0, "focus");
                    c.Focus.Ranks = result.Read<uint>(0, "focus_ranks");
                    c.Self.Base = result.Read<uint>(0, "self");
                    c.Self.Ranks = result.Read<uint>(0, "self_ranks");
                    
                    c.Health.Ranks = result.Read<uint>(0, "health_ranks");
                    c.Health.Current = result.Read<uint>(0, "health_current");
                    c.Stamina.Ranks = result.Read<uint>(0, "stamina_ranks");
                    c.Stamina.Current = result.Read<uint>(0, "stamina_current");
                    c.Mana.Ranks = result.Read<uint>(0, "mana_ranks");
                    c.Mana.Current = result.Read<uint>(0, "mana_current");
                }
            }

            return c;
        }
    }
}
