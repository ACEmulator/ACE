using ACE.Database.Models.Shard;
using log4net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ACE.Database
{
    public class ServerPropertyDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool Exists(bool retryUntilFound)
        {
            var config = Common.ConfigManager.Config.MySql.Shard;

            for (; ; )
            {
                using (var context = new ShardDbContext())
                {
                    if (((RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>()).Exists())
                    {
                        log.Debug($"Successfully connected to {config.Database} database on {config.Host}:{config.Port}.");
                        return true;
                    }
                }

                log.Error($"Attempting to reconnect to {config.Database} database on {config.Host}:{config.Port} in 5 seconds...");

                if (retryUntilFound)
                    Thread.Sleep(5000);
                else
                    return false;
            }
        }

        public void AddBool(string key, bool value, string description = "")
        {
            var stat = new ConfigPropertiesBoolean
            {
                Key = key,
                Value = value,
                Description = description
            };

            using (var context = new ShardDbContext())
            {
                context.ConfigPropertiesBoolean.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyBool(ConfigPropertiesBoolean stat)
        {
            using (var context = new ShardDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public ConfigPropertiesBoolean GetBool(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesBoolean.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool BoolExists(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesBoolean.Any(r => r.Key == key);
        }

        public void AddLong(string key, long value, string description = "")
        {
            var stat = new ConfigPropertiesLong
            {
                Key = key,
                Value = value,
                Description = description
            };

            using (var context = new ShardDbContext())
            {
                context.ConfigPropertiesLong.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyLong(ConfigPropertiesLong stat)
        {
            using (var context = new ShardDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public ConfigPropertiesLong GetLong(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesLong.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool LongExists(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesLong.Any(r => r.Key == key);
        }

        public void AddDouble(string key, double value, string description = "")
        {
            var stat = new ConfigPropertiesDouble
            {
                Key = key,
                Value = value,
                Description = description
            };

            using (var context = new ShardDbContext())
            {
                context.ConfigPropertiesDouble.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyDouble(ConfigPropertiesDouble stat)
        {
            using (var context = new ShardDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public ConfigPropertiesDouble GetDouble(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesDouble.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool DoubleExists(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesDouble.Any(r => r.Key == key);
        }

        public void AddString(string key, string value, string description = "")
        {
            var stat = new ConfigPropertiesString
            {
                Key = key,
                Value = value,
                Description = description
            };

            using (var context = new ShardDbContext())
            {
                context.ConfigPropertiesString.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyString(ConfigPropertiesString stat)
        {
            using (var context = new ShardDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public ConfigPropertiesString GetString(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesString.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool StringExists(string key)
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesString.Any(r => r.Key == key);
        }

        public List<ConfigPropertiesBoolean> GetAllBools()
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesBoolean.AsNoTracking().ToList();
        }

        public List<ConfigPropertiesLong> GetAllLongs()
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesLong.AsNoTracking().ToList();
        }

        public List<ConfigPropertiesDouble> GetAllDoubles()
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesDouble.AsNoTracking().ToList();
        }

        public List<ConfigPropertiesString> GetAllStrings()
        {
            using (var context = new ShardDbContext())
                return context.ConfigPropertiesString.AsNoTracking().ToList();
        }
    }
}
