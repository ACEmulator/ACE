using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Data.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

using log4net;

using ACE.Database.Entity;
using ACE.Database.Models.EventLog;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Database
{
    public class LogDatabase
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool Exists(bool retryUntilFound)
        {
            var config = Common.ConfigManager.Config.MySql.Log;

            for (; ; )
            {
                using (var context = new LogDbContext())
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


        #region Account Session Log

        public void LogAccountSessionStart(uint accountId, string accountName, string sessionIP)
        {
            var logEntry = new AccountSessionLog();

            try
            {
                logEntry.AccountId = accountId;
                logEntry.AccountName = accountName;
                logEntry.SessionIP = sessionIP;
                logEntry.LoginDateTime = DateTime.Now;

                using (var context = new LogDbContext())
                {
                    context.AccountSessions.Add(logEntry);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in LogAccountSessionStart saving session log data to DB. Ex: {ex}");
            }

            return;
        }

        public void LogAccountSessionEnd(uint accountId)
        {
            var logEntry = GetAccountLatestOpenSessionLog(accountId);

            if (logEntry != null)
            {
                try
                {
                    logEntry.LogoutDateTime = DateTime.Now;

                    using (var context = new LogDbContext())
                    {
                        context.Entry(logEntry).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Exception in LogAccountSessionEnd saving session log data to DB. Ex: {ex}");
                }
            }

            return;
        }

        public AccountSessionLog GetAccountLatestOpenSessionLog(uint accountId)
        {
            try
            {
                using (var context = new LogDbContext())
                {
                    return context.AccountSessions
                        .AsNoTracking()
                        .OrderByDescending(r => r.Id)
                        .FirstOrDefault(r => r.AccountId == accountId && !r.LogoutDateTime.HasValue);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in GetLatestOpenSessionLogByAccountId looking for latest open session log for accountId = {accountId}. Ex: {ex}");
                return null;
            }
        }

        #endregion Account Session Log

        #region Character Login Log

        public void LogCharacterLogin(uint accountId, string accountName, string sessionIP, uint characterId, string characterName)
        {
            var logEntry = new CharacterLoginLog();

            try
            {
                logEntry.AccountId = accountId;
                logEntry.AccountName = accountName;
                logEntry.SessionIP = sessionIP;
                logEntry.CharacterId = characterId;
                logEntry.CharacterName = characterName;
                logEntry.LoginDateTime = DateTime.Now;

                using (var context = new LogDbContext())
                {
                    context.CharacterLogins.Add(logEntry);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in LogCharacterLogin saving character login info to DB. Ex: {ex}");
            }
        }


        public void LogCharacterLogout(uint characterId)
        {
            var logEntry = GetLatestOpenCharacterLoginLog(characterId);

            if (logEntry != null)
            {

                try
                {
                    logEntry.LogoutDateTime = DateTime.Now;

                    using (var context = new LogDbContext())
                    {
                        context.Entry(logEntry).State = EntityState.Modified;
                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Exception in LogCharacterLogout saving character logout date to DB. CharacterId = {characterId}, Ex: {ex}");
                }
            }
        }

        public CharacterLoginLog GetLatestOpenCharacterLoginLog(uint characterId)
        {
            try
            {
                using (var context = new LogDbContext())
                {
                    return context.CharacterLogins
                        .AsNoTracking()
                        .OrderByDescending(r => r.Id)
                        .FirstOrDefault(r => r.CharacterId == characterId && !r.LogoutDateTime.HasValue);
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in GetLatestOpenCharacterLoginLog looking for latest open session log for characterId = {characterId}. Ex: {ex}");
                return null;
            }
        }

        #endregion Character Login Log

        #region Tinkering Log

        public void LogTinkeringEvent(uint characterId, string characterName, uint itemBiotaId, float chance, float roll, bool isSuccess, uint itemNumPreviousTinks, uint itemWorkmanship, string salvageType, uint salvageWorkmanship)
        {
            var logEntry = new TinkerLog();

            log.Info($"LogTinkeringEvent itemBiotaId = {itemBiotaId}");

            try
            {
                logEntry.CharacterId = characterId;
                logEntry.CharacterName = characterName;
                logEntry.ItemBiotaId = itemBiotaId;
                logEntry.TinkDateTime = DateTime.Now;
                logEntry.SuccessChance = chance;
                logEntry.Roll = roll;
                logEntry.IsSuccess = isSuccess;
                logEntry.ItemNumPreviousTinks = itemNumPreviousTinks;
                logEntry.ItemWorkmanship = itemWorkmanship;
                logEntry.SalvageType = salvageType;
                logEntry.SalvageWorkmanship = salvageWorkmanship;

                using (var context = new LogDbContext())
                {
                    context.TinkeringEvents.Add(logEntry);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in LogTinkeringEvent saving session log data to DB. Ex: {ex}");
            }

            return;
        }

        #endregion Tinkering Log

        #region PK Kills Log

        public PKKill LogPkKill(uint victimId, uint killerId, uint? victimMonarchId, uint? killerMonarchId)
        {
            var kill = new PKKill();

            try
            {
                kill.VictimId = victimId;
                kill.KillerId = killerId;
                kill.VictimMonarchId = victimMonarchId;
                kill.KillerMonarchId = killerMonarchId;
                kill.KillDateTime = DateTime.Now;

                using (var context = new LogDbContext())
                {
                    context.PKKills.Add(kill);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in LogPkKill saving kill data to DB. Ex: {ex}");
            }

            return kill;
        }

        #endregion PK Kills Log
    }
}
