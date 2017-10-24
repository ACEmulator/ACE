using System;

using ACE.Common;
using ACE.Common.Cryptography;
using ACE.Database;
using ACE.Entity;
using ACE.Managers;
using ACE.Network.Enum;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Packets;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using log4net;
using ACE.Api.Common;
using System.Security.Principal;

namespace ACE.Network.Handlers
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
            if (!string.IsNullOrEmpty(loginRequest.JwtToken))
            {
                log.Info($"First 2 of token: {Convert.ToByte(loginRequest.JwtToken[0])}, {Convert.ToByte(loginRequest.JwtToken[1])})");
                Task t = new Task(() => DoLogin(session, loginRequest));
                t.Start();
            }
            else
            {
                log.Info("Login request has an empty JwtToken.");
                Task t = new Task(() => DoLogin(session, loginRequest));
                t.Start();
            }
        }

        private static void DoLogin(Session session, PacketInboundLoginRequest loginRequest)
        {
            // validate the token
            Guid accountGuid;
            string accountName;
            string loggingIdentifier;
            Guid subscriptionGuid;
            Subscription sub;

            if (ConfigManager.Config.Server.SecureAuthentication)
            {
                try
                {
                    var tokenInfo = JwtManager.ParseRemoteToken(loginRequest.JwtToken);
                    if (tokenInfo == null)
                        throw new UnauthorizedAccessException($"improper token used for login {loginRequest.ClientAccountString}, token {loginRequest.JwtToken}");

                    accountName = tokenInfo.Name;
                    accountGuid = tokenInfo.AccountGuid;
                }
                catch (Exception ex)
                {
                    log.Info("Error in HandleLoginRequest validating the ticket.", ex);
                    session.SendCharacterError(CharacterError.AccountInvalid);
                    return;
                }

                if (!Guid.TryParse(loginRequest.ClientAccountString, out subscriptionGuid))
                {
                    // if it's not a guid, it's an account name.  verify it matches the ticket
                    if (accountName != loginRequest.ClientAccountString)
                    {
                        log.Info("Error in HandleLoginRequest validating the ticket.");
                        session.SendCharacterError(CharacterError.AccountInvalid);
                        return;
                    }
                    else
                    {
                        // look for subscriptions
                        var subs = DatabaseManager.Authentication.GetSubscriptionsByAccount(accountGuid);
                        if (subs.Count < 1)
                        {
                            // go go gadget dynamic subscription creation
                            sub = new Subscription()
                            {
                                AccessLevel = Entity.Enum.AccessLevel.Player,
                                Name = "auto",
                                AccountGuid = accountGuid
                            };
                            DatabaseManager.Authentication.CreateSubscription(sub);
                        }
                        else
                        {
                            // already have a subscription, just pull it
                            sub = subs[0];
                        }
                    }
                }
                else
                {
                    var subs = DatabaseManager.Authentication.GetSubscriptionsByAccount(accountGuid);
                    sub = subs.Find(s => s.SubscriptionGuid == subscriptionGuid);
                }

                loggingIdentifier = $"{accountName}.{sub.Name}";
            }
            else
            {
                // insecure mode.  we have no token of value, and only the ClientAccountString.
                if (!Guid.TryParse(loginRequest.ClientAccountString, out subscriptionGuid))
                {
                    // client account string is not a guid. assume it is an account name
                    var account = DatabaseManager.Authentication.GetAccountByName(loginRequest.ClientAccountString);

                    if (account == null)
                    {
                        // no account, dynamically create one
                        account = new Account();
                        account.Name = loginRequest.ClientAccountString;
                        account.DisplayName = loginRequest.ClientAccountString;
                        account.SetPassword("");
                        DatabaseManager.Authentication.CreateAccount(account);
                    }

                    // look for subscriptions
                    var subs = DatabaseManager.Authentication.GetSubscriptionsByAccount(account.AccountGuid);
                    if (subs.Count < 1)
                    {
                        // go go gadget dynamic subscription creation
                        sub = new Subscription()
                        {
                            AccessLevel = Entity.Enum.AccessLevel.Player,
                            Name = "default",
                            AccountGuid = account.AccountGuid
                        };
                        DatabaseManager.Authentication.CreateSubscription(sub);
                    }
                    else
                    {
                        // already have a subscription, just pull it
                        sub = subs[0];
                    }

                    loggingIdentifier = $"{account.Name}.{sub.Name}";
                }
                else
                {
                    // subscription guid provided
                    sub = DatabaseManager.Authentication.GetSubscriptionByGuid(subscriptionGuid);

                    loggingIdentifier = $"Unknown.{sub.Name}";
                }
            }

            try
            {
                log.Info($"new client connected: {loginRequest.ClientAccountString}. setting session properties");
                SubscriptionSelectCallback(sub, session, loginRequest.ClientAccountString, loggingIdentifier);
            }
            catch (Exception ex)
            {
                log.Info("Error in HandleLoginRequest trying to find the subscription.", ex);
                SubscriptionSelectCallback(null, session, null, null);
            }
        }

        private static void SubscriptionSelectCallback(Subscription subscription, Session session, string clientAccountString, string loggingIdentifier)
        {
            if (subscription == null)
            {
                log.Debug("subscription null in SubscriptionSelectCallback.");
                session.SendCharacterError(CharacterError.AccountDoesntExist);
                return;
            }

            var connectRequest = new PacketOutboundConnectRequest(session.Network.ConnectionData.ServerTime, 0, session.Network.ClientId, ISAAC.ServerSeed, ISAAC.ClientSeed);
            session.Network.EnqueueSend(connectRequest);

            if (WorldManager.Find(subscription.SubscriptionId) != null)
            {
                var foundSession = WorldManager.Find(subscription.SubscriptionId);
                log.Debug($"found session for {subscription.SubscriptionGuid}");

                if (foundSession.State == SessionState.AuthConnected)
                    session.SendCharacterError(CharacterError.AccountInUse);
                return;
            }

            session.SetSubscription(subscription, clientAccountString, loggingIdentifier);
            session.State = SessionState.AuthConnectResponse;
        }

        public static void HandleConnectResponse(ClientPacket packet, Session session)
        {
            PacketInboundConnectResponse connectResponse = new PacketInboundConnectResponse(packet);

            log.Debug($"Getting characters for subscription id {session.SubscriptionId}");
            DatabaseManager.Shard.GetCharacters(session.SubscriptionId, ((List<CachedCharacter> result) =>
            {
                result = result.OrderByDescending(o => o.LoginTimestamp).ToList();
                session.UpdateCachedCharacters(result);

                GameMessageCharacterList characterListMessage = new GameMessageCharacterList(result, session.ClientAccountString);
                GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName, WorldManager.GetAll().Count, (int)ConfigManager.Config.Server.Network.MaximumAllowedSessions);
                GameMessageDDDInterrogation dddInterrogation = new GameMessageDDDInterrogation();

                log.Debug($"sending {result.Count} characters to {session.ClientAccountString} for {ConfigManager.Config.Server.WorldName}");
                session.Network.EnqueueSend(characterListMessage, serverNameMessage);
                session.Network.EnqueueSend(dddInterrogation);

                session.State = SessionState.AuthConnected;
            }));
        }
    }
}
