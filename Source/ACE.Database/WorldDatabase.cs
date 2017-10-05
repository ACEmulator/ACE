using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Database
{
    public class WorldDatabase : Database, IWorldDatabase
    {
        private enum WorldPreparedStatement
        {
            GetPointsOfInterest,
            GetWeenieClass,
            GetObjectsByLandblock,
            GetCreaturesByLandblock,
            GetWeeniePalettes,
            GetWeenieTextureMaps,
            GetWeenieAnimations,
            GetPaletteOverridesByObject,
            GetAnimationOverridesByObject,
            GetTextureOverridesByObject,
            GetCreatureDataByWeenie,
            InsertCreatureStaticLocation,
            GetCreatureGeneratorByLandblock,
            GetCreatureGeneratorData,
            GetPortalObjectsByAceObjectId,
            GetItemsByTypeId,
            GetAceObjectPropertiesInt,
            GetAceObjectPropertiesBigInt,
            GetAceObjectPropertiesDouble,
            GetAceObjectPropertiesBool,
            GetAceObjectPropertiesString,
            GetAceObjectPropertiesDid,
            GetAceObjectPropertiesIid,
            GetAceObject,
            GetAceObjectPropertiesPosition,
            GetAceObjectPropertiesSpell,
            GetAceObjectGeneratorLinks,
            GetMaxId,
            GetAceObjectPropertiesAttributes,
            GetAceObjectPropertiesAttributes2nd,
            GetAceObjectPropertiesSkills,
            GetAceObjectPropertiesBook,
            GetWeenieInstancesByLandblock,
            GetAllRecipes,
            GetVendorWeenieInventoryById
        }

        protected override Type PreparedStatementType => typeof(WorldPreparedStatement);

        private void ConstructMaxQueryStatement(WorldPreparedStatement id, string tableName, string columnName)
        {
            // NOTE: when moved to WordDatabase, ace_shard needs to be changed to ace_world
            AddPreparedStatement<WorldPreparedStatement>(id, $"SELECT MAX(`{columnName}`) FROM `{tableName}` WHERE `{columnName}` >= ? && `{columnName}` < ?",
                MySqlDbType.UInt32, MySqlDbType.UInt32);
        }

        protected override void InitializePreparedStatements()
        {
            ConstructStatement(WorldPreparedStatement.GetPointsOfInterest, typeof(TeleportLocation), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetWeenieClass, typeof(AceObject), ConstructedStatementType.Get);
            HashSet<string> criteria1 = new HashSet<string> { "itemType" };
            ConstructGetListStatement(WorldPreparedStatement.GetItemsByTypeId, typeof(CachedWeenieClass), criteria1);
            HashSet<string> criteria2 = new HashSet<string> { "landblock" };
            ConstructGetListStatement(WorldPreparedStatement.GetObjectsByLandblock, typeof(CachedWorldObject), criteria2);
            ConstructStatement(WorldPreparedStatement.GetTextureOverridesByObject, typeof(TextureMapOverride), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetPaletteOverridesByObject, typeof(PaletteOverride), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAnimationOverridesByObject, typeof(AnimationOverride), ConstructedStatementType.GetList);

            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesAttributes, typeof(AceObjectPropertiesAttribute), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesAttributes2nd, typeof(AceObjectPropertiesAttribute2nd), ConstructedStatementType.GetList);
            // Uncomment below when Skills are in database
            // ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesSkills, typeof(AceObjectPropertiesSkill), ConstructedStatementType.GetList);

            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesString, typeof(AceObjectPropertiesString), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesPosition, typeof(AceObjectPropertiesPosition), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesSpell, typeof(AceObjectPropertiesSpell), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectGeneratorLinks, typeof(AceObjectGeneratorLink), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesBook, typeof(AceObjectPropertiesBook), ConstructedStatementType.GetList);

            ConstructStatement(WorldPreparedStatement.GetAceObject, typeof(AceObject), ConstructedStatementType.Get);

            ConstructMaxQueryStatement(WorldPreparedStatement.GetMaxId, "ace_object", "aceObjectId");

            ConstructGetListStatement(WorldPreparedStatement.GetWeenieInstancesByLandblock, typeof(WeenieObjectInstance), criteria2);

            ConstructGetListStatement(WorldPreparedStatement.GetAllRecipes, typeof(Recipe), new HashSet<string>());

            ConstructGetListStatement(WorldPreparedStatement.GetVendorWeenieInventoryById, typeof(VendorItems), new HashSet<string> { "aceObjectId" });
        }

        public List<CachedWeenieClass> GetRandomWeeniesOfType(uint itemType, uint numWeenies)
        {
            var criteria = new Dictionary<string, object> { { "itemType", itemType } };
            var weenieList = ExecuteConstructedGetListStatement<WorldPreparedStatement, CachedWeenieClass>(WorldPreparedStatement.GetItemsByTypeId, criteria);
            if (weenieList.Count <= 0) return null;
            Random rnd = new Random();
            int r = rnd.Next(weenieList.Count);
            var randomWeenieList = new List<CachedWeenieClass>();
            for (int i = 0; i < numWeenies; i++)
            {
                randomWeenieList.Add(weenieList[r]);
                r = rnd.Next(weenieList.Count);
            }
            return randomWeenieList;
        }      

        public List<AceObject> GetObjectsByLandblock(ushort landblock)
        {
            var criteria = new Dictionary<string, object> { { "landblock", landblock } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, CachedWorldObject>(WorldPreparedStatement.GetObjectsByLandblock, criteria);
            List<AceObject> ret = new List<AceObject>();
            objects.ForEach(cwo =>
            {
                var o = GetWorldObject(cwo.AceObjectId);
                o.DataIdProperties = GetAceObjectPropertiesDid(o.AceObjectId);
                o.InstanceIdProperties = GetAceObjectPropertiesIid(o.AceObjectId);
                o.IntProperties = GetAceObjectPropertiesInt(o.AceObjectId);
                o.Int64Properties = GetAceObjectPropertiesBigInt(o.AceObjectId);
                o.BoolProperties = GetAceObjectPropertiesBool(o.AceObjectId);
                o.DoubleProperties = GetAceObjectPropertiesDouble(o.AceObjectId);
                o.StringProperties = GetAceObjectPropertiesString(o.AceObjectId);
                o.TextureOverrides = GetAceObjectTextureMaps(o.AceObjectId);
                o.AnimationOverrides = GetAceObjectAnimations(o.AceObjectId);
                o.PaletteOverrides = GetAceObjectPalettes(o.AceObjectId);
                o.SpellIdProperties = GetAceObjectPropertiesSpell(o.AceObjectId);
                o.GeneratorLinks = GetAceObjectGeneratorLinks(o.AceObjectId);
                o.AceObjectPropertiesPositions = GetAceObjectPositions(o.AceObjectId).ToDictionary(x => (PositionType)x.DbPositionType, x => new Position(x));
                o.BookProperties = GetAceObjectPropertiesBook(o.AceObjectId).ToDictionary(x => x.Page);
                ret.Add(o);
            });
            return ret;
        }

        public List<AceObject> GetWeenieInstancesByLandblock(ushort landblock)
        {
            var criteria = new Dictionary<string, object> { { "landblock", landblock } };
            var instances = ExecuteConstructedGetListStatement<WorldPreparedStatement, WeenieObjectInstance>(WorldPreparedStatement.GetWeenieInstancesByLandblock, criteria);
            List<AceObject> ret = new List<AceObject>();
            instances.ForEach(instance =>
            {
                // Create a new AceObject from Weenie.
                AceObject ao = GetWorldObject(instance.WeenieClassId);

                // Load in the object's properties from the weenie.
                ao.DataIdProperties = GetAceObjectPropertiesDid(ao.AceObjectId);
                ao.InstanceIdProperties = GetAceObjectPropertiesIid(ao.AceObjectId);
                ao.IntProperties = GetAceObjectPropertiesInt(ao.AceObjectId);
                ao.Int64Properties = GetAceObjectPropertiesBigInt(ao.AceObjectId);
                ao.BoolProperties = GetAceObjectPropertiesBool(ao.AceObjectId);
                ao.DoubleProperties = GetAceObjectPropertiesDouble(ao.AceObjectId);
                ao.StringProperties = GetAceObjectPropertiesString(ao.AceObjectId);
                ao.TextureOverrides = GetAceObjectTextureMaps(ao.AceObjectId);
                ao.AnimationOverrides = GetAceObjectAnimations(ao.AceObjectId);
                ao.PaletteOverrides = GetAceObjectPalettes(ao.AceObjectId);
                ao.SpellIdProperties = GetAceObjectPropertiesSpell(ao.AceObjectId);
                ao.GeneratorLinks = GetAceObjectGeneratorLinks(ao.AceObjectId);
                ao.AceObjectPropertiesPositions = GetAceObjectPositions(ao.AceObjectId).ToDictionary(x => (PositionType)x.DbPositionType, x => new Position(x));
                ao.BookProperties = GetAceObjectPropertiesBook(ao.AceObjectId).ToDictionary(x => x.Page);

                // Set the object's current location for this instance.
                ao.Location = new Position(instance.LandblockRaw, 
                    instance.PositionX, instance.PositionY, instance.PositionZ, 
                    instance.RotationX, instance.RotationY, instance.RotationZ, instance.RotationW);

                // Use the guid recorded by the PCAP.
                // This step could eventually be removed if we want to let the GuidManager handle assigning guids for static objects, ignoring the recorded guids.
                string cmsClone = ao.CurrentMotionState; // Make a copy of the CurrentMotionState for cloning
                ao = (AceObject)ao.Clone(instance.PreassignedGuid); // Clone AceObject and assign the recorded Guid
                ao.CurrentMotionState = cmsClone; // Restore CurrentMotionState from original weenie

                ret.Add(ao);
            });
            return ret;
        }

        public AceObject GetWorldObject(uint objId)
        {
            AceObject ret = new AceObject();
            var criteria = new Dictionary<string, object> { { "aceObjectId", objId } };
            bool success = ExecuteConstructedGetStatement<WorldPreparedStatement>(WorldPreparedStatement.GetAceObject, typeof(AceObject), criteria, ret);
            if (!success)
            {
                return null;
            }
            return ret;
        }

        private List<PaletteOverride> GetWeeniePalettes(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object>();
            criteria.Add("aceObjectId", aceObjectId);
            return ExecuteConstructedGetListStatement<WorldPreparedStatement, PaletteOverride>(WorldPreparedStatement.GetWeeniePalettes, criteria);
        }

        private List<TextureMapOverride> GetWeenieTextureMaps(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            return ExecuteConstructedGetListStatement<WorldPreparedStatement, TextureMapOverride>(WorldPreparedStatement.GetWeenieTextureMaps, criteria);
        }

        private List<AnimationOverride> GetWeenieAnimations(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            return ExecuteConstructedGetListStatement<WorldPreparedStatement, AnimationOverride>(WorldPreparedStatement.GetWeenieAnimations, criteria);
        }

        private List<TextureMapOverride> GetAceObjectTextureMaps(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, TextureMapOverride>(WorldPreparedStatement.GetTextureOverridesByObject, criteria);
            return objects;
        }

        private List<AceObjectPropertiesPosition> GetAceObjectPositions(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesPosition>(WorldPreparedStatement.GetAceObjectPropertiesPosition, criteria);
            return objects;
        }

        private List<PaletteOverride> GetAceObjectPalettes(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, PaletteOverride>(WorldPreparedStatement.GetPaletteOverridesByObject, criteria);
            return objects;
        }

        private List<AnimationOverride> GetAceObjectAnimations(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AnimationOverride>(WorldPreparedStatement.GetAnimationOverridesByObject, criteria);
            return objects;
        }

        private List<AceObjectGeneratorLink> GetAceObjectGeneratorLinks(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectGeneratorLink>(WorldPreparedStatement.GetAceObjectGeneratorLinks, criteria);
            return objects;
        }

        public AceObject GetAceObjectByWeenie(uint weenieClassId)
        {
            var bao = new AceObject();

            // We can do this because aceObjectId = WeenieClassId for all baseAceObjects.
            // TODO: Ask Mogwai how would you query on a secondary key?
            var criteria = new Dictionary<string, object> { { "aceObjectId", weenieClassId } };
            if (!ExecuteConstructedGetStatement(WorldPreparedStatement.GetWeenieClass, typeof(AceObject), criteria, bao))
                return null;
            bao.DataIdProperties = GetAceObjectPropertiesDid(bao.AceObjectId);
            bao.InstanceIdProperties = GetAceObjectPropertiesIid(bao.AceObjectId);
            bao.IntProperties = GetAceObjectPropertiesInt(bao.AceObjectId);
            bao.Int64Properties = GetAceObjectPropertiesBigInt(bao.AceObjectId);
            bao.BoolProperties = GetAceObjectPropertiesBool(bao.AceObjectId);
            bao.DoubleProperties = GetAceObjectPropertiesDouble(bao.AceObjectId);
            bao.StringProperties = GetAceObjectPropertiesString(bao.AceObjectId);
            bao.TextureOverrides = GetAceObjectTextureMaps(bao.AceObjectId);
            bao.AnimationOverrides = GetAceObjectAnimations(bao.AceObjectId);
            bao.PaletteOverrides = GetAceObjectPalettes(bao.AceObjectId);
            bao.SpellIdProperties = GetAceObjectPropertiesSpell(bao.AceObjectId);
            bao.GeneratorLinks = GetAceObjectGeneratorLinks(bao.AceObjectId);
            bao.AceObjectPropertiesPositions = GetAceObjectPositions(bao.AceObjectId).ToDictionary(x => (PositionType)x.DbPositionType, x => new Position(x));
            bao.AceObjectPropertiesAttributes = GetAceObjectPropertiesAttribute(bao.AceObjectId).ToDictionary(x => (Ability)x.AttributeId,
                x => new CreatureAbility(x));
            bao.AceObjectPropertiesAttributes2nd = GetAceObjectPropertiesAttribute2nd(bao.AceObjectId).ToDictionary(x => (Ability)x.Attribute2ndId,
                x => new CreatureVital(bao, x));
            bao.BookProperties = GetAceObjectPropertiesBook(bao.AceObjectId).ToDictionary(x => x.Page);
            // Uncomment when we have skills saved to database to import
            // bao.AceObjectPropertiesSkills = GetAceObjectPropertiesSkill(bao.AceObjectId).ToDictionary(x => (Skill)x.SkillId,
            //    x => new CreatureSkill(bao, x));
            return bao;
        }

        private List<AceObjectPropertiesInt> GetAceObjectPropertiesInt(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesInt>(WorldPreparedStatement.GetAceObjectPropertiesInt, criteria);
            return objects;
        }

        private List<AceObjectPropertiesInt64> GetAceObjectPropertiesBigInt(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesInt64>(WorldPreparedStatement.GetAceObjectPropertiesBigInt, criteria);
            return objects;
        }

        private List<AceObjectPropertiesBool> GetAceObjectPropertiesBool(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesBool>(WorldPreparedStatement.GetAceObjectPropertiesBool, criteria);
            return objects;
        }

        private List<AceObjectPropertiesDouble> GetAceObjectPropertiesDouble(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesDouble>(WorldPreparedStatement.GetAceObjectPropertiesDouble, criteria);
            return objects;
        }

        private List<AceObjectPropertiesString> GetAceObjectPropertiesString(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesString>(WorldPreparedStatement.GetAceObjectPropertiesString, criteria);
            return objects;
        }

        private List<AceObjectPropertiesDataId> GetAceObjectPropertiesDid(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesDataId>(WorldPreparedStatement.GetAceObjectPropertiesDid, criteria);
            return objects;
        }

        private List<AceObjectPropertiesInstanceId> GetAceObjectPropertiesIid(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesInstanceId>(WorldPreparedStatement.GetAceObjectPropertiesIid, criteria);
            return objects;
        }

        private List<AceObjectPropertiesSpell> GetAceObjectPropertiesSpell(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesSpell>(WorldPreparedStatement.GetAceObjectPropertiesSpell, criteria);
            return objects;
        }

        private List<AceObjectPropertiesSkill> GetAceObjectPropertiesSkill(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesSkill>(WorldPreparedStatement.GetAceObjectPropertiesSkills, criteria);
            return objects;
        }

        private List<AceObjectPropertiesAttribute> GetAceObjectPropertiesAttribute(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesAttribute>(WorldPreparedStatement.GetAceObjectPropertiesAttributes, criteria);
            return objects;
        }

        private List<AceObjectPropertiesAttribute2nd> GetAceObjectPropertiesAttribute2nd(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesAttribute2nd>(WorldPreparedStatement.GetAceObjectPropertiesAttributes2nd, criteria);
            return objects;
        }

        private List<AceObjectPropertiesBook> GetAceObjectPropertiesBook(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesBook>(WorldPreparedStatement.GetAceObjectPropertiesBook, criteria);
            return objects;
        }

        public AceObject GetObject(uint aceObjectId)
        {
            throw new NotImplementedException();
        }

        // TODO: this needs to be refactored to just replace all calls to GetWeenie with the other method which should be renamed.
        public AceObject GetWeenie(uint weenieClassId)
        {
            return GetAceObjectByWeenie(weenieClassId);
        }

        public List<TeleportLocation> GetPointsOfInterest()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, TeleportLocation>(WorldPreparedStatement.GetPointsOfInterest, criteria);
            return objects;
        }

        public Task<bool> SaveObject(AceObject aceObject)
        {
            // Temp took out async until we implement this to kill the warning.
            throw new NotImplementedException();
        }

        private uint GetMaxGuid(WorldPreparedStatement id, uint min, uint max)
        {
            object[] critera = new object[] { min, max };
            MySqlResult res = SelectPreparedStatement<WorldPreparedStatement>(id, critera);
            var ret = res.Rows[0][0];
            if (ret is DBNull)
            {
                return uint.MaxValue;
            }

            return (uint)res.Rows[0][0];
        }

        public uint GetCurrentId(uint min, uint max)
        {
            return GetMaxGuid(WorldPreparedStatement.GetMaxId, min, max);
        }

        public List<Recipe> GetAllRecipes()
        {
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, Recipe>(WorldPreparedStatement.GetAllRecipes, new Dictionary<string, object>());
            return objects;
        }

        public List<VendorItems> GetVendorWeenieInventoryById(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, VendorItems>(WorldPreparedStatement.GetVendorWeenieInventoryById, criteria);
            return objects;
        }
    }
}
