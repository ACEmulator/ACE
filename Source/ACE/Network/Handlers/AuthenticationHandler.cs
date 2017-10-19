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

        public static async void HandleLoginRequest(ClientPacket packet, Session session)
        {
            PacketInboundLoginRequest loginRequest = new PacketInboundLoginRequest(packet);

            // validate the token
            Guid accountGuid;

            try
            {
                var principal = AceJwtTokenHandler.ValidateAndGetClaims(loginRequest.JwtToken);
                accountGuid = Guid.Parse(principal.Identity.Name);
            }
            catch (Exception ex)
            {
                log.Info("Error in HandleLoginRequest validating the ticket.", ex);
                session.SendCharacterError(CharacterError.AccountInvalid);
                return;
            }

            Task t = new Task(() =>
            {
                try
                {
                    Guid subscriptionGuid = Guid.Parse(loginRequest.ClientAccountString);
                    var subs = DatabaseManager.Authentication.GetSubscriptionsByAccount(accountGuid);
                    var sub = subs.Find(s => s.SubscriptionGuid == subscriptionGuid);
                    log.Info($"new client connected: {loginRequest.ClientAccountString}. setting session properties");
                    SubscriptionSelectCallback(sub, session, loginRequest.ClientAccountString);
                }
                catch (Exception ex)
                {
                    log.Info("Error in HandleLoginRequest trying to find the subscription.", ex);
                    SubscriptionSelectCallback(null, session, null);
                }
            });

            t.Start();

            await t;
        }

        private static void SubscriptionSelectCallback(Subscription subscription, Session session, string clientAccountString)
        {
            if (subscription == null)
            {
                log.Info("subscription null in SubscriptionSelectCallback.");
                session.SendCharacterError(CharacterError.AccountDoesntExist);
                return;
            }

            var connectRequest = new PacketOutboundConnectRequest(session.Network.ConnectionData.ServerTime, 0, session.Network.ClientId, ISAAC.ServerSeed, ISAAC.ClientSeed);
            session.Network.EnqueueSend(connectRequest);

            if (WorldManager.Find(subscription.SubscriptionId) != null)
            {
                var foundSession = WorldManager.Find(subscription.SubscriptionId);
                log.Info($"found session for {subscription.SubscriptionGuid}");

                if (foundSession.State == SessionState.AuthConnected)
                    session.SendCharacterError(CharacterError.AccountInUse);
                return;
            }
            
            session.SetSubscription(subscription, clientAccountString);
            session.State = SessionState.AuthConnectResponse;
        }

        public static void HandleConnectResponse(ClientPacket packet, Session session)
        {
            PacketInboundConnectResponse connectResponse = new PacketInboundConnectResponse(packet);

            DatabaseManager.Shard.GetCharacters(session.SubscriptionId, ((List<CachedCharacter> result) =>
            {
                result = result.OrderByDescending(o => o.LoginTimestamp).ToList();
                session.UpdateCachedCharacters(result);

                GameMessageCharacterList characterListMessage = new GameMessageCharacterList(result, session.ClientAccountString);
                GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName, WorldManager.GetAll().Count, (int)ConfigManager.Config.Server.Network.MaximumAllowedSessions);
                GameMessageDDDInterrogation dddInterrogation = new GameMessageDDDInterrogation();

                session.Network.EnqueueSend(characterListMessage, serverNameMessage);
                session.Network.EnqueueSend(dddInterrogation);

                session.State = SessionState.AuthConnected;
            }));
        }
    }
}
