using System;
using System.Collections.Generic;
using ACE.Entity;

namespace ACE.Database
{
    public class WorldDatabase : Database, IWorldDatabase
    {
        private enum WorldPreparedStatement
        {
            TeleportLocationSelect,
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
            GetSubPortalDataByWeenie
		}

        protected override Type PreparedStatementType => typeof(WorldPreparedStatement);

        protected override void InitializePreparedStatements()
        {
            AddPreparedStatement(WorldPreparedStatement.TeleportLocationSelect, "SELECT `location`, `cell`, `x`, `y`, `z`, `qx`, `qy`, `qz`, `qw` FROM `teleport_location`;");
            ConstructStatement(WorldPreparedStatement.GetWeenieClass, typeof(BaseAceObject), ConstructedStatementType.Get);
            ConstructStatement(WorldPreparedStatement.GetSubPortalDataByWeenie, typeof(AceSubPortalData), ConstructedStatementType.Get);
            ConstructStatement(WorldPreparedStatement.GetPortalObjectsByAceObjectId, typeof(AcePortalObject), ConstructedStatementType.Get);
            ConstructStatement(WorldPreparedStatement.GetObjectsByLandblock, typeof(AceObject), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetCreaturesByLandblock, typeof(AceCreatureStaticLocation), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetWeeniePalettes, typeof(WeeniePaletteOverride), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetWeenieTextureMaps, typeof(WeenieTextureMapOverride), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetWeenieAnimations, typeof(WeenieAnimationOverride), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetTextureOverridesByObject, typeof(TextureMapOverride), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetPaletteOverridesByObject, typeof(PaletteOverride), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetAnimationOverridesByObject, typeof(AnimationOverride), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetCreatureDataByWeenie, typeof(AceCreatureObject), ConstructedStatementType.Get);
            ConstructStatement(WorldPreparedStatement.InsertCreatureStaticLocation, typeof(AceCreatureStaticLocation), ConstructedStatementType.Insert);
            ConstructStatement(WorldPreparedStatement.GetCreatureGeneratorByLandblock, typeof(AceCreatureGeneratorLocation), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetCreatureGeneratorData, typeof(AceCreatureGeneratorData), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetItemsByTypeId, typeof(BaseAceObject), ConstructedStatementType.GetList);
        }

        public BaseAceObject GetRandomBaseAceObjectByTypeId(uint typeId)
        {
            // TODO: Og II - pick up here
            var criteria = new Dictionary<string, object> { { "typeId", typeId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, BaseAceObject>(WorldPreparedStatement.GetItemsByTypeId, criteria);
            if (objects.Count > 0)
            {
                var rnd = new Random();
                var r = rnd.Next(objects.Count);
                return objects[r];
            }
            return null;
        }

        public List<TeleportLocation> GetLocations()
        {
            var result = SelectPreparedStatement(WorldPreparedStatement.TeleportLocationSelect);
            List<TeleportLocation> locations = new List<TeleportLocation>();

            for (uint i = 0u; i < result.Count; i++)
            {
                locations.Add(new TeleportLocation
                {
                    Location = result.Read<string>(i, "location"),
                    Position = new Position(result.Read<uint>(i, "cell"), result.Read<float>(i, "x"), result.Read<float>(i, "y"),
                        result.Read<float>(i, "z"), result.Read<float>(i, "qx"), result.Read<float>(i, "qy"), result.Read<float>(i, "qz"), result.Read<float>(i, "qw"))
                });
            }

            return locations;
        }

        public AceSubPortalData GetSubPortalDataByWeenie(uint weenieClassId)
        {
            AceSubPortalData spd = new AceSubPortalData();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("weenieClassId", weenieClassId);
            if (ExecuteConstructedGetStatement(WorldPreparedStatement.GetSubPortalDataByWeenie, typeof(AceSubPortalData), criteria, spd))
                return spd;
            else
                return null;
        }

        public AcePortalObject GetPortalObjectsByAceObjectId(uint aceObjectId)
        {
            AcePortalObject apo = new AcePortalObject();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("baseAceObjectId", aceObjectId);
            if (ExecuteConstructedGetStatement(WorldPreparedStatement.GetPortalObjectsByAceObjectId, typeof(AcePortalObject), criteria, apo))
            {
                apo.TextureOverrides = GetAceObjectTextureMaps(apo.AceObjectId);
                apo.AnimationOverrides = GetAceObjectAnimations(apo.AceObjectId);
                apo.PaletteOverrides = GetAceObjectPalettes(apo.AceObjectId);

                return apo;
            }
            else
                return null;
        }

        public List<AceObject> GetObjectsByLandblock(ushort landblock)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("landblock", landblock);
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObject>(WorldPreparedStatement.GetObjectsByLandblock, criteria);
            objects.ForEach(o =>
            {
                o.TextureOverrides = GetAceObjectTextureMaps(o.AceObjectId);
                o.AnimationOverrides = GetAceObjectAnimations(o.AceObjectId);
                o.PaletteOverrides = GetAceObjectPalettes(o.AceObjectId);
            });
            return objects;
        }

        public List<AceCreatureStaticLocation> GetCreaturesByLandblock(ushort landblock)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("landblock", landblock);
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceCreatureStaticLocation>(WorldPreparedStatement.GetCreaturesByLandblock, criteria);
            objects.ForEach(o => o.CreatureData = GetCreatureDataByWeenie(o.WeenieClassId));

            return objects;
        }

        public List<AceCreatureGeneratorLocation> GetCreatureGeneratorsByLandblock(ushort landblock)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("landblock", landblock);
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceCreatureGeneratorLocation>(WorldPreparedStatement.GetCreatureGeneratorByLandblock, criteria);
            objects.ForEach(o => o.CreatureGeneratorData = GetCreatureGeneratorData(o.GeneratorId));

            return objects;
        }

        private List<AceCreatureGeneratorData> GetCreatureGeneratorData(uint generatorId)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("generatorId", generatorId);
            return ExecuteConstructedGetListStatement<WorldPreparedStatement, AceCreatureGeneratorData>(WorldPreparedStatement.GetCreatureGeneratorData, criteria);
        }

        private List<WeeniePaletteOverride> GetWeeniePalettes(uint weenieClassId)
         {
             Dictionary<string, object> criteria = new Dictionary<string, object>();
             criteria.Add("weenieClassId", weenieClassId);
             return ExecuteConstructedGetListStatement<WorldPreparedStatement, WeeniePaletteOverride>(WorldPreparedStatement.GetWeeniePalettes, criteria);
         }

        private List<WeenieTextureMapOverride> GetWeenieTextureMaps(uint weenieClassId)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("weenieClassId", weenieClassId);
            return ExecuteConstructedGetListStatement<WorldPreparedStatement, WeenieTextureMapOverride>(WorldPreparedStatement.GetWeenieTextureMaps, criteria);
        }

        private List<WeenieAnimationOverride> GetWeenieAnimations(uint weenieClassId)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("weenieClassId", weenieClassId);
            return ExecuteConstructedGetListStatement<WorldPreparedStatement, WeenieAnimationOverride>(WorldPreparedStatement.GetWeenieAnimations, criteria);
        }
        private List<TextureMapOverride> GetAceObjectTextureMaps(uint aceObjectId)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("baseAceObjectId", aceObjectId);
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, TextureMapOverride>(WorldPreparedStatement.GetTextureOverridesByObject, criteria);
            return objects;
        }

        private List<PaletteOverride> GetAceObjectPalettes(uint aceObjectId)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("baseAceObjectId", aceObjectId);
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, PaletteOverride>(WorldPreparedStatement.GetPaletteOverridesByObject, criteria);
            return objects;
        }

        private List<AnimationOverride> GetAceObjectAnimations(uint aceObjectId)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("baseAceObjectId", aceObjectId);
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AnimationOverride>(WorldPreparedStatement.GetAnimationOverridesByObject, criteria);
            return objects;
        }

        public AceCreatureObject GetCreatureDataByWeenie(uint weenieClassId)
        {
            AceCreatureObject aco = new AceCreatureObject();
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("weenieClassId", weenieClassId);
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
            var criteria = new Dictionary<string, object> { { "baseAceObjectId", weenieClassId } };
            if (!ExecuteConstructedGetStatement(WorldPreparedStatement.GetWeenieClass, typeof(BaseAceObject), criteria, bao))
                return null;

            foreach (var pal in GetWeeniePalettes(weenieClassId))
            {
                var opal = new PaletteOverride
                             {
                                 AceObjectId = pal.WeenieClassId,
                                 Length = pal.Length,
                                 Offset = pal.Offset,
                                 SubPaletteId = pal.SubPaletteId
                             };
                bao.PaletteOverrides.Add(opal);
            }
            foreach (var tex in GetWeenieTextureMaps(weenieClassId))
            {
                var otex = new TextureMapOverride
                               {
                                   AceObjectId = tex.WeenieClassId,
                                   Index = tex.Index,
                                   NewId = tex.NewId,
                                   OldId = tex.OldId
                               };
                bao.TextureOverrides.Add(otex);
            }
            foreach (var ani in GetWeenieAnimations(weenieClassId))
            {
                var oani = new AnimationOverride
                               {
                                   AceObjectId = ani.WeenieClassId,
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
    }
}
