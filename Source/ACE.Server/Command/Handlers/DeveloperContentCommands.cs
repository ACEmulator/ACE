using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

using ACE.Adapter.Lifestoned;
using ACE.Database;
using ACE.Database.Models.World;
using ACE.Database.SQLFormatters.World;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Factories;
using ACE.Server.Managers;
using ACE.Server.Network;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.WorldObjects;

namespace ACE.Server.Command.Handlers.Processors
{
    public class DeveloperContentCommands
    {
        [CommandHandler("import-json", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Imports a JSON weenie from the Content folder", "<wcid>")]
        public static void HandleImportJson(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var json_folder = $"{di.FullName}{sep}json{sep}weenies{sep}";

            var wcid = parameters[0];
            var prefix = wcid + " - ";

            if (wcid.Equals("all", StringComparison.OrdinalIgnoreCase))
                prefix = "";

            di = new DirectoryInfo(json_folder);

            var files = di.Exists ? di.GetFiles($"{prefix}*.json") : null;

            if (files == null || files.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find {json_folder}{prefix}*.json");
                return;
            }

            var converter = new WeenieSQLWriter();
            converter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();
            converter.SpellNames = DatabaseManager.World.GetAllSpellNames();
            converter.TreasureDeath = DatabaseManager.World.GetAllTreasureDeath();
            converter.TreasureWielded = DatabaseManager.World.GetAllTreasureWielded();

            foreach (var file in files)
                HandleImportJson(session, json_folder, file.Name, converter);
        }

        [CommandHandler("import-sql", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Imports SQL weenie from the Content folder", "<wcid>")]
        public static void HandleImportSQL(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session);
            if (!di.Exists) return;

            var sep = Path.DirectorySeparatorChar;

            var sql_folder = $"{di.FullName}{sep}sql{sep}weenies{sep}";

            var wcid = parameters[0];
            var prefix = wcid + " ";

            if (wcid.Equals("all", StringComparison.OrdinalIgnoreCase))
                prefix = "";

            di = new DirectoryInfo(sql_folder);

            var files = di.Exists ? di.GetFiles($"{prefix}*.sql") : null;

            if (files == null || files.Length == 0)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find {sql_folder}{prefix}*.sql");
                return;
            }

            foreach (var file in files)
                HandleImportSQL(session, sql_folder, file.Name);
        }

        /// <summary>
        /// Returns the absolute content folder path, and verifies it exists
        /// </summary>
        private static DirectoryInfo VerifyContentFolder(Session session, bool showError = true)
        {
            var content_folder = PropertyManager.GetString("content_folder").Item;

            var sep = Path.DirectorySeparatorChar;

            // handle relative path
            if (content_folder.StartsWith("."))
            {
                var cwd = Directory.GetCurrentDirectory() + sep;
                content_folder = cwd + content_folder;
            }

            var di = new DirectoryInfo(content_folder);

            if (!di.Exists && showError)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find content folder: {di.FullName}");
                CommandHandlerHelper.WriteOutputInfo(session, "To set your content folder, /modifystring content_folder <path>");
            }
            return di;
        }

        /// <summary>
        /// Converts JSON to SQL, imports to database, and clears the weenie cache
        /// </summary>
        private static void HandleImportJson(Session session, string json_folder, string json_file, WeenieSQLWriter converter)
        {
            if (!uint.TryParse(Regex.Match(json_file, @"\d+").Value, out var wcid))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find wcid from {json_file}");
                return;
            }

            // convert json -> sql
            var sqlFile = json2sql(session, json_folder, json_file, converter);
            if (sqlFile == null) return;

            // import sql to db
            var sql_folder = json_folder.Replace("json", "sql");
            ImportSQL(sql_folder + sqlFile);
            CommandHandlerHelper.WriteOutputInfo(session, $"Imported {sqlFile}");

            // clear this weenie out of the cache
            DatabaseManager.World.ClearCachedWeenie(wcid);
        }

        /// <summary>
        /// Converts a json file to sql file
        /// </summary>
        public static string json2sql(Session session, string folder, string json_filename, WeenieSQLWriter converter)
        {
            var json_file = folder + json_filename;

            // read json into lsd weenie
            var success = LifestonedLoader.TryLoadWeenie(json_file, out var weenie);

            if (!success)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to load {json_file}");
                return null;
            }

            // convert to ace weenie
            success = LifestonedConverter.TryConvert(weenie, out var output);

            if (!success)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {json_file}");
                return null;
            }

            // output to sql
            var sqlFolder = folder.Replace("json", "sql");

            var di = new DirectoryInfo(sqlFolder);

            if (!di.Exists)
                di.Create();

            var sqlFilename = "";

            try
            {
                if (output.LastModified == DateTime.MinValue)
                    output.LastModified = DateTime.UtcNow;

                sqlFilename = converter.GetDefaultFileName(output);
                var sqlFile = new StreamWriter(sqlFolder + sqlFilename);

                converter.CreateSQLDELETEStatement(output, sqlFile);
                sqlFile.WriteLine();

                converter.CreateSQLINSERTStatement(output, sqlFile);

                var metadata = new Adapter.GDLE.Models.Metadata(weenie);
                if (metadata.HasInfo)
                {
                    var jsonEx = JsonConvert.SerializeObject(metadata, LifestonedConverter.SerializerSettings);
                    sqlFile.WriteLine($"\n/* Lifestoned Changelog:\n{jsonEx}\n*/");
                }

                sqlFile.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {json_file}");
                return null;
            }

            CommandHandlerHelper.WriteOutputInfo(session, $"Converted {json_filename} to {sqlFilename}");

            return sqlFilename;
        }

        /// <summary>
        /// Converts SQL to JSON, imports to database, clears the weenie cache
        /// </summary>
        private static void HandleImportSQL(Session session, string sql_folder, string sql_file)
        {
            if (!uint.TryParse(Regex.Match(sql_file, @"\d+").Value, out var wcid))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find wcid from {sql_file}");
                return;
            }

            // import sql to db
            ImportSQL(sql_folder + sql_file);
            CommandHandlerHelper.WriteOutputInfo(session, $"Imported {sql_file}");

            // clear this weenie out of the cache
            DatabaseManager.World.ClearCachedWeenie(wcid);

            // load weenie from database
            var weenie = DatabaseManager.World.GetCachedWeenie(wcid);

            if (weenie == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't load weenie {wcid} from db");
                return;
            }

            sql2json(session, weenie, sql_folder, sql_file);
        }

        /// <summary>
        /// Converts a sql file to json file
        /// </summary>
        public static bool sql2json(Session session, Weenie weenie, string sql_folder, string sql_filename)
        {
            if (!LifestonedConverter.TryConvertACEWeenieToLSDJSON(weenie, out var json, out var json_weenie))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {sql_filename} to json");
                return false;
            }

            var json_folder = sql_folder.Replace("sql", "json");
            var json_filename = sql_filename.Replace(".sql", ".json");

            var di = new DirectoryInfo(json_folder);

            if (!di.Exists)
                di.Create();

            if (File.Exists(json_folder + json_filename) && LifestonedLoader.AppendMetadata(json_folder + json_filename, json_weenie))
            {
                json = JsonConvert.SerializeObject(json_weenie, LifestonedConverter.SerializerSettings);
            }

            File.WriteAllText(json_folder + json_filename, json);

            CommandHandlerHelper.WriteOutputInfo(session, $"Converted {sql_filename} to {json_filename}");

            return true;
        }

        /// <summary>
        /// Imports an SQL file into the database
        /// </summary>
        public static void ImportSQL(string sqlFile)
        {
            var sqlCommands = File.ReadAllText(sqlFile);

            // not sure why ExecuteSqlCommand doesn't parse this correctly..
            var idx = sqlCommands.IndexOf($"/* Lifestoned Changelog:");
            if (idx != -1)
                sqlCommands = sqlCommands.Substring(0, idx);

            using (var ctx = new WorldDbContext())
                ctx.Database.ExecuteSqlCommand(sqlCommands);
        }

        [CommandHandler("createinst", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Spawns a new wcid or classname as a landblock instance", "<wcid or classname>")]
        public static void HandleCreateInst(Session session, params string[] parameters)
        {
            var loc = new Position(session.Player.Location);

            var param = parameters[0];

            Weenie weenie = null;

            if (uint.TryParse(param, out var wcid))
                weenie = DatabaseManager.World.GetCachedWeenie(wcid);   // wcid
            else
                weenie = DatabaseManager.World.GetCachedWeenie(param);  // classname

            if (weenie == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find weenie {param}", ChatMessageType.Broadcast));
                return;
            }

            var landblock = session.Player.CurrentLandblock.Id.Landblock;
            var nextStaticGuid = GetNextStaticGuid(landblock);

            var wo = WorldObjectFactory.CreateWorldObject(weenie, new ObjectGuid(nextStaticGuid));

            if (wo == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Failed to create new object for {weenie.ClassId} - {weenie.ClassName}", ChatMessageType.Broadcast));
                return;
            }

            if (!wo.Stuck)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"{weenie.ClassId} - {weenie.ClassName} is missing PropertyBool.Stuck, cannot spawn as landblock instance", ChatMessageType.Broadcast));
                return;
            }

            // spawn as ethereal temporarily, to spawn directly on player position
            wo.Ethereal = true;
            wo.Location = new Position(loc);

            // even on flat ground, objects can sometimes fail to spawn at the player's current Z
            // Position.Z has some weird thresholds when moving around, but i guess the same logic doesn't apply when trying to spawn in...
            wo.Location.PositionZ += 0.05f;

            session.Network.EnqueueSend(new GameMessageSystemChat($"Creating new landblock instance @ {loc.ToLOCString()}\n{wo.WeenieClassId} - {wo.Name} ({nextStaticGuid:X8})", ChatMessageType.Broadcast));

            wo.EnterWorld();

            LastStaticGuid[landblock] = wo.Guid.Full;

            // serialize to .sql file
            var contentFolder = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;
            var folder = new DirectoryInfo($"{contentFolder.FullName}{sep}sql{sep}6 LandblockExtendedData{sep}");

            if (!folder.Exists)
                folder.Create();

            var pos = wo.PhysicsObj.Position;
            var origin = pos.Frame.Origin;
            var rotation = pos.Frame.Orientation;

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var insert = $"INSERT INTO `landblock_instance` (`guid`, `weenie_Class_Id`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`, `is_Link_Child`, `last_Modified`)\n" +
                $"VALUES(0x{wo.Guid.Full:X8}, {wo.WeenieClassId}, 0x{wo.PhysicsObj.Position.ObjCellID:X8}, {origin.X}, {origin.Y}, {origin.Z}, {rotation.W}, {rotation.X}, {rotation.Y}, {rotation.Z}, False, '{timestamp}'); /* {wo.Name} */\n" +
                $"/* @teleloc {wo.Location.ToLOCString()} */";

            var sql_filename = $"{landblock:X4}.sql";

            using (var file = File.Open($"{folder.FullName}{sep}{sql_filename}", FileMode.OpenOrCreate))
            {
                file.Seek(0, SeekOrigin.End);
                using (var stream = new StreamWriter(file))
                {
                    if (file.Position > 0)
                        stream.WriteLine();

                    stream.WriteLine(insert);
                }
            }
        }

        public static Dictionary<ushort, uint> LastStaticGuid = new Dictionary<ushort, uint>();

        public static uint GetNextStaticGuid(ushort landblock)
        {
            if (LastStaticGuid.TryGetValue(landblock, out var lastStaticGuid))
                return lastStaticGuid + 1;

            using (var ctx = new WorldDbContext())
            {
                // TODO: eventually find gaps
                var highestLandblockInst = ctx.LandblockInstance.Where(i => i.Landblock == landblock).OrderByDescending(i => i.Guid).FirstOrDefault();

                if (highestLandblockInst == null)
                    return (uint)(0x70000000 | (landblock << 12));
                else
                    return highestLandblockInst.Guid + 1;
            }
        }

        [CommandHandler("addenc", AccessLevel.Developer, CommandHandlerFlag.RequiresWorld, 1, "Spawns a new wcid or classname in the current outdoor cell as an encounter", "<wcid or classname>")]
        public static void HandleAddEncounter(Session session, params string[] parameters)
        {
            var param = parameters[0];

            Weenie weenie = null;

            if (uint.TryParse(param, out var wcid))
                weenie = DatabaseManager.World.GetCachedWeenie(wcid);   // wcid
            else
                weenie = DatabaseManager.World.GetCachedWeenie(param);  // classname

            if (weenie == null)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat($"Couldn't find weenie {param}", ChatMessageType.Broadcast));
                return;
            }

            var pos = session.Player.Location;

            if ((pos.Cell & 0xFFFF) >= 0x100)
            {
                session.Network.EnqueueSend(new GameMessageSystemChat("You must be outdoors to create an encounter!", ChatMessageType.Broadcast));
                return;
            }

            var cellX = (int)pos.PositionX / 24;
            var cellY = (int)pos.PositionY / 24;

            // spawn encounter
            var wo = SpawnEncounter(weenie, cellX, cellY, pos, session);

            if (wo == null) return;

            session.Network.EnqueueSend(new GameMessageSystemChat($"Creating new encounter @ landblock {pos.Landblock:X4}, cellX={cellX}, cellY={cellY}\n{wo.WeenieClassId} - {wo.Name}", ChatMessageType.Broadcast));

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var sql = $"INSERT INTO encounter set landblock=0x{pos.Landblock:X4}, weenie_Class_Id={weenie.ClassId} /* {wo.Name} */, cell_X={cellX}, cell_Y={cellY}, last_Modified='{timestamp}';";

            Console.WriteLine(sql);

            // serialize to .sql file
            var contentFolder = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;
            var folder = new DirectoryInfo($"{contentFolder.FullName}{sep}sql{sep}6 LandblockExtendedData{sep}");

            if (!folder.Exists)
                folder.Create();

            var sql_filename = $"{pos.Landblock:X4}.sql";

            using (var file = File.Open($"{folder.FullName}{sep}{sql_filename}", FileMode.OpenOrCreate))
            {
                file.Seek(0, SeekOrigin.End);

                using (var stream = new StreamWriter(file))
                    stream.WriteLine(sql);
            }
        }

        public static WorldObject SpawnEncounter(Weenie weenie, int cellX, int cellY, Position pos, Session session)
        {
            var wo = WorldObjectFactory.CreateNewWorldObject(weenie.ClassId);

            if (wo == null)
            {
                Console.WriteLine($"Failed to create encounter weenie");
                return null;
            }

            var xPos = Math.Clamp(cellX * 24.0f, 0.5f, 191.5f);
            var yPos = Math.Clamp(cellY * 24.0f, 0.5f, 191.5f);

            var newPos = new Physics.Common.Position();
            newPos.ObjCellID = pos.Cell;
            newPos.Frame = new Physics.Animation.AFrame(new Vector3(xPos, yPos, 0), Quaternion.Identity);
            newPos.adjust_to_outside();

            newPos.Frame.Origin.Z = session.Player.CurrentLandblock.PhysicsLandblock.GetZ(newPos.Frame.Origin);

            wo.Location = new Position(newPos.ObjCellID, newPos.Frame.Origin, newPos.Frame.Orientation);

            var sortCell = Physics.Common.LScape.get_landcell(newPos.ObjCellID) as Physics.Common.SortCell;
            if (sortCell != null && sortCell.has_building())
            {
                Console.WriteLine($"Failed to create encounter near building cell");
                return null;
            }

            if (PropertyManager.GetBool("override_encounter_spawn_rates").Item)
            {
                wo.RegenerationInterval = PropertyManager.GetDouble("encounter_regen_interval").Item;

                wo.ReinitializeHeartbeats();

                foreach (var profile in wo.Biota.BiotaPropertiesGenerator)
                    profile.Delay = (float)PropertyManager.GetDouble("encounter_delay").Item;
            }

            wo.EnterWorld();

            return wo;
        }

        [CommandHandler("export-json", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Exports a weenie from database to JSON file", "<wcid>")]
        public static void HandleExportJson(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;

            if (!uint.TryParse(parameters[0], out var wcid))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"{parameters[0]} not a valid wcid");
                return;
            }

            var weenie = DatabaseManager.World.GetCachedWeenie(wcid);
            if (weenie == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find weenie {wcid}");
                return;
            }

            if (!LifestonedConverter.TryConvertACEWeenieToLSDJSON(weenie, out var json, out var json_weenie))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {weenie.ClassId} - {weenie.ClassName} to json");
                return;
            }

            var json_folder = $"{di.FullName}{sep}json{sep}weenies{sep}";

            di = new DirectoryInfo(json_folder);

            if (!di.Exists)
                di.Create();

            var json_filename = $"{weenie.ClassId} - {weenie.WeeniePropertiesString.FirstOrDefault(i => i.Type == (int)PropertyString.Name)?.Value}.json";

            if (File.Exists(json_folder + json_filename) && LifestonedLoader.AppendMetadata(json_folder + json_filename, json_weenie))
            {
                json = JsonConvert.SerializeObject(json_weenie, LifestonedConverter.SerializerSettings);
            }

            File.WriteAllText(json_folder + json_filename, json);

            CommandHandlerHelper.WriteOutputInfo(session, $"Exported {json_folder}{json_filename}");
        }

        [CommandHandler("export-sql", AccessLevel.Developer, CommandHandlerFlag.None, 1, "Exports a weenie from database to SQL file", "<wcid>")]
        public static void HandleExportSql(Session session, params string[] parameters)
        {
            DirectoryInfo di = VerifyContentFolder(session, false);

            var sep = Path.DirectorySeparatorChar;

            if (!uint.TryParse(parameters[0], out var wcid))
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"{parameters[0]} not a valid wcid");
                return;
            }

            var weenie = DatabaseManager.World.GetCachedWeenie(wcid);
            if (weenie == null)
            {
                CommandHandlerHelper.WriteOutputInfo(session, $"Couldn't find weenie {wcid}");
                return;
            }

            var sql_folder = $"{di.FullName}{sep}sql{sep}weenies{sep}";

            di = new DirectoryInfo(sql_folder);

            if (!di.Exists)
                di.Create();

            var converter = new WeenieSQLWriter();

            converter.WeenieNames = DatabaseManager.World.GetAllWeenieNames();
            converter.SpellNames = DatabaseManager.World.GetAllSpellNames();
            converter.TreasureDeath = DatabaseManager.World.GetAllTreasureDeath();
            converter.TreasureWielded = DatabaseManager.World.GetAllTreasureWielded();

            var sql_filename = converter.GetDefaultFileName(weenie);

            var writer = new StreamWriter(sql_folder + sql_filename);

            try
            {
                converter.CreateSQLDELETEStatement(weenie, writer);
                writer.WriteLine();
                converter.CreateSQLINSERTStatement(weenie, writer);
                writer.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                CommandHandlerHelper.WriteOutputInfo(session, $"Failed to convert {weenie.ClassId} - {weenie.ClassName}");
                return;
            }

            CommandHandlerHelper.WriteOutputInfo(session, $"Exported {sql_folder}{sql_filename}");
        }

        [CommandHandler("clearcache", AccessLevel.Developer, CommandHandlerFlag.None, "Clears the various database caches. This enables live editing of the database information")]
        public static void HandleClearCache(Session session, params string[] parameters)
        {
            var mode = CacheType.All;
            if (parameters.Length > 0)
            {
                if (parameters[0].Contains("weenie", StringComparison.OrdinalIgnoreCase))
                    mode = CacheType.Weenie;
                if (parameters[0].Contains("spell", StringComparison.OrdinalIgnoreCase))
                    mode = CacheType.Spell;
            }

            if (mode.HasFlag(CacheType.Weenie))
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Clearing weenie cache");
                DatabaseManager.World.ClearWeenieCache();
            }

            if (mode.HasFlag(CacheType.Spell))
            {
                CommandHandlerHelper.WriteOutputInfo(session, "Clearing spell cache");
                DatabaseManager.World.ClearSpellCache();
                WorldObject.ClearSpellCache();
            }
        }

        [Flags]
        public enum CacheType
        {
            None   = 0x0,
            Weenie = 0x1,
            Spell  = 0x2,
            All    = 0xFFFF
        };
    }
}
