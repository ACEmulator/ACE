using ACE.Entity;
using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database
{
    public interface ISerializedShardDatabase
    {
        void GetObjectsByLandblock(ushort landblock, Action<List<AceObject>> callback);

        void GetObject(uint aceObjectId, Action<AceObject> callback);

        void SaveObject(AceObject aceObject, Action<bool> callback);

        void GetCharacters(uint accountId, Action<List<CachedCharacter>> callback);

        /// <summary>
        /// Loads an object by name.  Primary use case: characters.
        /// </summary>
        void GetObjectInfoByName(string name, Action<ObjectInfo> callback);

        void DeleteFriend(uint characterId, uint friendCharacterId);

        void AddFriend(uint characterId, uint friendCharacterId);

        void RemoveAllFriends(uint characterId);

        void IsCharacterNameAvailable(string name, Action<bool> callback);

        void DeleteOrRestore(ulong unixTime, uint id, Action<bool> callback);

        void SetCharacterAccessLevelByName(string name, AccessLevel accessLevel, Action<uint> callback);

        void RenameCharacter(string currentName, string newName, Action<uint> callback);

        void GetNextCharacterId(Action<uint> callback);

        void GetCharacter(uint id, Action<AceCharacter> callback);
    }
}
