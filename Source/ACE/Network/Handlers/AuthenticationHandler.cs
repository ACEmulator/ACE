using ACE.Cryptography;
using ACE.Database;
using ACE.Entity;
using ACE.Managers;
using ACE.Network.GameEvent;
using System;
using System.Collections.Generic;

namespace ACE.Network
{
    public static class AuthenticationHandler
    {
        public static async void HandleLoginRequest(ClientPacket packet, Session session)
        {
            string someString = packet.Payload.ReadString16L();
            packet.Payload.ReadUInt32(); // data length left in packet including ticket
            packet.Payload.ReadUInt32();
            packet.Payload.ReadUInt32();
            uint timestamp    = packet.Payload.ReadUInt32();
            string account    = packet.Payload.ReadString16L();
            packet.Payload.ReadUInt32();
            string glsTicket  = packet.Payload.ReadString32L();

            try
            {
                var result = await DatabaseManager.Authentication.GetAccountByName(account);
                AccountSelectCallback(result, session);
            }
            catch (IndexOutOfRangeException)
            {
                AccountSelectCallback(null, session);
            }
        }

        private static void AccountSelectCallback(Account account, Session session)
        {
            var connectResponse = new ServerPacket(0x0B, PacketHeaderFlags.ConnectRequest);
            connectResponse.Payload.Write(0u);
            connectResponse.Payload.Write(0u);
            connectResponse.Payload.Write(13626398284849559039ul); // some sort of check value?
            connectResponse.Payload.Write((ushort)0);
            connectResponse.Payload.Write((ushort)0);
            connectResponse.Payload.Write(ISAAC.ServerSeed);
            connectResponse.Payload.Write(ISAAC.ClientSeed);
            connectResponse.Payload.Write(0u);

            NetworkManager.SendPacket(ConnectionType.Login, connectResponse, session);

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

            session.SetAccount(account.AccountId, account.Name);
            session.State = SessionState.AuthConnectResponse;
        }

        public static async void HandleConnectResponse(ClientPacket packet, Session session)
        {
            ulong check = packet.Payload.ReadUInt64(); // 13626398284849559039 - sent in previous packet

            var result = await DatabaseManager.Character.GetByAccount(session.Id);
            CharacterListSelectCallback(result, session);

            // looks like account settings/info, expansion information ect? (this is needed for world entry)
            var packet75e5         = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            var packet75e5Fragment = new ServerPacketFragment(5, FragmentOpcode.Unknown75E5);
            packet75e5Fragment.Payload.Write(1ul);
            packet75e5Fragment.Payload.Write(1ul);
            packet75e5Fragment.Payload.Write(1ul);
            packet75e5Fragment.Payload.Write(2ul);
            packet75e5Fragment.Payload.Write(0ul);
            packet75e5Fragment.Payload.Write(1ul);
            packet75e5.Fragments.Add(packet75e5Fragment);

            NetworkManager.SendPacket(ConnectionType.Login, packet75e5, session);

            var patchStatus = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            patchStatus.Fragments.Add(new ServerPacketFragment(5, FragmentOpcode.PatchStatus));

            NetworkManager.SendPacket(ConnectionType.Login, patchStatus, session);

            session.State = SessionState.AuthConnected;
        }

        public static void CharacterListSelectCallback(List<CachedCharacter> characters, Session session)
        {
            var characterList     = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            var characterFragment = new ServerPacketFragment(9, FragmentOpcode.CharacterList);
            characterFragment.Payload.Write(0u);
            characterFragment.Payload.Write(characters.Count);

            session.CachedCharacters.Clear();
            foreach(var character in characters)
            {
                characterFragment.Payload.WriteGuid(character.Guid);
                characterFragment.Payload.WriteString16L(character.Name);
                characterFragment.Payload.Write(character.DeleteTime != 0ul ? (uint)(WorldManager.GetUnixTime() - character.DeleteTime) : 0u);
                session.CachedCharacters.Add(character);
            }

            characterFragment.Payload.Write(0u);
            characterFragment.Payload.Write(11u /*slotCount*/);
            characterFragment.Payload.WriteString16L(session.Account);
            characterFragment.Payload.Write(0u /*useTurbineChat*/);
            characterFragment.Payload.Write(0u /*hasThroneOfDestiny*/);
            characterList.Fragments.Add(characterFragment);

            NetworkManager.SendPacket(ConnectionType.Login, characterList, session);

            var serverName         = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            var serverNameFragment = new ServerPacketFragment(9, FragmentOpcode.ServerName);
            serverNameFragment.Payload.Write(0u);
            serverNameFragment.Payload.Write(0u);
            serverNameFragment.Payload.WriteString16L(ConfigManager.Config.Server.WorldName);
            serverName.Fragments.Add(serverNameFragment);

            NetworkManager.SendPacket(ConnectionType.Login, serverName, session);
        }

        public static void HandleWorldLoginRequest(ClientPacket packet, Session session)
        {
            ulong connectionKey = packet.Payload.ReadUInt64();
            if (session.WorldConnectionKey == 0)
                session = WorldManager.Find(connectionKey);

            if (connectionKey != session.WorldConnectionKey || connectionKey == 0)
            {
                session.SendCharacterError(CharacterError.EnterGamePlayerAccountMissing);
                return;
            }

            session.WorldConnection    = new SessionConnectionData(ConnectionType.World);
            session.Player          = new Player(session);
            session.CharacterRequested = null;
            session.State              = SessionState.WorldConnectResponse;

            var connectResponse = new ServerPacket(0x18, PacketHeaderFlags.ConnectRequest);
            connectResponse.Payload.Write(0u);
            connectResponse.Payload.Write(0u);
            connectResponse.Payload.Write(13626398284849559039ul); // some sort of check value?
            connectResponse.Payload.Write((ushort)0);
            connectResponse.Payload.Write((ushort)0);
            connectResponse.Payload.Write(ISAAC.WorldServerSeed);
            connectResponse.Payload.Write(ISAAC.WorldClientSeed);
            connectResponse.Payload.Write(0u);

            NetworkManager.SendPacket(ConnectionType.World, connectResponse, session);
        }

        public static void HandleWorldConnectResponse(ClientPacket packet, Session session)
        {
            session.State = SessionState.WorldConnected;

            var serverSwitch = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.ServerSwitch);
            serverSwitch.Payload.Write((uint)0x18);
            serverSwitch.Payload.Write((uint)0x00);

            NetworkManager.SendPacket(ConnectionType.World, serverSwitch, session);

            new GameEventPopupString(session, ConfigManager.Config.Server.Welcome).Send();
            session.Player.Load();
        }
    }
}
