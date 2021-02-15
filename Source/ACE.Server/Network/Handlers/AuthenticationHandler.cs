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
using ACE.Server.Entity;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Managers;
using ACE.Server.Network.Packets;

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
            try
            {
                PacketInboundLoginRequest loginRequest = new PacketInboundLoginRequest(packet);

                if (loginRequest.Account.Length > 50)
                {
                    NetworkManager.SendLoginRequestReject(session, CharacterError.AccountInvalid);
                    session.Terminate(SessionTerminationReason.AccountInformationInvalid);
                    return;
                }

                Task t = new Task(() => DoLogin(session, loginRequest));
                t.Start();
            }
            catch (Exception ex)
            {
                log.ErrorFormat("Received LoginRequest from {0} that threw an exception.", session.EndPoint);
                log.Error(ex);
            }
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
                        if (WorldManager.WorldStatus == WorldManager.WorldStatusState.Open)
                            log.Info($"Auto creating account for: {loginRequest.Account}");
                        else
                            log.Debug($"Auto creating account for: {loginRequest.Account}");

                        var accessLevel = (AccessLevel)ConfigManager.Config.Server.Accounts.DefaultAccessLevel;

                        if (!System.Enum.IsDefined(typeof(AccessLevel), accessLevel))
                            accessLevel = AccessLevel.Player;

                        if (DatabaseManager.AutoPromoteNextAccountToAdmin)
                        {
                            accessLevel = AccessLevel.Admin;
                            DatabaseManager.AutoPromoteNextAccountToAdmin = false;
                            log.Warn($"Automatically setting account AccessLevel to Admin for account \"{loginRequest.Account}\" because there are no admin accounts in the current database.");
                        }

                        account = DatabaseManager.Authentication.CreateAccount(loginRequest.Account.ToLower(), loginRequest.Password, accessLevel, session.EndPoint.Address);
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
            packetLog.DebugFormat("ConnectRequest TS: {0}", Timers.PortalYearTicks);

            if (session.Network.ConnectionData.ServerSeed == null || session.Network.ConnectionData.ClientSeed == null)
            {
                // these are null if ConnectionData.DiscardSeeds() is called because of some other error condition.
                session.Terminate(SessionTerminationReason.BadHandshake, new GameMessageCharacterError(CharacterError.ServerCrash1));
                return;
            }

            var connectRequest = new PacketOutboundConnectRequest(
                Timers.PortalYearTicks,
                session.Network.ConnectionData.ConnectionCookie,
                session.Network.ClientId,
                session.Network.ConnectionData.ServerSeed,
                session.Network.ConnectionData.ClientSeed);

            session.Network.ConnectionData.DiscardSeeds();

            session.Network.EnqueueSend(connectRequest);

            if (loginRequest.NetAuthType < NetAuthType.AccountPassword)
            {
                if (loginRequest.Account == "acservertracker:jj9h26hcsggc")
                {
                    //log.Info($"Incoming ping from a Thwarg-Launcher client... Sending Pong...");

                    session.Terminate(SessionTerminationReason.PongSentClosingConnection, new GameMessageCharacterError(CharacterError.ServerCrash1));

                    return;
                }

                if (WorldManager.WorldStatus == WorldManager.WorldStatusState.Open)
                    log.Info($"client {loginRequest.Account} connected with no Password or GlsTicket included so booting");
                else
                    log.Debug($"client {loginRequest.Account} connected with no Password or GlsTicket included so booting");

                session.Terminate(SessionTerminationReason.NotAuthorizedNoPasswordOrGlsTicketIncludedInLoginReq, new GameMessageCharacterError(CharacterError.AccountInvalid));

                return;
            }

            if (account == null)
            {
                session.Terminate(SessionTerminationReason.NotAuthorizedAccountNotFound, new GameMessageCharacterError(CharacterError.AccountDoesntExist));
                return;
            }

            if (!PropertyManager.GetBool("account_login_boots_in_use").Item)
            {
                if (NetworkManager.Find(account.AccountName) != null)
                {
                    session.Terminate(SessionTerminationReason.AccountInUse, new GameMessageCharacterError(CharacterError.Logon));
                    return;
                }
            }

            if (loginRequest.NetAuthType == NetAuthType.AccountPassword)
            {
                if (!account.PasswordMatches(loginRequest.Password))
                {
                    if (WorldManager.WorldStatus == WorldManager.WorldStatusState.Open)
                        log.Info($"client {loginRequest.Account} connected with non matching password so booting");
                    else
                        log.Debug($"client {loginRequest.Account} connected with non matching password so booting");

                    session.Terminate(SessionTerminationReason.NotAuthorizedPasswordMismatch, new GameMessageBootAccount(" because the password entered for this account was not correct"));

                    // TO-DO: temporary lockout of account preventing brute force password discovery
                    // exponential duration of lockout for targeted account

                    return;
                }

                if (PropertyManager.GetBool("account_login_boots_in_use").Item)
                {
                    var previouslyConnectedAccount = NetworkManager.Find(account.AccountName);

                    if (previouslyConnectedAccount != null)
                    {
                        // Boot the existing account
                        previouslyConnectedAccount.Terminate(SessionTerminationReason.AccountLoggedIn, new GameMessageCharacterError(CharacterError.Logon));

                        // We still can't let the new account in. They'll need to retry after the previous account has been successfully booted.
                        session.Terminate(SessionTerminationReason.AccountInUse, new GameMessageCharacterError(CharacterError.Logon));
                        return;
                    }
                }

                if (WorldManager.WorldStatus == WorldManager.WorldStatusState.Open)
                    log.Info($"client {loginRequest.Account} connected with verified password");
                else
                    log.Debug($"client {loginRequest.Account} connected with verified password");
            }
            else if (loginRequest.NetAuthType == NetAuthType.GlsTicket)
            {
                if (WorldManager.WorldStatus == WorldManager.WorldStatusState.Open)
                    log.Info($"client {loginRequest.Account} connected with GlsTicket which is not implemented yet so booting");
                else
                    log.Debug($"client {loginRequest.Account} connected with GlsTicket which is not implemented yet so booting");

                session.Terminate(SessionTerminationReason.NotAuthorizedGlsTicketNotImplementedToProcLoginReq, new GameMessageCharacterError(CharacterError.AccountInvalid));

                return;
            }

            if (account.BanExpireTime.HasValue)
            {
                var now = DateTime.UtcNow;
                if (now < account.BanExpireTime.Value)
                {
                    var reason = account.BanReason;
                    session.Terminate(SessionTerminationReason.AccountBanned, new GameMessageAccountBanned(account.BanExpireTime.Value, $"{(reason != null ? $" - {reason}" : null)}"), null, reason);
                    return;
                }
                else
                {
                    account.UnBan();
                }
            }

            account.UpdateLastLogin(session.EndPoint.Address);

            session.SetAccount(account.AccountId, account.AccountName, (AccessLevel)account.AccessLevel);
            session.State = SessionState.AuthConnectResponse;
        }

        public static void HandleConnectResponse(Session session)
        {
            if (WorldManager.WorldStatus == WorldManager.WorldStatusState.Open || session.AccessLevel > AccessLevel.Player)
            {
                DatabaseManager.Shard.GetCharacters(session.AccountId, false, result =>
                {
                    // If you want to create default characters for accounts that have none, here is where you would do it.

                    SendConnectResponse(session, result);
                });
            }
            else
            {
                session.Terminate(SessionTerminationReason.WorldClosed, new GameMessageCharacterError(CharacterError.LogonServerFull));
            }
        }

        private static void SendConnectResponse(Session session, List<Character> characters)
        {
            characters = characters.OrderByDescending(o => o.LastLoginTimestamp).ToList(); // The client highlights the first character in the list. We sort so the first character sent is the one we last logged in
            session.UpdateCharacters(characters);

            GameMessageCharacterList characterListMessage = new GameMessageCharacterList(session.Characters, session);
            GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName, PlayerManager.GetOnlineCount(), (int)ConfigManager.Config.Server.Network.MaximumAllowedSessions);
            GameMessageDDDInterrogation dddInterrogation = new GameMessageDDDInterrogation();

            session.Network.EnqueueSend(characterListMessage, serverNameMessage);
            session.Network.EnqueueSend(dddInterrogation);
        }
    }
}
