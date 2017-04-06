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
            GetWeenieAnimations
        }

        protected override Type PreparedStatementType => typeof(WorldPreparedStatement);

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(WorldPreparedStatement.TeleportLocationSelect, "SELECT `location`, `cell`, `x`, `y`, `z`, `qx`, `qy`, `qz`, `qw` FROM `teleport_location`;");
            // ConstructStatement(WorldPreparedStatement.GetWeenieClass, typeof(BaseAceObject), ConstructedStatementType.Get);
            ConstructStatement(WorldPreparedStatement.GetObjectsByLandblock, typeof(AceObject), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetCreaturesByLandblock, typeof(AceCreatureStaticLocation), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetWeeniePalettes, typeof(WeeniePaletteOverride), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetWeenieTextureMaps, typeof(WeenieTextureMapOverride), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetWeenieAnimations, typeof(WeenieAnimationOverride), ConstructedStatementType.GetList);
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

        public List<AceObject> GetObjectsByLandblock(ushort landblock)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("landblock", landblock);
            return ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObject>(WorldPreparedStatement.GetObjectsByLandblock, criteria);
        }

        public List<AceCreatureStaticLocation> GetCreaturesByLandblock(ushort landblock)
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            criteria.Add("landblock", landblock);
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceCreatureStaticLocation>(WorldPreparedStatement.GetCreaturesByLandblock, criteria);
            objects.ForEach(o =>
            {
                o.WeeniePaletteOverrides = GetWeeniePalettes(o.WeenieClassId);
                o.WeenieTextureMapOverrides = GetWeenieTextureMaps(o.WeenieClassId);
                o.WeenieAnimationOverrides = GetWeenieAnimations(o.WeenieClassId);
            });

            return objects;
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
    }
}
