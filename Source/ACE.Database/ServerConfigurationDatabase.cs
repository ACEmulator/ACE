using ACE.Database.Models.Config;
using log4net;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using System.Threading;

namespace ACE.Database
{
    class ServerConfigurationDatabase
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


    }
}
