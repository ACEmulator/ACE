using MySql.Data.MySqlClient;
using System;

namespace ACE.Database
{
    public enum CharacterPreparedStatement
    {
        CharacterListSelect
    }

    public class CharacterDatabase : Database
    {
        protected override Type GetPreparedStatementType() { return typeof(CharacterPreparedStatement); }

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(CharacterPreparedStatement.CharacterListSelect, "SELECT `guid`, `name`, `deleteTime` FROM `character` WHERE `id` = ?;", MySqlDbType.UInt32);
        }
    }
}
