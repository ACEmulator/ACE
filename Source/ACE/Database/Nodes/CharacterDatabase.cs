using MySql.Data.MySqlClient;
using System;

namespace ACE.Database
{
    public enum CharacterPreparedStatement
    {
        CharacterDeleteOrRestore,
        CharacterInsert,
        CharacterAppearanceInsert,
        CharacterStatsInsert,
        CharacterSkillsInsert,
        CharacterStartupGearInsert,
        CharacterListSelect,
        CharacterMaxIndex,
        CharacterPositionSelect,
        CharacterUniqueNameSelect
    }

    public class CharacterDatabase : Database
    {
        protected override Type preparedStatementType { get { return typeof(CharacterPreparedStatement); } }

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(CharacterPreparedStatement.CharacterMaxIndex, "SELECT MAX(`guid`) FROM `character`;");
            AddPreparedStatement(CharacterPreparedStatement.CharacterUniqueNameSelect, "SELECT COUNT(`name`) as cnt FROM `character` WHERE BINARY `name` = ?;", MySqlDbType.VarString);
            AddPreparedStatement(CharacterPreparedStatement.CharacterInsert, "INSERT INTO `character` (`guid`, `accountId`, `name`, `templateOption`, `startArea`, `isAdmin`, `isEnvoy`) VALUES (?, ?, ?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.VarString, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte);
            AddPreparedStatement(CharacterPreparedStatement.CharacterAppearanceInsert, "INSERT INTO `character_appearance` (`id`, `race`, `gender`, `eyes`, `nose`, `mouth`, `eyeColor`, `hairColor`, `hairStyle`, `hairHue`, `skinHue`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.Double);
            AddPreparedStatement(CharacterPreparedStatement.CharacterStatsInsert, "INSERT INTO `character_stats` (`id`, `strength`, `endurance`, `coordination`, `quickness`, `focus`, `self`) VALUES (?, ?, ?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UByte);
            AddPreparedStatement(CharacterPreparedStatement.CharacterSkillsInsert, "INSERT INTO `character_skills` (`id`, `skillId`, `skillStatus`, `skillPoints`) VALUES (?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.UInt16);
            AddPreparedStatement(CharacterPreparedStatement.CharacterStartupGearInsert, "INSERT INTO `character_startup_gear` (`id`, `headgearStyle`, `headgearColor`, `headgearHue`, `shirtStyle`, `shirtColor`, `shirtHue`, `pantsStyle`, `pantsColor`, `pantsHue`, `footwearStyle`, `footwearColor`, `footwearHue`) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.UInt32, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double, MySqlDbType.UByte, MySqlDbType.UByte, MySqlDbType.Double);
            AddPreparedStatement(CharacterPreparedStatement.CharacterDeleteOrRestore, "UPDATE `character` SET `deleteTime` = ? WHERE `guid` = ?;", MySqlDbType.UInt64, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterListSelect, "SELECT `guid`, `name`, `deleteTime` FROM `character` WHERE `accountId` = ? ORDER BY `name` ASC;", MySqlDbType.UInt32);

            // world entry
            AddPreparedStatement(CharacterPreparedStatement.CharacterPositionSelect, "SELECT `cell`, `positionX`, `positionY`, `positionZ`, `rotationX`, `rotationY`, `rotationZ`, `rotationW` FROM `character_position` WHERE `id` = ?;", MySqlDbType.UInt32);
        }
    }
}
