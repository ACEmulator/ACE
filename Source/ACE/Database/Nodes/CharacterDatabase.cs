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
            AddPreparedStatement(CharacterPreparedStatement.CharacterDeleteOrRestore, "UPDATE `characters` SET `DeleteTime` = ? WHERE `Character_ID` = ?;", MySqlDbType.UInt64, MySqlDbType.UInt32);
            AddPreparedStatement(CharacterPreparedStatement.CharacterListSelect, "SELECT `Character_ID`, `Name`, `DeleteTime` FROM `characters` WHERE `Character_ID` = ? ORDER BY `name` ASC;", MySqlDbType.UInt32);
        }
    }
}
