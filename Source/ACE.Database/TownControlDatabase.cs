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

        public List<Town> GetAllTowns()
        {
            var townList = new List<Town>();

            try
            {
                using (var context = new TownControlDbContext())
                {
                    if (context != null && context.Town != null)
                    {
                        townList = context.Town.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("GetAllTowns hit an exception. Ex: {0}", ex);
                throw;
            }

            return townList;
        }

        public Town GetTownById(uint townId)
        {
            try
            {
                using (var context = new TownControlDbContext())
                {
                    Town town = context.Town.Where(x => x.TownId == townId).FirstOrDefault();

                    return town;
                }
            }
            catch(Exception ex)
            {
                log.ErrorFormat("GetTownById hit an exception. Ex: {0}", ex);
                throw;
            }

            return null;
        }

        public void UpdateTown(Town town)
        {
            try
            {
                using (var context = new TownControlDbContext())
                {
                    var townDbRec = context.Town.FirstOrDefault(x => x.TownId == town.TownId);

                    if(townDbRec == null)
                    {
                        return;
                    }

                    townDbRec.IsInConflict = town.IsInConflict;
                    townDbRec.LastConflictStartDateTime = town.LastConflictStartDateTime;
                    townDbRec.CurrentOwnerID = town.CurrentOwnerID;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("UpdateTown hit an exception. Ex: {0}", ex);
                throw;
            }
        }


        public TownControlEvent StartTownControlEvent(uint townId, uint attackingClanId, string attackingClanName, uint? defendingClanId, string defendingClanName)
        {
            try
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
            catch(Exception ex)
            {
                log.ErrorFormat("StartTownControlEvent hit an exception. Ex: {0}", ex);
                throw;
            }

            return null;
        }

        public void UpdateTownControlEvent(TownControlEvent tcEvent)
        {
            try
            {
                using (var context = new TownControlDbContext())
                {
                    var eventDbRec = context.TownControlEvent.FirstOrDefault(x => x.EventId == tcEvent.EventId);

                    if (eventDbRec == null)
                    {
                        return;
                    }

                    eventDbRec.AttackingClanId = tcEvent.AttackingClanId;
                    eventDbRec.AttackingClanName = tcEvent.AttackingClanName;
                    eventDbRec.DefendingClanId = tcEvent.DefendingClanId;
                    eventDbRec.EventStartDateTime = tcEvent.EventStartDateTime;
                    eventDbRec.EventEndDateTime = tcEvent.EventEndDateTime;
                    eventDbRec.IsAttackSuccess = tcEvent.IsAttackSuccess;

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("UpdateTownControlEvent hit an exception. Ex: {0}", ex);
                throw;
            }
        }

        public TownControlEvent GetLatestTownControlEventByTownId(uint townId)
        {
            try
            {
                using (var context = new TownControlDbContext())
                {
                    var townEvents = context.TownControlEvent
                        .AsNoTracking()
                        .Where(r => r.TownId == townId);

                    if (townEvents != null && townEvents.Count() > 0)
                    {
                        return townEvents.ToList().OrderByDescending(x => x.EventId).FirstOrDefault();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch(Exception ex)
            {
                log.ErrorFormat("GetLatestTownControlEventByTownId hit an exception. Ex: {0}", ex);
                throw;
            }

            return null;
        }


        public TownControlEvent GetLatestTownControlEventByAttackingMonarchId(uint attackingMonarchId, uint townId)
        {
            try
            {
                using (var context = new TownControlDbContext())
                {
                    var townEvents = context.TownControlEvent
                        .AsNoTracking()
                        .Where(r => r.TownId == townId && r.AttackingClanId == attackingMonarchId);

                    if (townEvents != null && townEvents.Count() > 0)
                    {
                        return townEvents.ToList().OrderByDescending(x => x.EventId).FirstOrDefault();
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                log.ErrorFormat("GetLatestTownControlEventByAttackingMonarchId hit an exception. Ex: {0}", ex);
                throw;
            }

            return null;
        }
    }
}
