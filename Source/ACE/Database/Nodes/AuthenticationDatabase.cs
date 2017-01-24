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
            AddPreparedStatement(AuthenticationPreparedStatement.AccountInsert, "INSERT INTO `user_accounts` (`UserName`, `Password`, `salt`) VALUES (?, ?, ?);", MySqlDbType.VarString, MySqlDbType.VarString, MySqlDbType.VarString);
            AddPreparedStatement(AuthenticationPreparedStatement.AccountMaxIndex, "SELECT MAX(`id`) FROM `user_accounts`;");
            AddPreparedStatement(AuthenticationPreparedStatement.AccountSelect, "SELECT `id`, `UserName`, `password`, `salt` FROM `user_accounts` WHERE `UserName` = ?;", MySqlDbType.VarString);
        }
    }
}
