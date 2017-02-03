using ACE.Entity;
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
            CharacterListSelect,
            CharacterMaxIndex,
            CharacterPositionSelect,
            CharacterUniqueNameSelect
        }

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

        public void CreateCharacter(uint id, uint accountId, string name, uint templateOption, uint startArea, bool isAdmin, bool isEnvoy)
        {
            ExecutePreparedStatement(CharacterPreparedStatement.CharacterInsert, id, accountId, name, templateOption, startArea, isAdmin, isEnvoy);
        }
    }
}
