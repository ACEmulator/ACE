using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using log4net;

using ACE.Common;
using ACE.Common.Cryptography;
using ACE.Database;
using ACE.Database.Models.Auth;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Server.Command.Handlers;
using ACE.Server.Managers;
using ACE.Server.Network.Enum;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Packets;

using Account = ACE.Database.Models.Auth.Account;

namespace ACE.Server.Network.Handlers
{
    public static class AuthenticationHandler
    {
        /// <summary>
        /// Seconds until an authentication request will timeout/expire.
        /// </summary>
        public const int DefaultAuthTimeout = 15;

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
                        log.Info($"Auto creating account for: {loginRequest.Account}");
                        // no account, dynamically create one
                        string[] parameters = new string[] { loginRequest.Account, loginRequest.Password };
                        AccountCommands.HandleAccountCreate(session, parameters);
                        account = DatabaseManager.Authentication.GetAccountByName(loginRequest.Account);
                    }
                }
            }

            try
            {
                log.Info($"new client connected: {loginRequest.Account}. setting session properties");
                AccountSelectCallback(account, session, loginRequest);
            }
            catch (Exception ex)
            {
                log.Info("Error in HandleLoginRequest trying to find the account.", ex);
                AccountSelectCallback(null, session, null);
            }
        }


        private static void AccountSelectCallback(Account account, Session session, PacketInboundLoginRequest loginRequest)
        {
            log.DebugFormat("ConnectRequest TS: {0}", session.Network.ConnectionData.ServerTime);
            var connectRequest = new PacketOutboundConnectRequest(session.Network.ConnectionData.ServerTime, 0, session.Network.ClientId, ISAAC.ServerSeed, ISAAC.ClientSeed);

            session.Network.EnqueueSend(connectRequest);

            if (loginRequest.NetAuthType < NetAuthType.AccountPassword)
            {
                log.Info($"client {loginRequest.Account} connected with no Password or GlsTicket included so booting");

                session.SendCharacterError(CharacterError.AccountInUse);
                session.State = SessionState.NetworkTimeout;

                return;
            }

            if (account == null)
            {
                session.SendCharacterError(CharacterError.AccountDoesntExist);
                session.State = SessionState.NetworkTimeout;
                return;
            }

            if (WorldManager.Find(account.AccountName) != null)
            {
                var foundSession = WorldManager.Find(account.AccountName);

                if (foundSession.State == SessionState.AuthConnected)
                {
                    session.SendCharacterError(CharacterError.AccountInUse);
                    session.State = SessionState.NetworkTimeout;
                }
                return;
            }

            if (loginRequest.NetAuthType == NetAuthType.AccountPassword)
            {
                if (!account.PasswordMatches(loginRequest.Password))
                {
                    log.Info($"client {loginRequest.Account} connected with non matching password does so booting");

                    session.SendCharacterError(CharacterError.AccountInUse);
                    session.State = SessionState.NetworkTimeout;

                    return;
                }

                log.Info($"client {loginRequest.Account} connected with verified password");
            }
            else if (loginRequest.NetAuthType == NetAuthType.GlsTicket)
            {
                log.Info($"client {loginRequest.Account} connected with GlsTicket which is not implemented yet so booting");

                session.SendCharacterError(CharacterError.AccountInUse);
                session.State = SessionState.NetworkTimeout;

                return;
            }

            // TODO: check for account bans

            session.SetAccount(account.AccountId, account.AccountName, (AccessLevel)account.AccessLevel);
            session.State = SessionState.AuthConnectResponse;
        }

        public static void HandleConnectResponse(ClientPacket packet, Session session)
        {
            PacketInboundConnectResponse connectResponse = new PacketInboundConnectResponse(packet);

            DatabaseManager.Shard.GetCharacters(session.Id, ((List<CachedCharacter> result) =>
            {
                result = result.OrderByDescending(o => o.LoginTimestamp).ToList();
                session.UpdateCachedCharacters(result);

                GameMessageCharacterList characterListMessage = new GameMessageCharacterList(result, session.Account);
                GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName, WorldManager.GetAll().Count, (int)ConfigManager.Config.Server.Network.MaximumAllowedSessions);
                GameMessageDDDInterrogation dddInterrogation = new GameMessageDDDInterrogation();

                session.Network.EnqueueSend(characterListMessage, serverNameMessage);
                session.Network.EnqueueSend(dddInterrogation);

                session.State = SessionState.AuthConnected;
            }));
        }
    }
}
