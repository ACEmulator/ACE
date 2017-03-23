using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

using ACE.Entity;

namespace ACE.Database
{

    public class WorldDatabase : Database, IWorldDatabase
    {
        private enum WorldPreparedStatement
        {
            CreatureDataSelectByName,
            ModelDataSelectById,
            PaletteDataSelectById,
            TextureDataSelectById,
            TeleportLocationSelect
        }

        protected override Type preparedStatementType => typeof(WorldPreparedStatement);

        protected override void InitialisePreparedStatements()
        {
            AddPreparedStatement(WorldPreparedStatement.TeleportLocationSelect, "SELECT `location`, `cell`, `x`, `y`, `z`, `qx`, `qy`, `qz`, `qw` FROM `teleport_location`;");
            AddPreparedStatement(WorldPreparedStatement.CreatureDataSelectByName, "SELECT `id`, `wcid`, `name`, `iconid`, `setupid`, `phstableid`, `stableid`, `itemscapacity`, `objectdescription`, `physicsstate`, `containerscapacity`, `paletteid` FROM `object_data` WHERE `type` = 16 AND `name` = ?;", MySqlDbType.Text);
            AddPreparedStatement(WorldPreparedStatement.ModelDataSelectById, "SELECT `id`, `index`, `resourceid` FROM `model_data` WHERE `id` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(WorldPreparedStatement.PaletteDataSelectById, "SELECT `id`, `paletteid`, `offset`, `length` FROM `palette_data` WHERE `id` = ?;", MySqlDbType.UInt32);
            AddPreparedStatement(WorldPreparedStatement.TextureDataSelectById, "SELECT `id`, `index`, `oldid`, `newid` FROM `texture_data` WHERE `id` = ?;", MySqlDbType.UInt32);

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

        public Creature GetCreatureByName(string name)
        {
            var result = SelectPreparedStatement(WorldPreparedStatement.CreatureDataSelectByName, name);
            Creature cr = new Creature();

            if (result.Count > 0)
            {
                uint i = 0;
                cr.Id = result.Read<uint>(i, "id");
                cr.Wcid = result.Read<uint>(i, "wcid");
                cr.Name = result.Read<string>(i, "name");
                cr.IconId = result.Read<uint>(i, "iconid");
                cr.SetupId = result.Read<uint>(i, "setupid");
                cr.PhsTableId = result.Read<uint>(i, "phstableid");
                cr.STableId = result.Read<uint>(i, "stableid");
                cr.ItemsCapacity = result.Read<byte>(i, "itemscapacity");
                cr.ObjectDescription = result.Read<uint>(i, "objectdescription");
                cr.PhysicsState = result.Read<uint>(i, "physicsstate");
                cr.ContainersCapacity = result.Read<uint>(i, "containerscapacity");
                cr.PaletteId = result.Read<uint>(i, "paletteid");

                if (result.Count > 1)
                {
                    // Error message could be more user friendly, but this should do for now
                    Console.WriteLine("Found more than one creature with the name " + name + "using the first one");
                }

            }
            else return null;

            result = null;
            result = SelectPreparedStatement(WorldPreparedStatement.ModelDataSelectById, cr.Id);
            
            for (uint i = 0u; i < result.Count; i++)
            {
                cr.Model.Add(new ObjectModel
                {
                    Index = result.Read<byte>(i, "index"),
                    ResourceId = result.Read<uint>(i, "resourceid")
                });
            }

            result = null;
            result = SelectPreparedStatement(WorldPreparedStatement.PaletteDataSelectById, cr.Id);

            for (uint i = 0u; i < result.Count; i++)
            {
                cr.Palette.Add(new ObjectPalette
                {
                    PaletteID = result.Read<uint>(i, "paletteid"),
                    Offset = result.Read<byte>(i, "offset"),
                    Length = result.Read<ushort>(i, "length")
                });
            }

            result = null;
            result = SelectPreparedStatement(WorldPreparedStatement.TextureDataSelectById, cr.Id);

            for (uint i = 0u; i < result.Count; i++)
            {
                cr.Texture.Add(new ObjectTexture
                {
                    Index = result.Read<byte>(i, "index"),
                    OldTexture = result.Read<uint>(i, "oldid"),
                    NewTexture = result.Read<uint>(i, "newid")
                });
            }

            // TODO: Read the creature stats from DB creature_data

            return cr;
        }
    }
}
