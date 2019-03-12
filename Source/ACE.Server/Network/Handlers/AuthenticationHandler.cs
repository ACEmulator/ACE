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
            PacketInboundLoginRequest loginRequest = new PacketInboundLoginRequest(packet);
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
                session.DropSession("AccountSelectCallback threw an exception.");
            }
        }


        private static void AccountSelectCallback(Account account, Session session, PacketInboundLoginRequest loginRequest)
        {
            packetLog.DebugFormat("ConnectRequest TS: {0}", session.Network.ConnectionData.ServerTime);

            if (session.Network.ConnectionData.ServerSeed == null || session.Network.ConnectionData.ClientSeed == null)
            {
                // these are null if ConnectionData.DiscardSeeds() is called because of some other error condition.
                session.BootSession("Bad handshake", new GameMessageCharacterError(CharacterError.Undefined));
                return;
            }

            var connectRequest = new PacketOutboundConnectRequest(
                session.Network.ConnectionData.ServerTime,
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
                    log.Info($"Incoming ping from a Thwarg-Launcher client... Sending Pong...");

                    session.BootSession("Pong sent, closing connection.", new GameMessageCharacterError(CharacterError.Undefined));

                    return;
                }

                log.Info($"client {loginRequest.Account} connected with no Password or GlsTicket included so booting");
               
                session.BootSession("Not Authorized: No password or GlsTicket included in login request", new GameMessageCharacterError(CharacterError.AccountInUse));

                return;
            }

            if (account == null)
            {
                session.BootSession("Not Authorized: Account Not Found", new GameMessageCharacterError(CharacterError.AccountDoesntExist));
                return;
            }

            if (WorldManager.Find(account.AccountName) != null)
            {
                session.SendCharacterError(CharacterError.AccountInUse);
                session.BootSession("Account In Use: Found another session already logged in for this account.", new GameMessageCharacterError(CharacterError.AccountInUse));
                return;
            }

            if (loginRequest.NetAuthType == NetAuthType.AccountPassword)
            {
                if (!account.PasswordMatches(loginRequest.Password))
                {
                    log.Info($"client {loginRequest.Account} connected with non matching password does so booting");

                    session.BootSession("Not Authorized: Password does not match.", new GameMessageCharacterError(CharacterError.AccountInUse));

                    // TO-DO: temporary lockout of account preventing brute force password discovery
                    // exponential duration of lockout for targeted account

                    return;
                }

                log.Info($"client {loginRequest.Account} connected with verified password");
            }
            else if (loginRequest.NetAuthType == NetAuthType.GlsTicket)
            {
                log.Info($"client {loginRequest.Account} connected with GlsTicket which is not implemented yet so booting");

                session.SendCharacterError(CharacterError.AccountInUse);
                session.BootSession("Not Authorized: GlsTicket is not implemented to process login request", new GameMessageCharacterError(CharacterError.AccountInUse));

                return;
            }

            // TODO: check for account bans

            session.SetAccount(account.AccountId, account.AccountName, (AccessLevel)account.AccessLevel);
            session.State = SessionState.AuthConnectResponse;
        }

        public static void HandleConnectResponse(Session session)
        {
            DatabaseManager.Shard.GetCharacters(session.AccountId, false, result =>
            {
                // If you want to create default characters for accounts that have none, here is where you would do it.

                SendConnectResponse(session, result);
            });
        }

        private static void SendConnectResponse(Session session, List<Character> characters)
        {
            characters = characters.OrderByDescending(o => o.LastLoginTimestamp).ToList(); // The client highlights the first character in the list. We sort so the first character sent is the one we last logged in
            session.UpdateCharacters(characters);

            GameMessageCharacterList characterListMessage = new GameMessageCharacterList(session.Characters, session);
            GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName, PlayerManager.GetAllOnline().Count, (int)ConfigManager.Config.Server.Network.MaximumAllowedSessions);
            GameMessageDDDInterrogation dddInterrogation = new GameMessageDDDInterrogation();

            session.Network.EnqueueSend(characterListMessage, serverNameMessage);
            session.Network.EnqueueSend(dddInterrogation);
        }
    }
}
