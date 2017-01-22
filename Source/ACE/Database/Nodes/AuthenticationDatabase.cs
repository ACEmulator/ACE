using MySql.Data.MySqlClient;
using System;

namespace ACE.Database
{
    public enum AuthenticationPreparedStatement
    {
        AccountInsert,
        AccountMaxIndex,
        AccountSelect
    }

    public class AuthenticationDatabase : Database
    {
        protected override Type GetPreparedStatementType() { return typeof(AuthenticationPreparedStatement); }

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(AuthenticationPreparedStatement.AccountInsert, "INSERT INTO `account` (`id`, `account`, `password`, `salt`) VALUES (?, ?, ?, ?);", MySqlDbType.UInt32, MySqlDbType.VarString, MySqlDbType.VarString, MySqlDbType.VarString);
            AddPreparedStatement(AuthenticationPreparedStatement.AccountMaxIndex, "SELECT MAX(`id`) FROM `account`;");
            AddPreparedStatement(AuthenticationPreparedStatement.AccountSelect, "SELECT `id`, `account`, `password`, `salt` FROM `account` WHERE `account` = ?;", MySqlDbType.VarString);
        }
    }
}
