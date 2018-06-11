using ACE.Database.Models.Config;
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
        private static bool IsActive = false;

        public bool Exists(bool retryUntilFound)
        {
            var config = Common.ConfigManager.Config.MySql.Config;

            if (config == null)
            {
                log.Warn("The server property database is not loaded. Please create the server property database.");
                return false;
            }

            for (; ; )
            {
                using (var context = new ConfigDbContext())
                {
                    if (((RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>()).Exists())
                    {
                        log.Debug($"Successfully connected to {config.Database} database on {config.Host}:{config.Port}.");
                        IsActive = true;
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

        public void AddBool(string key, bool value)
        {
            if (!IsActive)
                return;
            var stat = new PropertiesBoolean
            {
                Key = key,
                Value = value
            };

            using (var context = new ConfigDbContext())
            {
                context.PropertiesBoolean.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyBool(PropertiesBoolean stat)
        {
            if (!IsActive)
                return;
            using (var context = new ConfigDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public PropertiesBoolean GetBool(string key)
        {
            if (!IsActive)
                return null;
            using (var context = new ConfigDbContext())
                return context.PropertiesBoolean.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool BoolExists(string key)
        {
            if (!IsActive)
                return false;
            using (var context = new ConfigDbContext())
                return context.PropertiesBoolean.Any(r => r.Key == key);
        }

        public void AddLong(string key, long value)
        {
            if (!IsActive)
                return;
            var stat = new PropertiesLong
            {
                Key = key,
                Value = value
            };

            using (var context = new ConfigDbContext())
            {
                context.PropertiesLong.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyLong(PropertiesLong stat)
        {
            if (!IsActive)
                return;
            using (var context = new ConfigDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public PropertiesLong GetLong(string key)
        {
            if (!IsActive)
                return null;
            using (var context = new ConfigDbContext())
                return context.PropertiesLong.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool LongExists(string key)
        {
            if (!IsActive)
                return false;
            using (var context = new ConfigDbContext())
                return context.PropertiesLong.Any(r => r.Key == key);
        }

        public void AddDouble(string key, double value)
        {
            if (!IsActive)
                return;
            var stat = new PropertiesDouble
            {
                Key = key,
                Value = value
            };

            using (var context = new ConfigDbContext())
            {
                context.PropertiesDouble.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyDouble(PropertiesDouble stat)
        {
            if (!IsActive)
                return;
            using (var context = new ConfigDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public PropertiesDouble GetDouble(string key)
        {
            if (!IsActive)
                return null;
            using (var context = new ConfigDbContext())
                return context.PropertiesDouble.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool DoubleExists(string key)
        {
            if (!IsActive)
                return false;
            using (var context = new ConfigDbContext())
                return context.PropertiesDouble.Any(r => r.Key == key);
        }

        public void AddString(string key, string value)
        {
            if (!IsActive)
                return;
            var stat = new PropertiesString
            {
                Key = key,
                Value = value
            };

            using (var context = new ConfigDbContext())
            {
                context.PropertiesString.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyString(PropertiesString stat)
        {
            if (!IsActive)
                return;
            using (var context = new ConfigDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public PropertiesString GetString(string key)
        {
            if (!IsActive)
                return null;
            using (var context = new ConfigDbContext())
                return context.PropertiesString.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool StringExists(string key)
        {
            if (!IsActive)
                return false;
            using (var context = new ConfigDbContext())
                return context.PropertiesString.Any(r => r.Key == key);
        }

        public List<PropertiesBoolean> GetAllBools()
        {
            if (!IsActive)
                return new List<PropertiesBoolean>();
            using (var context = new ConfigDbContext())
                return context.PropertiesBoolean.AsNoTracking().ToList();
        }

        public List<PropertiesLong> GetAllLongs()
        {
            if (!IsActive)
                return new List<PropertiesLong>();
            using (var context = new ConfigDbContext())
                return context.PropertiesLong.AsNoTracking().ToList();
        }

        public List<PropertiesDouble> GetAllDoubles()
        {
            if (!IsActive)
                return new List<PropertiesDouble>();
            using (var context = new ConfigDbContext())
                return context.PropertiesDouble.AsNoTracking().ToList();
        }

        public List<PropertiesString> GetAllStrings()
        {
            if (!IsActive)
                return new List<PropertiesString>();
            using (var context = new ConfigDbContext())
                return context.PropertiesString.AsNoTracking().ToList();
        }
    }
}
