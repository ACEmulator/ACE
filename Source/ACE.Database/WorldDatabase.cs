using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Database
{
    public class WorldDatabase : Database, IWorldDatabase
    {
        private enum WorldPreparedStatement
        {
            GetPointsOfInterest = 0,
            GetWeenieClass = 1,
            GetObjectsByLandblock = 2,
            GetCreaturesByLandblock = 3,
            GetWeeniePalettes = 4,
            GetWeenieTextureMaps = 5,
            GetWeenieAnimations = 6,
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
            GetAceObject = 23,
            GetAceObjectPropertiesPosition = 24,
        }

        protected override Type PreparedStatementType => typeof(WorldPreparedStatement);

        protected override void InitializePreparedStatements()
        {
            ConstructStatement(WorldPreparedStatement.GetPointsOfInterest, typeof(TeleportLocation), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetWeenieClass, typeof(AceObject), ConstructedStatementType.Get);

            // ConstructStatement(WorldPreparedStatement.GetPortalObjectsByAceObjectId, typeof(AcePortalObject), ConstructedStatementType.Get);
            // ConstructStatement(WorldPreparedStatement.GetObjectsByLandblock, typeof(AceObject), ConstructedStatementType.GetList);
            // ConstructStatement(WorldPreparedStatement.GetCreaturesByLandblock, typeof(AceCreatureStaticLocation), ConstructedStatementType.GetList);
            // ConstructStatement(WorldPreparedStatement.GetWeeniePalettes, typeof(WeeniePaletteOverride), ConstructedStatementType.GetList);
            // ConstructStatement(WorldPreparedStatement.GetWeenieTextureMaps, typeof(WeenieTextureMapOverride), ConstructedStatementType.GetList);
            // ConstructStatement(WorldPreparedStatement.GetWeenieAnimations, typeof(WeenieAnimationOverride), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetTextureOverridesByObject, typeof(TextureMapOverride), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetPaletteOverridesByObject, typeof(PaletteOverride), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAnimationOverridesByObject, typeof(AnimationOverride), ConstructedStatementType.GetList);

            // ConstructStatement(WorldPreparedStatement.GetCreatureDataByWeenie, typeof(AceCreatureObject), ConstructedStatementType.Get);
            // ConstructStatement(WorldPreparedStatement.InsertCreatureStaticLocation, typeof(AceCreatureStaticLocation), ConstructedStatementType.Insert);
            // ConstructStatement(WorldPreparedStatement.GetCreatureGeneratorByLandblock, typeof(AceCreatureGeneratorLocation), ConstructedStatementType.GetList);
            // ConstructStatement(WorldPreparedStatement.GetCreatureGeneratorData, typeof(AceCreatureGeneratorData), ConstructedStatementType.GetList);
            // ConstructStatement(
            //     WorldPreparedStatement.GetItemsByTypeId,
            //     typeof(AceObject),
            //     ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesString, typeof(AceObjectPropertiesString), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObjectPropertiesPosition, typeof(AceObjectPropertiesPosition), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetObjectsByLandblock, typeof(CachedWordObject), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAceObject, typeof(AceObject), ConstructedStatementType.Get);
        }

        public AceObject GetRandomWeenieOfType(uint itemType)
        {
            var criteria = new Dictionary<string, object> { { "itemType", itemType } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObject>(WorldPreparedStatement.GetItemsByTypeId, criteria);
            if (objects.Count <= 0) return null;
            var rnd = new Random();
            var r = rnd.Next(objects.Count);
            return GetBaseAceObjectDataByWeenie(objects[r].WeenieClassId);
        }

        // public List<TeleportLocation> GetLocations()
        // {
        //     var result = SelectPreparedStatement(WorldPreparedStatement.TeleportLocationSelect);
        //     var locations = new List<TeleportLocation>();
        //
        //     for (var i = 0u; i < result.Count; i++)
        //     {
        //         locations.Add(new TeleportLocation
        //         {
        //             Location = result.Read<string>(i, "name"),
        //             Position = new Position(result.Read<uint>(i, "landblock"), result.Read<float>(i, "posX"), result.Read<float>(i, "posY"),
        //                 result.Read<float>(i, "posZ"), result.Read<float>(i, "qx"), result.Read<float>(i, "qy"), result.Read<float>(i, "qz"), result.Read<float>(i, "qw"))
        //         });
        //     }
        //
        //     return locations;
        // }

        ////public AcePortalObject GetPortalObjectsByAceObjectId(uint aceObjectId)
        ////{
        ////    var apo = new AcePortalObject();
        ////    var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
        ////    if (ExecuteConstructedGetStatement(WorldPreparedStatement.GetPortalObjectsByAceObjectId, typeof(AcePortalObject), criteria, apo))
        ////    {
        ////        apo.IntProperties = GetAceObjectPropertiesInt(apo.AceObjectId);
        ////        apo.Int64Properties = GetAceObjectPropertiesBigInt(apo.AceObjectId);
        ////        apo.BoolProperties = GetAceObjectPropertiesBool(apo.AceObjectId);
        ////        apo.DoubleProperties = GetAceObjectPropertiesDouble(apo.AceObjectId);
        ////        apo.StringProperties = GetAceObjectPropertiesString(apo.AceObjectId);
        ////        apo.TextureOverrides = GetAceObjectTextureMaps(apo.AceObjectId);
        ////        apo.AnimationOverrides = GetAceObjectAnimations(apo.AceObjectId);
        ////        apo.PaletteOverrides = GetAceObjectPalettes(apo.AceObjectId);

        ////        return apo;
        ////    }
        ////    return null;
        ////}

        public List<AceObject> GetObjectsByLandblock(ushort landblock)
        {
            var criteria = new Dictionary<string, object> { { "landblock", landblock } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, CachedWordObject>(WorldPreparedStatement.GetObjectsByLandblock, criteria);
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
                o.Positions = GetAceObjectPositions(o.AceObjectId);
                ret.Add(o);
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

        public List<AceCreatureStaticLocation> GetCreaturesByLandblock(ushort landblock)
        {
            var criteria = new Dictionary<string, object> { { "landblock", landblock } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceCreatureStaticLocation>(WorldPreparedStatement.GetCreaturesByLandblock, criteria);
            objects.ForEach(o => o.WeenieObject = GetWeenie(o.WeenieClassId));

            return objects;
        }

        public List<AceCreatureGeneratorLocation> GetCreatureGeneratorsByLandblock(ushort landblock)
        {
            var criteria = new Dictionary<string, object> { { "landblock", landblock } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceCreatureGeneratorLocation>(WorldPreparedStatement.GetCreatureGeneratorByLandblock, criteria);
            objects.ForEach(o => o.CreatureGeneratorData = GetCreatureGeneratorData(o.GeneratorId));

            return objects;
        }

        private List<AceCreatureGeneratorData> GetCreatureGeneratorData(uint generatorId)
        {
            var criteria = new Dictionary<string, object> { { "generatorId", generatorId } };
            return ExecuteConstructedGetListStatement<WorldPreparedStatement, AceCreatureGeneratorData>(WorldPreparedStatement.GetCreatureGeneratorData, criteria);
        }

        private List<WeeniePaletteOverride> GetWeeniePalettes(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object>();
            criteria.Add("aceObjectId", aceObjectId);
            return ExecuteConstructedGetListStatement<WorldPreparedStatement, WeeniePaletteOverride>(WorldPreparedStatement.GetWeeniePalettes, criteria);
        }

        private List<WeenieTextureMapOverride> GetWeenieTextureMaps(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            return ExecuteConstructedGetListStatement<WorldPreparedStatement, WeenieTextureMapOverride>(WorldPreparedStatement.GetWeenieTextureMaps, criteria);
        }

        private List<WeenieAnimationOverride> GetWeenieAnimations(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            return ExecuteConstructedGetListStatement<WorldPreparedStatement, WeenieAnimationOverride>(WorldPreparedStatement.GetWeenieAnimations, criteria);
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

        public AceObject GetBaseAceObjectDataByWeenie(uint weenieClassId)
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
            bao.Positions = GetAceObjectPositions(bao.AceObjectId);
            return bao;
        }

        public bool InsertStaticCreatureLocation(AceCreatureStaticLocation acsl)
        {
            return ExecuteConstructedInsertStatement(WorldPreparedStatement.InsertCreatureStaticLocation, typeof(AceCreatureStaticLocation), acsl);
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

        public AceObject GetObject(uint aceObjectId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveObject(AceObject aceObject)
        {
            throw new NotImplementedException();
        }

        // TODO: this needs to be refactored to just replace all calls to GetWeenie with the other method which should be renamed.
        public AceObject GetWeenie(uint weenieClassId)
        {
            return GetBaseAceObjectDataByWeenie(weenieClassId);
        }

        public List<TeleportLocation> GetPointsOfInterest()
        {
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, TeleportLocation>(WorldPreparedStatement.GetPointsOfInterest, null);
            return objects;
        }
    }
}
