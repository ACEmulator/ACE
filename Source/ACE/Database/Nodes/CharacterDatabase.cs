using MySql.Data.MySqlClient;
using System;

namespace ACE.Database
{
    public enum CharacterPreparedStatement
    {
        CharacterDeleteOrRestore,
        CharacterListSelect
    }

    public class CharacterDatabase : Database
    {
        protected override Type GetPreparedStatementType() { return typeof(CharacterPreparedStatement); }

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(CharacterPreparedStatement.CharacterDeleteOrRestore, "UPDATE `character` SET `deleteTime` = ? WHERE `guid` = ?;", MySqlDbType.UInt64, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterListSelect, "SELECT `guid`, `name`, `deleteTime` FROM `character` WHERE `id` = ? ORDER BY `name` ASC;", MySqlDbType.UInt32);
        }
    }
}
