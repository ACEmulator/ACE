using MySql.Data.MySqlClient;
using System;

namespace ACE.Database
{
    public enum CharacterPreparedStatement
    {
    }

    public class CharacterDatabase : Database
    {
        protected override Type GetPreparedStatementType() { return typeof(CharacterPreparedStatement); }

        protected override void InitialisePreparedStatements()
        {
        }
    }
}
