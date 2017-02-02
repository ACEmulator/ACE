using MySql.Data.MySqlClient;
using System;

namespace ACE.Database
{
    public enum CharacterPreparedStatement
    {
        CharacterDeleteOrRestore,
        CharacterInsert,
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
            AddPreparedStatement(CharacterPreparedStatement.CharacterDeleteOrRestore, "UPDATE `character` SET `deleteTime` = ? WHERE `guid` = ?;", MySqlDbType.UInt64, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterListSelect, "SELECT `guid`, `name`, `deleteTime` FROM `character` WHERE `accountId` = ? ORDER BY `name` ASC;", MySqlDbType.UInt32);

            // world entry
            AddPreparedStatement(CharacterPreparedStatement.CharacterPositionSelect, "SELECT `cell`, `positionX`, `positionY`, `positionZ`, `rotationX`, `rotationY`, `rotationZ`, `rotationW` FROM `character_position` WHERE `id` = ?;", MySqlDbType.UInt32);
        }
    }
}
