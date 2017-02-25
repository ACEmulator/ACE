using System;
using System.Collections.Generic;

using ACE.Common;
using ACE.Common.Cryptography;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.Entity;
using ACE.Managers;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.GameEvent.Events;
using ACE.Network.Managers;
using ACE.Network.Packets;

namespace ACE.Network.Handlers
{
    public static class AuthenticationHandler
    {
        public static async void HandleLoginRequest(ClientPacket packet, Session session)
        {
            PacketInboundLoginRequest loginRequest = new PacketInboundLoginRequest(packet);

            try
            {
                var result = await DatabaseManager.Authentication.GetAccountByName(loginRequest.Account);
                AccountSelectCallback(result, session);
            }
            catch (IndexOutOfRangeException)
            {
                AccountSelectCallback(null, session);
            }
        }

        private static void AccountSelectCallback(Account account, Session session)
        {
            var connectResponse = new PacketOutboundConnectRequest(ISAAC.ServerSeed, ISAAC.ClientSeed);

            session.LoginSession.Enqueue(connectResponse);

            if (account == null)
            {
                session.SendCharacterError(CharacterError.AccountDoesntExist);
                return;
            }
            
            if (WorldManager.Find(account.Name) != null)
            {
                session.SendCharacterError(CharacterError.AccountInUse);
                return;
            }
            
            /*if (glsTicket != digest)
            {
            }*/

            /*if (WorldManager.ServerIsFull())
            {
                session.SendCharacterError(CharacterError.LogonServerFull);
                return;
            }*/

            // TODO: check for account bans

            session.SetAccount(account.AccountId, account.Name, account.AccessLevel);
            session.State = SessionState.AuthConnectResponse;
        }

        public static async void HandleConnectResponse(ClientPacket packet, Session session)
        {
            PacketInboundConnectResponse connectResponse = new PacketInboundConnectResponse(packet);

            var result = await DatabaseManager.Character.GetByAccount(session.Id);

            session.UpdateCachedCharacters(result);

            GameMessageCharacterList characterListMessage = new GameMessageCharacterList(result, session.Account);
            GameMessageServerName serverNameMessage = new GameMessageServerName(ConfigManager.Config.Server.WorldName);
            // looks like account settings/info, expansion information ect? (this is needed for world entry)
            GameMessageF7E5 unknown75e5Message = new GameMessageF7E5();
            GameMessagePatchStatus patchStatusMessage = new GameMessagePatchStatus();
            session.LoginSession.Enqueue(characterListMessage);
            session.LoginSession.Enqueue(serverNameMessage);
            session.LoginSession.Enqueue(unknown75e5Message);
            session.LoginSession.Enqueue(patchStatusMessage);

            session.State = SessionState.AuthConnected;
        }

        public static void HandleWorldLoginRequest(ClientPacket packet, Session session)
        {
            PacketInboundWorldLoginRequest loginRequest = new PacketInboundWorldLoginRequest(packet);
            ulong connectionKey = loginRequest.ConnectionKey;
            if (session.WorldConnectionKey == 0)
                session = WorldManager.Find(connectionKey);

            if (connectionKey != session.WorldConnectionKey || connectionKey == 0)
            {
                session.SendCharacterError(CharacterError.EnterGamePlayerAccountMissing);
                return;
            }

            session.State = SessionState.WorldConnectResponse;
            
            PacketOutboundConnectRequest connectRequest = new PacketOutboundConnectRequest(ISAAC.WorldServerSeed, ISAAC.WorldClientSeed);
            session.WorldSession.Enqueue(connectRequest);
        }

        public static void HandleWorldConnectResponse(ClientPacket packet, Session session)
        {
            session.State = SessionState.WorldConnected;
            var serverSwitch = new PacketOutboundServerSwitch();
            session.WorldSession.Enqueue(serverSwitch);
            session.WorldSession.Enqueue(new GameEventPopupString(session, ConfigManager.Config.Server.Welcome));
            session.Player.Load();
        }
    }
}
