using System;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using log4net;
// ReSharper disable InconsistentNaming

namespace ACE.Database
{
    public class ShardDatabase : Database, IShardDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static BlockingCollection<Tuple<Task<bool>, uint, long>> saveObjects = new BlockingCollection<Tuple<Task<bool>, uint, long>>();
        private static Thread taskCleanThread;

        private static long cacheVersion = 0;
        // NOTE: long in tuple (and cacheVersion) are there so if an item is queued for save multiple times, we don't remove it from the cache prematurely
        private static Dictionary<uint, Tuple<AceObject, long>> saveCache = new Dictionary<uint, Tuple<AceObject, long>>();
        private static object cacheLock = new object();

        private enum ShardPreparedStatement
        {
            // these are for the world database, but there's a lot of overlap
            // so we're just going to reserve 0-100 for them here
            TeleportLocationSelect = 0,
            GetWeenieClass = 1,
            GetObjectsByLandblock = 2,
            GetCreaturesByLandblock = 3,
            GetPaletteOverridesByObject = 7,
            GetAnimationOverridesByObject = 8,
            GetTextureOverridesByObject = 9,
            GetCreatureDataByWeenie = 10,
            InsertCreatureStaticLocation = 11,
            GetCreatureGeneratorByLandblock = 12,
            GetCreatureGeneratorData = 13,
            GetPortalObjectsByAceObjectId = 14,
            GetItemsByTypeId = 15,
            GetAceObjectPropertiesInt = 16,
            GetAceObjectPropertiesBigInt = 17,
            GetAceObjectPropertiesDouble = 18,
            GetAceObjectPropertiesBool = 19,
            GetAceObjectPropertiesString = 20,
            GetAceObjectPropertiesDid = 21,
            GetAceObjectPropertiesIid = 22,
            GetAceObjectPropertiesPositions = 23,
            GetAceObjectPropertiesAttributes = 24,
            GetAceObjectPropertiesAttributes2nd = 25,
            GetAceObjectPropertiesSkills = 26,

            SaveAceObject = 27,
            DeleteAceObject = 28,
            GetAceObject = 29,
            UpdateAceObject = 30,

            AddFriend = 101,
            DeleteFriend = 102,
            GetFriends = 103,
            DeleteAllFriends = 104,

            GetCharacters = 105,
            GetNextCharacterId = 106,
            IsCharacterNameAvailable = 107,
            // keep on going, you get the idea

            DeletePaletteOverridesByObject = 108,
            DeleteAnimationOverridesByObject = 109,
            DeleteTextureOverridesByObject = 110,
            DeleteAceObjectPropertiesInt = 111,
            DeleteAceObjectPropertiesBigInt = 112,
            DeleteAceObjectPropertiesDouble = 113,
            DeleteAceObjectPropertiesBool = 114,
            DeleteAceObjectPropertiesString = 115,
            DeleteAceObjectPropertiesDid = 116,
            DeleteAceObjectPropertiesIid = 117,
            DeleteAceObjectPropertiesPositions = 118,
            DeleteAceObjectPropertiesAttributes = 119,
            DeleteAceObjectPropertiesAttributes2nd = 120,
            DeleteAceObjectPropertiesSkills = 121,
            InsertPaletteOverridesByObject = 122,
            InsertAnimationOverridesByObject = 123,
            InsertTextureOverridesByObject = 124,
            InsertAceObjectPropertiesInt = 125,
            InsertAceObjectPropertiesBigInt = 126,
            InsertAceObjectPropertiesDouble = 127,
            InsertAceObjectPropertiesBool = 128,
            InsertAceObjectPropertiesString = 129,
            InsertAceObjectPropertiesDid = 130,
            InsertAceObjectPropertiesIid = 131,
            InsertAceObjectPropertiesPositions = 132,
            InsertAceObjectPropertiesAttributes = 133,
            InsertAceObjectPropertiesAttributes2nd = 134,
            InsertAceObjectPropertiesSkills = 135
        }

        protected override Type PreparedStatementType
        {
            get
            {
                return typeof(ShardPreparedStatement);
            }
        }

        public override void Initialize(string host, uint port, string user, string password, string database)
        {
            base.Initialize(host, port, user, password, database);

            // Set-up our save-thread here
            taskCleanThread = new Thread(SaveBackgroundTask);
            taskCleanThread.Start();
        }

        /// <summary>
        /// Shuts Down Safely
        /// </summary>
        public static void ShutDown()
        {
            saveObjects.CompleteAdding();

            // Wait for all the tasks to finish
            taskCleanThread.Join();
        }

        protected override void InitializePreparedStatements()
        {
            ConstructStatement(ShardPreparedStatement.GetNextCharacterId, typeof(CachedCharacter), ConstructedStatementType.GetAggregate);
            ConstructStatement(ShardPreparedStatement.IsCharacterNameAvailable, typeof(CachedCharacter), ConstructedStatementType.Get);

            ConstructStatement(ShardPreparedStatement.DeleteAceObject, typeof(AceObject), ConstructedStatementType.Delete);
            ConstructStatement(ShardPreparedStatement.SaveAceObject, typeof(AceObject), ConstructedStatementType.Insert);
            ConstructStatement(ShardPreparedStatement.GetAceObject, typeof(AceObject), ConstructedStatementType.Get);

            ConstructStatement(ShardPreparedStatement.GetCharacters, typeof(CachedCharacter), ConstructedStatementType.GetList);

            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesString, typeof(AceObjectPropertiesString), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.GetList);

            ConstructStatement(ShardPreparedStatement.GetTextureOverridesByObject, typeof(TextureMapOverride), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetPaletteOverridesByObject, typeof(PaletteOverride), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAnimationOverridesByObject, typeof(AnimationOverride), ConstructedStatementType.GetList);

            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesPositions, typeof(AceObjectPropertiesPosition), ConstructedStatementType.GetList);

            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesAttributes, typeof(AceObjectPropertiesAttribute), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesAttributes2nd, typeof(AceObjectPropertiesAttribute2nd), ConstructedStatementType.GetList);
            ConstructStatement(ShardPreparedStatement.GetAceObjectPropertiesSkills, typeof(AceObjectPropertiesSkill), ConstructedStatementType.GetList);

            // Delete statements
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesString, typeof(AceObjectPropertiesString), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesPositions, typeof(AceObjectPropertiesPosition), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesSkills, typeof(AceObjectPropertiesSkill), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesAttributes, typeof(AceObjectPropertiesAttribute), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAceObjectPropertiesAttributes2nd, typeof(AceObjectPropertiesAttribute2nd), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteTextureOverridesByObject, typeof(TextureMapOverride), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeletePaletteOverridesByObject, typeof(PaletteOverride), ConstructedStatementType.DeleteList);
            ConstructStatement(ShardPreparedStatement.DeleteAnimationOverridesByObject, typeof(AnimationOverride), ConstructedStatementType.DeleteList);

            // Insert statements
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesString, typeof(AceObjectPropertiesString), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesPositions, typeof(AceObjectPropertiesPosition), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesSkills, typeof(AceObjectPropertiesSkill), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesAttributes, typeof(AceObjectPropertiesAttribute), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAceObjectPropertiesAttributes2nd, typeof(AceObjectPropertiesAttribute2nd), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertTextureOverridesByObject, typeof(TextureMapOverride), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertPaletteOverridesByObject, typeof(PaletteOverride), ConstructedStatementType.InsertList);
            ConstructStatement(ShardPreparedStatement.InsertAnimationOverridesByObject, typeof(AnimationOverride), ConstructedStatementType.InsertList);
        }

        public Task AddFriend(uint characterId, uint friendCharacterId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFriend(uint characterId, uint friendCharacterId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAllFriends(uint characterId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteOrRestore(ulong unixTime, uint aceObjectId)
        {
            AceCharacter aceCharacter = new AceCharacter(aceObjectId);
            LoadIntoObject(aceCharacter);
            aceCharacter.DeleteTime = unixTime;

            aceCharacter.Deleted = false;  // This is a reminder - the DB will set this 1 hour after deletion.

            return SaveObject(aceCharacter);
        }

        public async Task<List<CachedCharacter>> GetCharacters(uint accountId)
        {
            var criteria = new Dictionary<string, object> { { "accountId", accountId }, { "deleted", 0 } };
            var objects = await Task.Run(() => ExecuteConstructedGetListStatement<ShardPreparedStatement, CachedCharacter>(ShardPreparedStatement.GetCharacters, criteria));

            return objects;
        }

        // FIXME(ddevec): This is racy.  If we have 2 sessions create a character at the same time this may return the same key 2x, crashing the server
        // One solution would be to read at init into a local variable, then protect accesses to that variable with a lock
        // I'm not that familiar with DB's as a whole, so I'll leave this for a DB person to fix.
        public uint GetNextCharacterId()
        {
            uint maxId = ExecuteConstructedGetAggregateStatement<ShardPreparedStatement, CachedCharacter, uint>(ShardPreparedStatement.GetNextCharacterId);

            ObjectGuid nextGuid = new ObjectGuid(maxId + 1, GuidType.Player);
            return nextGuid.Full;
        }

        /// <summary>
        /// FIXME(ddevec): Should not be called directly -- isntead use DbManager()...
        /// </summary>
        /// <param name="aceObjectId"></param>
        /// <returns></returns>
        public AceCharacter GetCharacter(uint aceObjectId)
        {
            AceCharacter ret = null;

            // check cache
            lock (cacheLock)
            {
                if (saveCache.ContainsKey(aceObjectId))
                {
                    ret = saveCache[aceObjectId].Item1 as AceCharacter;
                    if (ret == null)
                    {
                        log.Error($"Guid: {aceObjectId} attempting to be loaded as character, but saved as object");
                    }
                }
            }

            if (ret == null)
            {
                ret = new AceCharacter(aceObjectId);
                // load common stuff here
                LoadIntoCharacter(ret);
            }

            return ret;
        }

        /// <summary>
        /// FIXME(ddevec): Should not be called directly -- instead use DbManager()...
        /// </summary>
        /// <param name="aceObjectId"></param>
        /// <returns></returns>
        public AceObject GetObject(uint aceObjectId)
        {
            AceObject ret = null;
            lock (cacheLock)
            {
                if (saveCache.ContainsKey(aceObjectId))
                {
                    ret = saveCache[aceObjectId].Item1 as AceObject;
                }
            }

            if (ret == null)
            {
                ret = new AceObject(aceObjectId);
                LoadIntoObject(ret);
            }

            return ret;
        }

        /// <summary>
        /// Does the back-end (non-cached) loading
        /// </summary>
        /// <param name="aceCharacter"></param>
        private void LoadIntoCharacter(AceCharacter aceCharacter)
        {
            // Right now this just forwards to loadobject -- hopefully we keep it that way for simplicity...
            LoadIntoObject(aceCharacter);
        }

        /// <summary>
        /// Does the backend (non-cached) loading
        /// </summary>
        /// <param name="aceObject"></param>
        private void LoadIntoObject(AceObject aceObject)
        {
            // TODO: still to implement - load spells, friends, allegiance info, spell comps, spell bars
            aceObject.IntProperties = LoadAceTable<AceObjectPropertiesInt>(ShardPreparedStatement.GetAceObjectPropertiesInt, aceObject.AceObjectId);
            aceObject.Int64Properties = LoadAceTable<AceObjectPropertiesInt64>(ShardPreparedStatement.GetAceObjectPropertiesBigInt, aceObject.AceObjectId);
            aceObject.BoolProperties = LoadAceTable<AceObjectPropertiesBool>(ShardPreparedStatement.GetAceObjectPropertiesBool, aceObject.AceObjectId);
            aceObject.DoubleProperties = LoadAceTable<AceObjectPropertiesDouble>(ShardPreparedStatement.GetAceObjectPropertiesDouble, aceObject.AceObjectId);
            aceObject.StringProperties = LoadAceTable<AceObjectPropertiesString>(ShardPreparedStatement.GetAceObjectPropertiesString, aceObject.AceObjectId);
            aceObject.InstanceIdProperties = LoadAceTable<AceObjectPropertiesInstanceId>(ShardPreparedStatement.GetAceObjectPropertiesIid, aceObject.AceObjectId);
            aceObject.DataIdProperties = LoadAceTable<AceObjectPropertiesDataId>(ShardPreparedStatement.GetAceObjectPropertiesDid, aceObject.AceObjectId);
            aceObject.TextureOverrides = LoadAceTable<TextureMapOverride>(ShardPreparedStatement.GetTextureOverridesByObject, aceObject.AceObjectId);
            aceObject.AnimationOverrides = LoadAceTable<AnimationOverride>(ShardPreparedStatement.GetAnimationOverridesByObject, aceObject.AceObjectId);
            aceObject.PaletteOverrides = LoadAceTable<PaletteOverride>(ShardPreparedStatement.GetPaletteOverridesByObject, aceObject.AceObjectId);
            aceObject.AceObjectPropertiesPositions = LoadAceTable<AceObjectPropertiesPosition>(ShardPreparedStatement.GetAceObjectPropertiesPositions, aceObject.AceObjectId).ToDictionary(x => (PositionType)x.DbPositionType,
                x => new Position(x));
            aceObject.AceObjectPropertiesAttributes = LoadAceTable<AceObjectPropertiesAttribute>(ShardPreparedStatement.GetAceObjectPropertiesAttributes, aceObject.AceObjectId).ToDictionary(x => (Ability)x.AttributeId,
                x => new CreatureAbility(x));
            aceObject.AceObjectPropertiesAttributes2nd = LoadAceTable<AceObjectPropertiesAttribute2nd>(ShardPreparedStatement.GetAceObjectPropertiesAttributes2nd, aceObject.AceObjectId).ToDictionary(x => (Ability)x.Attribute2ndId,
                x => new CreatureVital(aceObject, x));
            aceObject.AceObjectPropertiesSkills = LoadAceTable<AceObjectPropertiesSkill>(ShardPreparedStatement.GetAceObjectPropertiesSkills, aceObject.AceObjectId).ToDictionary(x => (Skill)x.SkillId,
                x => new CreatureSkill(aceObject, x));
        }

        private List<T1> LoadAceTable<T1>(ShardPreparedStatement statement, uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, T1>(statement, criteria);
            return objects;
        }

        public Task<ObjectInfo> GetObjectInfoByName(string name)
        {
            throw new NotImplementedException();
        }

        private AceObject GetWorldObject(uint objId)
        {
            AceObject ret = new AceObject();
            var criteria = new Dictionary<string, object> { { "aceObjectId", objId } };
            bool success = ExecuteConstructedGetStatement<ShardPreparedStatement>(ShardPreparedStatement.GetAceObject, typeof(AceObject), criteria, ret);
            if (!success)
            {
                return null;
            }
            return ret;
        }

        private Dictionary<PositionType, Position> GetAceObjectPropertiesPositions(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, AceObjectPropertiesPosition>(ShardPreparedStatement.GetAceObjectPropertiesPositions, criteria);
            return objects.ToDictionary(x => (PositionType)x.DbPositionType, x => new Position(x));
        }

        public List<AceObject> GetObjectsByLandblock(ushort landblock)
        {
            var criteria = new Dictionary<string, object> { { "landblock", landblock } };
            var objects = ExecuteConstructedGetListStatement<ShardPreparedStatement, CachedWorldObject>(ShardPreparedStatement.GetObjectsByLandblock, criteria);
            List<AceObject> ret = new List<AceObject>();
            objects.ForEach(cwo =>
            {
                // Note(ddevec): Use GetObject to go through the cache (in case objects are saving)
                ret.Add(GetObject(cwo.AceObjectId));
            });
            return ret;
        }

        public bool IsCharacterNameAvailable(string name)
        {
            var cc = new CachedCharacter();
            var criteria = new Dictionary<string, object> { { "name", name } };
            return !(ExecuteConstructedGetStatement(ShardPreparedStatement.IsCharacterNameAvailable, typeof(CachedCharacter), criteria, cc));
        }

        public uint RenameCharacter(string currentName, string newName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveObject(AceObject aceObject)
        {
            long saveVersion;
            lock (cacheLock)
            {
                uint key = aceObject.AceObjectId;
                var valueTuple = new Tuple<AceObject, long>(aceObject, cacheVersion);
                saveVersion = cacheVersion;
                cacheVersion++;

                if (saveCache.ContainsKey(key))
                {
                    saveCache[key] = valueTuple;
                }
                else
                {
                    saveCache.Add(key, valueTuple);
                }
            }
            Task<bool> ret = new Task<bool>(() => SaveObjectInternal(aceObject));
            saveObjects.Add(new Tuple<Task<bool>, uint, long>(ret, aceObject.AceObjectId, saveVersion));
            return ret;
        }

        /// <summary>
        /// Backend save task, run by background thread.  Should only be invoked from BackgroundSaveTask though tasks passed to it.
        /// All other saving should be done by SaveObject instead
        /// </summary>
        /// <param name="aceObject"></param>
        /// <returns></returns>
        private bool SaveObjectInternal(AceObject aceObject)
        {
            DatabaseTransaction transaction = BeginTransaction();

            // For now we're just going to go simple and wipe and re-run this sucker -- make sure our inserst/updates work

            // TODO : this never completes as far as I can tell.   I think this is why positions are not saving.   Og II
            // I can't see any commit or any database updates taking place.   Nor any errors that I can see.

            DeleteObjectInternal(transaction, aceObject);
            SaveObjectInternal(transaction, aceObject);

            // FIXME(ddevec): Should we wait on this TXN?  I have no idea.
            Task<bool> txn = transaction.Commit();
            txn.Wait();

            return txn.Result;
        }

        public uint SetCharacterAccessLevelByName(string name, AccessLevel accessLevel)
        {
            throw new NotImplementedException();
        }

        private bool DeleteObjectInternal(DatabaseTransaction transaction, AceObject aceObject)
        {
            ListDeletePreparedStatement<AceObjectPropertiesInt>(ShardPreparedStatement.DeleteAceObjectPropertiesInt, transaction, aceObject.AceObjectId);
            ListDeletePreparedStatement<AceObjectPropertiesInt64>(ShardPreparedStatement.DeleteAceObjectPropertiesBigInt, transaction, aceObject.AceObjectId);
            ListDeletePreparedStatement<AceObjectPropertiesBool>(ShardPreparedStatement.DeleteAceObjectPropertiesBool, transaction, aceObject.AceObjectId);
            ListDeletePreparedStatement<AceObjectPropertiesDouble>(ShardPreparedStatement.DeleteAceObjectPropertiesDouble, transaction, aceObject.AceObjectId);
            ListDeletePreparedStatement<AceObjectPropertiesString>(ShardPreparedStatement.DeleteAceObjectPropertiesString, transaction, aceObject.AceObjectId);
            ListDeletePreparedStatement<AceObjectPropertiesInstanceId>(ShardPreparedStatement.DeleteAceObjectPropertiesIid, transaction, aceObject.AceObjectId);
            ListDeletePreparedStatement<AceObjectPropertiesDataId>(ShardPreparedStatement.DeleteAceObjectPropertiesDid, transaction, aceObject.AceObjectId);

            ListDeletePreparedStatement<TextureMapOverride>(ShardPreparedStatement.DeleteTextureOverridesByObject, transaction, aceObject.AceObjectId);
            ListDeletePreparedStatement<AnimationOverride>(ShardPreparedStatement.DeleteAnimationOverridesByObject, transaction, aceObject.AceObjectId);
            ListDeletePreparedStatement<PaletteOverride>(ShardPreparedStatement.DeletePaletteOverridesByObject, transaction, aceObject.AceObjectId);

            ListDeletePreparedStatement<AceObjectPropertiesPosition>(ShardPreparedStatement.DeleteAceObjectPropertiesPositions, transaction, aceObject.AceObjectId);
            ListDeletePreparedStatement<AceObjectPropertiesAttribute>(ShardPreparedStatement.DeleteAceObjectPropertiesAttributes, transaction, aceObject.AceObjectId);
            ListDeletePreparedStatement<AceObjectPropertiesAttribute2nd>(ShardPreparedStatement.DeleteAceObjectPropertiesAttributes2nd, transaction, aceObject.AceObjectId);
            ListDeletePreparedStatement<AceObjectPropertiesSkill>(ShardPreparedStatement.DeleteAceObjectPropertiesSkills, transaction, aceObject.AceObjectId);

            DeleteAceObjectBase(transaction, aceObject);

            return true;
        }

        private bool SaveObjectInternal(DatabaseTransaction transaction, AceObject aceObject)
        {
            // Update the character table -- save the AceObject to ace_object.

            SavePreparedStatement<AceObject>(ShardPreparedStatement.SaveAceObject, transaction, aceObject);

            ListSavePreparedStatement<AceObjectPropertiesInt>(ShardPreparedStatement.InsertAceObjectPropertiesInt, transaction, aceObject.IntProperties);
            ListSavePreparedStatement<AceObjectPropertiesInt64>(ShardPreparedStatement.InsertAceObjectPropertiesBigInt, transaction, aceObject.Int64Properties);
            ListSavePreparedStatement<AceObjectPropertiesBool>(ShardPreparedStatement.InsertAceObjectPropertiesBool, transaction, aceObject.BoolProperties);
            ListSavePreparedStatement<AceObjectPropertiesDouble>(ShardPreparedStatement.InsertAceObjectPropertiesDouble, transaction, aceObject.DoubleProperties);
            ListSavePreparedStatement<AceObjectPropertiesString>(ShardPreparedStatement.InsertAceObjectPropertiesString, transaction, aceObject.StringProperties);
            ListSavePreparedStatement<AceObjectPropertiesInstanceId>(ShardPreparedStatement.InsertAceObjectPropertiesIid, transaction, aceObject.InstanceIdProperties);
            ListSavePreparedStatement<AceObjectPropertiesDataId>(ShardPreparedStatement.InsertAceObjectPropertiesDid, transaction, aceObject.DataIdProperties);
            ListSavePreparedStatement<TextureMapOverride>(ShardPreparedStatement.InsertTextureOverridesByObject, transaction, aceObject.TextureOverrides);
            ListSavePreparedStatement<AnimationOverride>(ShardPreparedStatement.InsertAnimationOverridesByObject, transaction, aceObject.AnimationOverrides);
            ListSavePreparedStatement<PaletteOverride>(ShardPreparedStatement.InsertPaletteOverridesByObject, transaction, aceObject.PaletteOverrides);

            ListSavePreparedStatement<AceObjectPropertiesPosition>(ShardPreparedStatement.InsertAceObjectPropertiesPositions, transaction,
                aceObject.AceObjectPropertiesPositions.Select(x => x.Value.GetAceObjectPosition(aceObject.AceObjectId, x.Key)).ToList());
            ListSavePreparedStatement<AceObjectPropertiesAttribute>(ShardPreparedStatement.InsertAceObjectPropertiesAttributes, transaction,
                aceObject.AceObjectPropertiesAttributes.Select(x => x.Value.GetAttribute(aceObject.AceObjectId)).ToList());
            ListSavePreparedStatement<AceObjectPropertiesAttribute2nd>(ShardPreparedStatement.InsertAceObjectPropertiesAttributes2nd, transaction,
                aceObject.AceObjectPropertiesAttributes2nd.Select(x => x.Value.GetVital(aceObject.AceObjectId)).ToList());
            ListSavePreparedStatement<AceObjectPropertiesSkill>(ShardPreparedStatement.InsertAceObjectPropertiesSkills, transaction,
                aceObject.AceObjectPropertiesSkills.Select(x => x.Value.GetAceObjectSkill(aceObject.AceObjectId)).ToList());

            return true;
        }

        private void SavePreparedStatement<T1>(ShardPreparedStatement statement, DatabaseTransaction transaction, T1 obj)
        {
            transaction.AddPreparedInsertStatement<ShardPreparedStatement, T1>(statement, obj);
        }

        private void ListSavePreparedStatement<T1>(ShardPreparedStatement statement, DatabaseTransaction transaction, List<T1> obj)
        {
            transaction.AddPreparedInsertListStatement<ShardPreparedStatement, T1>(statement, obj);
        }

        private void DeleteAceObjectBase(DatabaseTransaction transaction, AceObject obj)
        {
            transaction.AddPreparedDeleteStatement<ShardPreparedStatement, AceObject>(ShardPreparedStatement.DeleteAceObject, obj);
        }

        private void ListDeletePreparedStatement<T1>(ShardPreparedStatement statement, DatabaseTransaction transaction, uint aceObjectId)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<ShardPreparedStatement, T1>(statement, critera);
        }

        /// <summary>
        /// Shuts down the background saving thread
        /// </summary>
        public void Shutdown()
        {
            saveObjects.CompleteAdding();
            taskCleanThread.Join();
        } 

        /// <summary>
        /// Background task for sending saves off to Db when needed, and maintaining our internal cache
        /// </summary>
        private static void SaveBackgroundTask()
        {
            // Stops us from shutting down before all the saves are done
            while (!saveObjects.IsCompleted)
            {
                try
                {
                    var saveTuple = saveObjects.Take();
                    Task<bool> saveTask = saveTuple.Item1;
                    uint guid = saveTuple.Item2;
                    long version = saveTuple.Item3;

                    saveTask.Start();
                    saveTask.Wait();

                    lock (cacheLock)
                    {
                        // NOTE(ddevec): The item MUST exist in the cache if we just ran it, no need to check key
                        var cacheTup = saveCache[guid];
                        long cachedVersion = cacheTup.Item2;
                        if (cachedVersion == version)
                        {
                            saveCache.Remove(guid);
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    // Note(ddevec): This can only happen if we're calling "Take()" when we're getting ready to shut down
                    //    Just ignore that instance
                }
            }
        }
    }
}
