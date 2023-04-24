using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using log4net;

using ACE.Database.Entity;
using ACE.Database.Models.PKKills;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database
{
    public class PKKillsDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool Exists(bool retryUntilFound)
        {
            var config = Common.ConfigManager.Config.MySql.PKKills;

            for (; ; )
            {
                using (var context = new PKKillsDbContext())
                {
                    if (((RelationalDatabaseCreator)context.Database.GetService<IDatabaseCreator>()).Exists())
                    {
                        log.Debug($"[DATABASE] Successfully connected to {config.Database} database on {config.Host}:{config.Port}.");
                        return true;
                    }
                }

                log.Error($"[DATABASE] Attempting to reconnect to {config.Database} database on {config.Host}:{config.Port} in 5 seconds...");

                if (retryUntilFound)
                    Thread.Sleep(5000);
                else
                    return false;
            }
        }
        //public void GetCharacter(uint characterId, Action<Character> callback)
        //{
        //    _queue.Add(new Task(() =>
        //    {
        //        var result = BaseDatabase.GetCharacter(characterId);
        //        callback?.Invoke(result);
        //    }));
        //}

        public Kills CreateKill(uint victimId, uint killerId, bool IsInArena=false, string ArenaType="1v1")
        {
            var kill = new Kills();
            kill.VictimId = victimId;
            kill.KillerId = killerId;
            kill.KillType = !IsInArena ? "GLOBAL" : $"ARENA:{ArenaType}";
            //            account.CreateTime = DateTime.UtcNow;
            using (var context = new PKKillsDbContext())
            {
                context.Kills.Add(kill);
                context.SaveChanges();
            }

            return kill;
        }

    }
}
