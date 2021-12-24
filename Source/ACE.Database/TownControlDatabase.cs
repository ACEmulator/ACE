using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using log4net;

using ACE.Database.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Database.Models.TownControl;

namespace ACE.Database
{
    public class TownControlDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool Exists(bool retryUntilFound)
        {
            var config = Common.ConfigManager.Config.MySql.TownControl;

            for (; ; )
            {
                using (var context = new TownControlDbContext())
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

        public Town GetTownById(uint townId)
        {
            using (var context = new TownControlDbContext())
            {
                Town town = context.Town.Where(x => x.TownId == townId).FirstOrDefault();

                return town;
            }
        }


        public TownControlEvent StartTownControlEvent(uint townId, uint attackingClanId, string attackingClanName, uint defendingClanId, string defendingClanName)
        {
            var tcEvent = new TownControlEvent()
            {
                EventStartDateTime = DateTime.UtcNow,
                EventEndDateTime = null,
                TownId = townId,
                AttackingClanId = attackingClanId,
                AttackingClanName = attackingClanName,
                DefendingClanId = defendingClanId,
                DefendingClanName = defendingClanName,
                IsAttackSuccess = null
            };

            using (var context = new TownControlDbContext())
            {
                context.TownControlEvent.Add(tcEvent);
                context.SaveChanges();
            }

            return tcEvent;
        }

        public TownControlEvent GetLatestTownControlEventByTownId(uint townId)
        {
            using (var context = new TownControlDbContext())
            {
                var townEvents = context.TownControlEvent
                    .AsNoTracking()
                    .Where(r => r.TownId == townId);

                if(townEvents != null && townEvents.Count() > 0)
                {
                    return townEvents.ToList().OrderByDescending(x => x.EventId).FirstOrDefault();
                }
                else
                {
                    return null;
                }
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

        //public Kills CreateKill(uint victimId, uint killerId)
        //{
        //    var kill = new Kills();
        //    kill.VictimId = victimId;
        //    kill.KillerId = killerId;
        //    //            account.CreateTime = DateTime.UtcNow;
        //    using (var context = new PKKillsDbContext())
        //    {
        //        context.Kills.Add(kill);
        //        context.SaveChanges();
        //    }

        //    return kill;
        //}

    }
}
