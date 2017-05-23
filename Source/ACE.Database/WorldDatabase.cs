using System;
using System.Collections.Generic;
using ACE.Entity;

namespace ACE.Database
{
    public class WorldDatabase : Database, IWorldDatabase
    {
        private enum WorldPreparedStatement
        {
            TeleportLocationSelect = 0,
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
            GetAceObjectPropertiesString = 20
        }

        protected override Type PreparedStatementType => typeof(WorldPreparedStatement);

        protected override void InitializePreparedStatements()
        {
            // TODO: Og II - switch to constructed Statement
            AddPreparedStatement(
                WorldPreparedStatement.TeleportLocationSelect,
                "SELECT `name`, `landblock`, `posX`, `posY`, `posZ`, `qx`, `qy`, `qz`, `qw` FROM `vw_teleport_location`;");
            ConstructStatement(
                WorldPreparedStatement.GetWeenieClass,
                typeof(BaseAceObject),
                ConstructedStatementType.Get);

            ConstructStatement(WorldPreparedStatement.GetPortalObjectsByAceObjectId, typeof(AcePortalObject), ConstructedStatementType.Get);
            ConstructStatement(WorldPreparedStatement.GetObjectsByLandblock, typeof(AceObject), ConstructedStatementType.GetList);
            // ConstructStatement(WorldPreparedStatement.GetCreaturesByLandblock, typeof(AceCreatureStaticLocation), ConstructedStatementType.GetList);
            // ConstructStatement(WorldPreparedStatement.GetWeeniePalettes, typeof(WeeniePaletteOverride), ConstructedStatementType.GetList);
            // ConstructStatement(WorldPreparedStatement.GetWeenieTextureMaps, typeof(WeenieTextureMapOverride), ConstructedStatementType.GetList);
            // ConstructStatement(WorldPreparedStatement.GetWeenieAnimations, typeof(WeenieAnimationOverride), ConstructedStatementType.GetList);
            ConstructStatement(
                WorldPreparedStatement.GetTextureOverridesByObject,
                typeof(TextureMapOverride),
                ConstructedStatementType.GetList);
            ConstructStatement(
                WorldPreparedStatement.GetPaletteOverridesByObject,
                typeof(PaletteOverride),
                ConstructedStatementType.GetList);
            ConstructStatement(
                WorldPreparedStatement.GetAnimationOverridesByObject,
                typeof(AnimationOverride),
                ConstructedStatementType.GetList);

            // ConstructStatement(WorldPreparedStatement.GetCreatureDataByWeenie, typeof(AceCreatureObject), ConstructedStatementType.Get);
            // ConstructStatement(WorldPreparedStatement.InsertCreatureStaticLocation, typeof(AceCreatureStaticLocation), ConstructedStatementType.Insert);
            // ConstructStatement(WorldPreparedStatement.GetCreatureGeneratorByLandblock, typeof(AceCreatureGeneratorLocation), ConstructedStatementType.GetList);
            // ConstructStatement(WorldPreparedStatement.GetCreatureGeneratorData, typeof(AceCreatureGeneratorData), ConstructedStatementType.GetList);
            ConstructStatement(
                WorldPreparedStatement.GetItemsByTypeId,
                typeof(BaseAceObject),
                ConstructedStatementType.GetList);
            ConstructStatement(
                WorldPreparedStatement.GetAceObjectPropertiesInt,
                typeof(AceObjectPropertiesInt),
                ConstructedStatementType.GetList);
            ConstructStatement(
                WorldPreparedStatement.GetAceObjectPropertiesBigInt,
                typeof(AceObjectPropertiesBigInt),
                ConstructedStatementType.GetList);
            ConstructStatement(
                WorldPreparedStatement.GetAceObjectPropertiesBool,
                typeof(AceObjectPropertiesBool),
                ConstructedStatementType.GetList);
            ConstructStatement(
                WorldPreparedStatement.GetAceObjectPropertiesDouble,
                typeof(AceObjectPropertiesDouble),
                ConstructedStatementType.GetList);
            ConstructStatement(
                WorldPreparedStatement.GetAceObjectPropertiesString,
                typeof(AceObjectPropertiesString),
                ConstructedStatementType.GetList);
        }

        public BaseAceObject GetRandomBaseAceObjectByTypeId(uint itemType)
        {
            var criteria = new Dictionary<string, object> { { "itemType", itemType } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, BaseAceObject>(WorldPreparedStatement.GetItemsByTypeId, criteria);
            if (objects.Count <= 0) return null;
            var rnd = new Random();
            var r = rnd.Next(objects.Count);
            return GetBaseAceObjectDataByWeenie(objects[r].WeenieClassId);
        }

        public List<TeleportLocation> GetLocations()
        {
            var result = SelectPreparedStatement(WorldPreparedStatement.TeleportLocationSelect);
            var locations = new List<TeleportLocation>();

            for (var i = 0u; i < result.Count; i++)
            {
                locations.Add(new TeleportLocation
                {
                    Location = result.Read<string>(i, "name"),
                    Position = new Position(result.Read<uint>(i, "landblock"), result.Read<float>(i, "posX"), result.Read<float>(i, "posY"),
                        result.Read<float>(i, "posZ"), result.Read<float>(i, "qx"), result.Read<float>(i, "qy"), result.Read<float>(i, "qz"), result.Read<float>(i, "qw"))
                });
            }

            return locations;
        }

        public AcePortalObject GetPortalObjectsByAceObjectId(uint aceObjectId)
        {
            var apo = new AcePortalObject();
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            if (ExecuteConstructedGetStatement(WorldPreparedStatement.GetPortalObjectsByAceObjectId, typeof(AcePortalObject), criteria, apo))
            {
                apo.AceObjectPropertiesInt = GetAceObjectPropertiesInt(apo.AceObjectId);
                apo.AceObjectPropertiesBigInt = GetAceObjectPropertiesBigInt(apo.AceObjectId);
                apo.AceObjectPropertiesBool = GetAceObjectPropertiesBool(apo.AceObjectId);
                apo.AceObjectPropertiesDouble = GetAceObjectPropertiesDouble(apo.AceObjectId);
                apo.AceObjectPropertiesString = GetAceObjectPropertiesString(apo.AceObjectId);
                apo.TextureOverrides = GetAceObjectTextureMaps(apo.AceObjectId);
                apo.AnimationOverrides = GetAceObjectAnimations(apo.AceObjectId);
                apo.PaletteOverrides = GetAceObjectPalettes(apo.AceObjectId);

                return apo;
            }
            return null;
        }

        public List<AceObject> GetObjectsByLandblock(uint landblock)
        {
            var criteria = new Dictionary<string, object> { { "landblock", landblock } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObject>(WorldPreparedStatement.GetObjectsByLandblock, criteria);
            objects.ForEach(o =>
            {
                o.AceObjectPropertiesInt = GetAceObjectPropertiesInt(o.AceObjectId);
                o.AceObjectPropertiesBigInt = GetAceObjectPropertiesBigInt(o.AceObjectId);
                o.AceObjectPropertiesBool = GetAceObjectPropertiesBool(o.AceObjectId);
                o.AceObjectPropertiesDouble = GetAceObjectPropertiesDouble(o.AceObjectId);
                o.AceObjectPropertiesString = GetAceObjectPropertiesString(o.AceObjectId);
                o.TextureOverrides = GetAceObjectTextureMaps(o.AceObjectId);
                o.AnimationOverrides = GetAceObjectAnimations(o.AceObjectId);
                o.PaletteOverrides = GetAceObjectPalettes(o.AceObjectId);
            });
            return objects;
        }

        public List<AceCreatureStaticLocation> GetCreaturesByLandblock(ushort landblock)
        {
            var criteria = new Dictionary<string, object> { { "landblock", landblock } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceCreatureStaticLocation>(WorldPreparedStatement.GetCreaturesByLandblock, criteria);
            objects.ForEach(o => o.CreatureData = GetCreatureDataByWeenie(o.WeenieClassId));

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

        public AceCreatureObject GetCreatureDataByWeenie(uint weenieClassId)
        {
            var aco = new AceCreatureObject();
            var criteria = new Dictionary<string, object> { { "weenieClassId", weenieClassId } };
            if (ExecuteConstructedGetStatement(WorldPreparedStatement.GetCreatureDataByWeenie, typeof(AceCreatureObject), criteria, aco))
            {
                aco.WeeniePaletteOverrides = GetWeeniePalettes(aco.WeenieClassId);
                aco.WeenieTextureMapOverrides = GetWeenieTextureMaps(aco.WeenieClassId);
                aco.WeenieAnimationOverrides = GetWeenieAnimations(aco.WeenieClassId);

                return aco;
            }
            else
                return null;
        }

        public BaseAceObject GetBaseAceObjectDataByWeenie(uint weenieClassId)
        {
            var bao = new BaseAceObject();

            // We can do this because aceObjectId = WeenieClassId for all baseAceObjects.
            // TODO: Ask Mogwai how would you query on a secondary key?
            var criteria = new Dictionary<string, object> { { "aceObjectId", weenieClassId } };
            if (
                !ExecuteConstructedGetStatement(
                    WorldPreparedStatement.GetWeenieClass,
                    typeof(BaseAceObject),
                    criteria,
                    bao)) return null;
            bao.AceObjectPropertiesInt = GetAceObjectPropertiesInt(bao.AceObjectId);
            bao.AceObjectPropertiesBigInt = GetAceObjectPropertiesBigInt(bao.AceObjectId);
            bao.AceObjectPropertiesBool = GetAceObjectPropertiesBool(bao.AceObjectId);
            bao.AceObjectPropertiesDouble = GetAceObjectPropertiesDouble(bao.AceObjectId);
            bao.AceObjectPropertiesString = GetAceObjectPropertiesString(bao.AceObjectId);

            foreach (var pal in this.GetAceObjectPalettes(bao.AceObjectId))
            {
                var opal = new PaletteOverride
                               {
                                   AceObjectId = pal.AceObjectId,
                                   Length = pal.Length,
                                   Offset = pal.Offset,
                                   SubPaletteId = pal.SubPaletteId
                               };
                bao.PaletteOverrides.Add(opal);
            }
            foreach (var tex in GetAceObjectTextureMaps(bao.AceObjectId))
            {
                var otex = new TextureMapOverride
                               {
                                   AceObjectId = tex.AceObjectId,
                                   Index = tex.Index,
                                   NewId = tex.NewId,
                                   OldId = tex.OldId
                               };
                bao.TextureOverrides.Add(otex);
            }
            foreach (var ani in this.GetAceObjectAnimations(bao.AceObjectId))
            {
                var oani = new AnimationOverride
                               {
                                   AceObjectId = ani.AceObjectId,
                                   AnimationId = ani.AnimationId,
                                   Index = ani.Index
                               };
                bao.AnimationOverrides.Add(oani);
            }
            return bao;
        }

        public bool InsertStaticCreatureLocation(AceCreatureStaticLocation acsl)
        {
            return ExecuteConstructedInsertStatement(WorldPreparedStatement.InsertCreatureStaticLocation, typeof(AceCreatureStaticLocation), acsl);
        }

        private List<AceObjectPropertiesInt> GetAceObjectPropertiesInt(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesInt>(WorldPreparedStatement.GetAceObjectPropertiesInt, criteria);
            return objects;
        }

        private List<AceObjectPropertiesBigInt> GetAceObjectPropertiesBigInt(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesBigInt>(WorldPreparedStatement.GetAceObjectPropertiesBigInt, criteria);
            return objects;
        }

        private List<AceObjectPropertiesBool> GetAceObjectPropertiesBool(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesBool>(WorldPreparedStatement.GetAceObjectPropertiesBool, criteria);
            return objects;
        }

        private List<AceObjectPropertiesDouble> GetAceObjectPropertiesDouble(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesDouble>(WorldPreparedStatement.GetAceObjectPropertiesDouble, criteria);
            return objects;
        }

        private List<AceObjectPropertiesString> GetAceObjectPropertiesString(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "AceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectPropertiesString>(WorldPreparedStatement.GetAceObjectPropertiesString, criteria);
            return objects;
        }
    }
}
