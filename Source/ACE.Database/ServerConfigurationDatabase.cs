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
    public class ServerConfigurationDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool Exists(bool retryUntilFound)
        {
            var config = Common.ConfigManager.Config.MySql.Config;

            for (; ; )
            {
                using (var context = new ConfigDbContext())
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

        public void AddBool(string key, bool value)
        {

            var stat = new BoolStat();
            stat.Key = key;
            stat.Value = value;

            using (var context = new ConfigDbContext())
            {
                context.BoolStat.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyBool(BoolStat stat)
        {
            using (var context = new ConfigDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public BoolStat GetBool(string key)
        {
            using (var context = new ConfigDbContext())
                return context.BoolStat.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool BoolExists(string key)
        {
            using (var context = new ConfigDbContext())
                return context.BoolStat.Any(r => r.Key == key);
        }

        public void AddInt(string key, int value)
        {

            var stat = new IntegerStat();
            stat.Key = key;
            stat.Value = value;

            using (var context = new ConfigDbContext())
            {
                context.IntegerStat.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyInt(IntegerStat stat)
        {
            using (var context = new ConfigDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public IntegerStat GetInt(string key)
        {
            using (var context = new ConfigDbContext())
                return context.IntegerStat.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool IntExists(string key)
        {
            using (var context = new ConfigDbContext())
                return context.IntegerStat.Any(r => r.Key == key);
        }

        public void AddFloat(string key, float value)
        {

            var stat = new FloatStat();
            stat.Key = key;
            stat.Value = value;

            using (var context = new ConfigDbContext())
            {
                context.FloatStat.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyFloat(FloatStat stat)
        {
            using (var context = new ConfigDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public FloatStat GetFloat(string key)
        {
            using (var context = new ConfigDbContext())
                return context.FloatStat.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool FloatExists(string key)
        {
            using (var context = new ConfigDbContext())
                return context.FloatStat.Any(r => r.Key == key);
        }

        public void AddString(string key, string value)
        {

            var stat = new StringStat();
            stat.Key = key;
            stat.Value = value;

            using (var context = new ConfigDbContext())
            {
                context.StringStat.Add(stat);

                context.SaveChanges();
            }
        }

        public void ModifyString(StringStat stat)
        {
            using (var context = new ConfigDbContext())
            {
                context.Entry(stat).State = EntityState.Modified;

                context.SaveChanges();
            }
        }

        public StringStat GetString(string key)
        {
            using (var context = new ConfigDbContext())
                return context.StringStat.AsNoTracking().FirstOrDefault(r => r.Key == key);
        }

        public bool StringExists(string key)
        {
            using (var context = new ConfigDbContext())
                return context.StringStat.Any(r => r.Key == key);
        }

        public List<BoolStat> GetAllBools()
        {
            using (var context = new ConfigDbContext())
                return context.BoolStat.AsNoTracking().ToList();
        }

        public List<IntegerStat> GetAllInts()
        {
            using (var context = new ConfigDbContext())
                return context.IntegerStat.AsNoTracking().ToList();
        }

        public List<FloatStat> GetAllFloats()
        {
            using (var context = new ConfigDbContext())
                return context.FloatStat.AsNoTracking().ToList();
        }

        public List<StringStat> GetAllStrings()
        {
            using (var context = new ConfigDbContext())
                return context.StringStat.AsNoTracking().ToList();
        }


    }
}
