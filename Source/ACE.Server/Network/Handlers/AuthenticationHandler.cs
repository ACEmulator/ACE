using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using log4net;

using ACE.Common;
using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.Database.Models.Shard;
using ACE.Entity.Enum;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Packets;
using System.Collections.ObjectModel;
using System.Threading;
using ACE.Server.Factories;

namespace ACE.Server.Network.Handlers
{
    public static class AuthenticationHandler
    {
        /// <summary>
        /// Seconds until an authentication request will timeout/expire.
        /// </summary>
        public const int DefaultAuthTimeout = 15;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly ILog packetLog = LogManager.GetLogger(System.Reflection.Assembly.GetEntryAssembly(), "Packets");

        public static void HandleLoginRequest(ClientPacket packet, Session session)
        {
            PacketInboundLoginRequest loginRequest = null;
            try
            {
                loginRequest = new PacketInboundLoginRequest(packet);
            }
            catch (Exception) { }
            Task t = new Task(() => DoLogin(session, loginRequest));
            t.Start();
        }
        private static void DoLogin(Session session, PacketInboundLoginRequest loginRequest)
        {
            var account = DatabaseManager.Authentication.GetAccountByName(loginRequest.Account);
            if (account == null)
            {
                if (loginRequest.NetAuthType == NetAuthType.AccountPassword && loginRequest.Password != "")
                {
                    if (ConfigManager.Config.Server.Accounts.AllowAutoAccountCreation)
                    {
                        // no account, dynamically create one
                        log.Info($"Auto creating account for: {loginRequest.Account}");
                        var accessLevel = (AccessLevel)ConfigManager.Config.Server.Accounts.DefaultAccessLevel;
                        if (!System.Enum.IsDefined(typeof(AccessLevel), accessLevel))
                            accessLevel = AccessLevel.Player;
                        account = DatabaseManager.Authentication.CreateAccount(loginRequest.Account.ToLower(), loginRequest.Password, accessLevel);
                    }
                }
            }
            try
            {
                log.Debug($"new client connected: {loginRequest.Account}. setting session properties");
                AccountSelectCallback(account, session, loginRequest);
            }
            catch (Exception ex)
            {
                log.Error("Error in HandleLoginRequest trying to find the account.", ex);
                session.Terminate(SessionTerminationReason.AccountSelectCallbackException);
            }
        }
        private static void AccountSelectCallback(Account account, Session session, PacketInboundLoginRequest loginRequest)
        {
            packetLog.DebugFormat("ConnectRequest TS: {0}", session.ServerTime);
            if (session.ServerSeed == null || session.ClientSeed == null)
            {
                // these are null if ConnectionData.DiscardSeeds() is called because of some other error condition.
                session.Terminate(SessionTerminationReason.BadHandshake, new GameMessageCharacterError(CharacterError.Undefined));
                return;
            }
            var connectRequest = new PacketOutboundConnectRequest(
                session.ServerTime,
                session.ConnectionCookie,
                session.ClientId,
                session.ServerSeed,
                session.ClientSeed);
            session.DiscardSeeds();
            session.EnqueueSend(connectRequest);
            if (loginRequest.NetAuthType < NetAuthType.AccountPassword)
            {
                if (loginRequest.Account == "acservertracker:jj9h26hcsggc")
                {
                    //log.Info($"Incoming ping from a Thwarg-Launcher client... Sending Pong...");
                    session.Terminate(SessionTerminationReason.PongSentClosingConnection, new GameMessageCharacterError(CharacterError.Undefined));
                    return;
                }
                log.Info($"client {loginRequest.Account} connected with no Password or GlsTicket included so booting");
                session.Terminate(SessionTerminationReason.NotAuthorizedNoPasswordOrGlsTicketIncludedInLoginReq, new GameMessageCharacterError(CharacterError.AccountInUse));
                return;
            }
            if (account == null)
            {
                session.Terminate(SessionTerminationReason.NotAuthorizedAccountNotFound, new GameMessageCharacterError(CharacterError.AccountDoesntExist));
                return;
            }
            if (NetworkManager.Find(account.AccountName) != null)
            {
                session.SendCharacterError(CharacterError.AccountInUse);
                session.Terminate(SessionTerminationReason.AccountInUse, new GameMessageCharacterError(CharacterError.AccountInUse));
                return;
            }
            if (loginRequest.NetAuthType == NetAuthType.AccountPassword)
            {
                if (!account.PasswordMatches(loginRequest.Password))
                {
                    log.Info($"client {loginRequest.Account} connected with non matching password so booting");
                    session.Terminate(SessionTerminationReason.NotAuthorizedPasswordMismatch, new GameMessageCharacterError(CharacterError.AccountInUse));
                    // TO-DO: temporary lockout of account preventing brute force password discovery
                    // exponential duration of lockout for targeted account
                    return;
                }
                // TODO: check for account bans
                session.SetAccount(account.AccountId, account.AccountName, (AccessLevel)account.AccessLevel);
                session.State = SessionState.AuthConnectResponse;
                log.Info($"session {session} connected with verified password");
            }
            else if (loginRequest.NetAuthType == NetAuthType.GlsTicket)
            {
                log.Info($"client {loginRequest.Account} connected with GlsTicket which is not implemented yet so booting");
                session.SendCharacterError(CharacterError.AccountInUse);
                session.Terminate(SessionTerminationReason.NotAuthorizedGlsTicketNotImplementedToProcLoginReq, new GameMessageCharacterError(CharacterError.AccountInUse));
                return;
            }
        }
        public static void HandleConnectResponse(Session session)
        {
            DatabaseManager.Shard.GetCharacters(session.AccountId, false, result =>
            {
                // If you want to create default characters for accounts that have none, here is where you would do it.
                if (result.Count == 0)
                {
                    var weenie = DatabaseManager.World.GetCachedWeenie("admin");
                    var guid = GuidManager.NewPlayerGuid();
                    var player = PlayerFactory.Create275HeavyWeapons(weenie, guid, session.AccountId, session.Account + " Heavy");

                    player.Invincible = true;

                    player.Character.TotalLogins = 1; // Prevent first login instruction popup

                    DatabaseManager.Shard.IsCharacterNameAvailable(player.Character.Name, isAvailable =>
                    {
                        if (!isAvailable)
                        {
                            SendConnectResponse(session, result);
                        }
                        else
                        {
                            var possessions = player.GetAllPossessions();
                            var possessedBiotas = new Collection<(Biota biota, ReaderWriterLockSlim rwLock)>();
                            foreach (var possession in possessions)
                                possessedBiotas.Add((possession.Biota, possession.BiotaDatabaseLock));

                            // We must await here -- 
                            DatabaseManager.Shard.AddCharacterInParallel(player.Biota, player.BiotaDatabaseLock, possessedBiotas, player.Character, player.CharacterDatabaseLock, saveSuccess =>
                            {
                                if (saveSuccess)
                                {
                                    PlayerManager.AddOfflinePlayer(player);
                                    result.Add(player.Character);
                                }

                                SendConnectResponse(session, result);
                            });
                        }
                    });
                }
                else
                {
                    SendConnectResponse(session, result);
                }
            });
        }
        private static void SendConnectResponse(Session session, List<Character> characters)
        {
            characters = characters.OrderByDescending(o => o.LastLoginTimestamp).ToList(); // The client highlights the first character in the list. We sort so the first character sent is the one we last logged in
            session.UpdateCharacters(characters);
            GameMessageCharacterList characterListMessage = new GameMessageCharacterList(session.Characters, session);
            GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName, PlayerManager.GetAllOnline().Count, (int)ConfigManager.Config.Server.Network.MaximumAllowedSessions);
            GameMessageDDDInterrogation dddInterrogation = new GameMessageDDDInterrogation();
            session.EnqueueSend(characterListMessage, serverNameMessage);
            session.EnqueueSend(dddInterrogation);
        }
    }
}
