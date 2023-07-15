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
using ACE.Database.Models.Log;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Database.Models.Shard;
using ACE.Database.Models.Auth;
using System.Net;
using Microsoft.Extensions.Logging.Abstractions;

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

        public PKKill LogPkKill(uint victimId, uint killerId, uint? victimMonarchId, uint? killerMonarchId, uint? victimArenaPlayerId = null, uint? killerArenaPlayerId = null)
        {
            var kill = new PKKill();

            try
            {
                kill.VictimId = victimId;
                kill.KillerId = killerId;
                kill.VictimMonarchId = victimMonarchId;
                kill.KillerMonarchId = killerMonarchId;
                kill.KillDateTime = DateTime.Now;
                kill.VictimArenaPlayerID = victimArenaPlayerId;
                kill.KillerArenaPlayerID = killerArenaPlayerId;

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

        #region Arenas   

        public uint SaveArenaEvent(ArenaEvent arenaEvent)
        {
            try
            {
                using (var context = new LogDbContext())
                {
                    if (arenaEvent.Id <= 0)
                    {
                        context.ArenaEvents.Add(arenaEvent);                        
                    }
                    else
                    {
                        context.Entry(arenaEvent).State = EntityState.Modified;
                    }

                    context.SaveChanges();

                    foreach (var arenaPlayer in arenaEvent.Players)
                    {
                        arenaPlayer.EventId = arenaEvent.Id;

                        if (arenaPlayer.Id <= 0)
                        {
                            context.ArenaPlayers.Add(arenaPlayer);
                        }
                        else
                        {
                            context.Entry(arenaPlayer).State = EntityState.Modified;                            
                        }                        
                    }

                    context.SaveChanges();

                    return arenaEvent.Id;
                }
            }
            catch(Exception ex)
            {
                log.Error($"Exception in SaveArenaEvent. Ex: {ex}");
            }

            return 0;
        }

        public void AddToArenaStats(uint characterId, string characterName, uint totalMatches, uint totalWins, uint totalDraws, uint totalLosses, uint totalDisqualified, uint totalDeaths, uint totalKills, uint totalDmgDealt, uint totalDmgReceived)
        {
            try
            {
                using (var context = new LogDbContext())
                {
                    var stats = context.ArenaCharacterStats.FirstOrDefault(x => x.CharacterId == characterId);
                    if(stats == null)
                    {
                        stats = new ArenaCharacterStats();
                        stats.CharacterId = characterId;
                        stats.CharacterName = characterName;
                        context.ArenaCharacterStats.Add(stats);
                    }
                    else
                    {
                        context.Entry(stats).State = EntityState.Modified;
                    }

                    stats.TotalMatches += totalMatches;
                    stats.TotalWins += totalWins;
                    stats.TotalDraws += totalDraws;
                    stats.TotalLosses += totalLosses;
                    stats.TotalDisqualified += totalDisqualified;
                    stats.TotalDeaths += totalDeaths;
                    stats.TotalKills += totalKills;
                    stats.TotalDmgDealt += totalDmgDealt;
                    stats.TotalDmgReceived += totalDmgReceived;
                    stats.RankPoints = stats.GetRankPoints();

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception saving ArenaCharacterStats. ex: {ex}");
            }
        }

        public string GetArenaStatsByCharacterId(uint characterId, string characterName)
        {
            string returnMsg = "";

            try
            {
                using (var context = new LogDbContext())
                {

                    var wonEvents = (from a in context.ArenaEvents
                                     join c in context.ArenaPlayers on a.Id equals c.EventId
                                     where c.CharacterId == characterId
                                     && c.TeamGuid == a.WinningTeamGuid
                                     select a);

                    var lostEvents = (from a in context.ArenaEvents
                                     join c in context.ArenaPlayers on a.Id equals c.EventId
                                     where c.CharacterId == characterId
                                     && a.WinningTeamGuid.HasValue
                                     && c.TeamGuid != a.WinningTeamGuid
                                     select a);

                    var drawEvents = (from a in context.ArenaEvents
                                      join c in context.ArenaPlayers on a.Id equals c.EventId
                                      where c.CharacterId == characterId
                                      && !a.WinningTeamGuid.HasValue                                      
                                      select a);

                    var onesWins = wonEvents.Where(x => x.EventType.ToLower().Equals("1v1"));
                    var twosWins = wonEvents.Where(x => x.EventType.ToLower().Equals("2v2"));
                    var ffaWins = wonEvents.Where(x => x.EventType.ToLower().Equals("ffa"));
                    var onesDraws = drawEvents.Where(x => x.EventType.ToLower().Equals("1v1"));
                    var twosDraws = drawEvents.Where(x => x.EventType.ToLower().Equals("2v2"));
                    var ffaDraws = drawEvents.Where(x => x.EventType.ToLower().Equals("ffa"));
                    var onesLosses = lostEvents.Where(x => x.EventType.ToLower().Equals("1v1"));
                    var twosLosses = lostEvents.Where(x => x.EventType.ToLower().Equals("2v2"));
                    var ffaLosses = lostEvents.Where(x => x.EventType.ToLower().Equals("ffa"));

                    var stats = context.ArenaCharacterStats.FirstOrDefault(x => x.CharacterId == characterId);
                    if(stats != null)
                    {

                    }
                    else
                    {
                        stats = new ArenaCharacterStats();
                    }

                    returnMsg = $"*********\nArena Stats for {characterName}\n  Arena Rank: {DatabaseManager.Log.GetArenaRank(stats.RankPoints)}\n  Rank Points: {stats.RankPoints}\n  Total Arena Matches Played: {stats.TotalMatches.ToString("n0")}\n  Total Arena Wins: {stats.TotalWins.ToString("n0")}\n  Total Arena Losses: {stats.TotalLosses.ToString("n0")}\n\n  1v1 Wins: {onesWins.Count().ToString("n0")}\n  1v1 Draws: {onesDraws.Count().ToString("n0")}\n  1v1 Losses: {onesLosses.Count().ToString("n0")}\n  2v2 Wins: {twosWins.Count().ToString("n0")}\n  2v2 Draws: {twosDraws.Count().ToString("n0")}\n  2v2 Losses: {twosLosses.Count().ToString("n0")}\n  FFA Wins: {ffaWins.Count().ToString("n0")}\n  FFA Draws: {ffaDraws.Count().ToString("n0")}\n  FFA Losses: {ffaLosses.Count().ToString("n0")}\n\n  Total Kills: {stats.TotalKills.ToString("n0")}\n  Total Deaths: {stats.TotalDeaths.ToString("n0")}\n  Total Damage Dealt: {stats.TotalDmgDealt.ToString("n0")}\n  Total Damage Received: {stats.TotalDmgReceived.ToString("n0")}\n*********\n";
                }
            }
            catch(Exception ex)
            {
                log.Error($"Exception in GetEventStatsByCharacterId for characterId = {characterId}. ex: {ex}");
            }

            return returnMsg;
        }

        public int GetArenaRank(uint rankPoints)
        {
            try
            {
                using (var context = new LogDbContext())
                {
                    var higherPlayers = context.ArenaCharacterStats.Where(x => x.RankPoints > rankPoints);
                    if(higherPlayers != null)
                    {
                        return higherPlayers.Count() + 1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error($"Error in GetArenaRank. ex:{ex}");
            }

            return -1;
        }

        public List<ArenaEvent> GetAllActiveEvents()
        {
            try
            {
                using (var context = new LogDbContext())
                {
                    var result = context.ArenaEvents
                            .AsNoTracking()
                            .OrderByDescending(r => r.StartDateTime)
                            .Where(r => !r.EndDateTime.HasValue);

                    if (result != null)
                    {
                        return result.ToList();
                    }
                }
            }
            catch(Exception ex)
            {
                log.Error($"Exception in GetAllActiveEvents. ex: {ex}");
            }

            return new List<ArenaEvent>();
        }

        public uint CreateArenaPlayer(ArenaPlayer player)
        {
            try
            {
                using (var context = new LogDbContext())
                {
                    context.ArenaPlayers.Add(player);
                    context.SaveChanges();

                    return player.Id;
                }
            }
            catch (Exception ex)
            {
                log.Error($"Exception in CreateArenaEvent. Ex: {ex}");
            }

            return 0;
        }

        public void UpdateArenaPlayer(ArenaPlayer player)
        {
            using (var context = new LogDbContext())
            {
                context.Entry(player).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        #endregion Arenas

        //public bool ExampleCustomSql()
        //{            
        //    var sql = @$"";
        //    using (var context = new ShardDbContext())
        //    {
        //        context.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));
        //        var connection = context.Database.GetDbConnection();
        //        connection.Open();
        //        var command = connection.CreateCommand();
        //        command.CommandText = sql;
        //        var reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            
        //        }
        //    }
        //}        
    }
}
