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
            var stat = new BoolStat
            {
                Key = key,
                Value = value
            };

            using (var context = new ConfigDbContext())
            {
                context.BoolStat.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyBool(BoolStat stat)
        {
            if (!IsActive)
                return;
            using (var context = new ConfigDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public BoolStat GetBool(string key)
        {
            if (!IsActive)
                return null;
            using (var context = new ConfigDbContext())
                return context.BoolStat.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool BoolExists(string key)
        {
            if (!IsActive)
                return false;
            using (var context = new ConfigDbContext())
                return context.BoolStat.Any(r => r.Key == key);
        }

        public void AddInt(string key, int value)
        {
            if (!IsActive)
                return;
            var stat = new IntegerStat
            {
                Key = key,
                Value = value
            };

            using (var context = new ConfigDbContext())
            {
                context.IntegerStat.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyInt(IntegerStat stat)
        {
            if (!IsActive)
                return;
            using (var context = new ConfigDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public IntegerStat GetInt(string key)
        {
            if (!IsActive)
                return null;
            using (var context = new ConfigDbContext())
                return context.IntegerStat.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool IntExists(string key)
        {
            if (!IsActive)
                return false;
            using (var context = new ConfigDbContext())
                return context.IntegerStat.Any(r => r.Key == key);
        }

        public void AddFloat(string key, float value)
        {
            if (!IsActive)
                return;
            var stat = new FloatStat
            {
                Key = key,
                Value = value
            };

            using (var context = new ConfigDbContext())
            {
                context.FloatStat.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyFloat(FloatStat stat)
        {
            if (!IsActive)
                return;
            using (var context = new ConfigDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public FloatStat GetFloat(string key)
        {
            if (!IsActive)
                return null;
            using (var context = new ConfigDbContext())
                return context.FloatStat.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool FloatExists(string key)
        {
            if (!IsActive)
                return false;
            using (var context = new ConfigDbContext())
                return context.FloatStat.Any(r => r.Key == key);
        }

        public void AddString(string key, string value)
        {
            if (!IsActive)
                return;
            var stat = new StringStat
            {
                Key = key,
                Value = value
            };

            using (var context = new ConfigDbContext())
            {
                context.StringStat.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyString(StringStat stat)
        {
            if (!IsActive)
                return;
            using (var context = new ConfigDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public StringStat GetString(string key)
        {
            if (!IsActive)
                return null;
            using (var context = new ConfigDbContext())
                return context.StringStat.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool StringExists(string key)
        {
            if (!IsActive)
                return false;
            using (var context = new ConfigDbContext())
                return context.StringStat.Any(r => r.Key == key);
        }

        public List<BoolStat> GetAllBools()
        {
            if (!IsActive)
                return new List<BoolStat>();
            using (var context = new ConfigDbContext())
                return context.BoolStat.AsNoTracking().ToList();
        }

        public List<IntegerStat> GetAllInts()
        {
            if (!IsActive)
                return new List<IntegerStat>();
            using (var context = new ConfigDbContext())
                return context.IntegerStat.AsNoTracking().ToList();
        }

        public List<FloatStat> GetAllFloats()
        {
            if (!IsActive)
                return new List<FloatStat>();
            using (var context = new ConfigDbContext())
                return context.FloatStat.AsNoTracking().ToList();
        }

        public List<StringStat> GetAllStrings()
        {
            if (!IsActive)
                return new List<StringStat>();
            using (var context = new ConfigDbContext())
                return context.StringStat.AsNoTracking().ToList();
        }
    }
}
