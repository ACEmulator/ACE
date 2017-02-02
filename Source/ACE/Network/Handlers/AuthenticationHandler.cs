﻿using ACE.Cryptography;
using ACE.Database;
using ACE.Entity;
using ACE.Managers;
using ACE.Network.GameEvent;

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

            var result = await DatabaseManager.Authentication.SelectPreparedStatementAsync(AuthenticationPreparedStatement.AccountSelect, account);
            AccountSelectCallback(result, session);
        }

        private static void AccountSelectCallback(MySqlResult result, Session session)
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

            if (result.Count == 0)
            {
                session.SendCharacterError(CharacterError.AccountDoesntExist);
                return;
            }

            uint accountId = result.Read<uint>(0, "id");
            string account = result.Read<string>(0, "account");

            if (WorldManager.Find(account) != null)
            {
                session.SendCharacterError(CharacterError.AccountInUse);
                return;
            }

            string digest = SHA2.Hash(SHA2Type.SHA256, result.Read<string>(0, "password") + result.Read<string>(0, "salt"));
            /*if (glsTicket != digest)
            {
            }*/

            /*if (WorldManager.ServerIsFull())
            {
                session.SendCharacterError(CharacterError.LogonServerFull);
                return;
            }*/

            // TODO: check for account bans

            session.SetAccount(accountId, account);
        }

        public static async void HandleConnectResponse(ClientPacket packet, Session session)
        {
            ulong check = packet.Payload.ReadUInt64(); // 13626398284849559039 - sent in previous packet

            var result = await DatabaseManager.Character.SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterListSelect, session.Id);
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
        }

        public static void CharacterListSelectCallback(MySqlResult result, Session session)
        {
            var characterList     = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            var characterFragment = new ServerPacketFragment(9, FragmentOpcode.CharacterList);
            characterFragment.Payload.Write(0u);
            characterFragment.Payload.Write(result.Count);

            session.CachedCharacters.Clear();
            for (byte i = 0; i < result.Count; i++)
            {
                uint lowGuid = result.Read<uint>(i, "guid");
                string name  = result.Read<string>(i, "name");

                characterFragment.Payload.Write(lowGuid);
                characterFragment.Payload.WriteString16L(name);

                ulong deleteTime = result.Read<ulong>(i, "deleteTime");
                characterFragment.Payload.Write(deleteTime != 0ul ? (uint)(WorldManager.GetUnixTime() - deleteTime) : 0u);

                session.CachedCharacters.Add(new CachedCharacter(lowGuid, i, name));
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
            if (connectionKey != session.WorldConnectionKey || connectionKey == 0)
            {
                session.SendCharacterError(CharacterError.EnterGamePlayerAccountMissing);
                return;
            }

            session.WorldConnection    = new SessionConnectionData(ConnectionType.World);
            session.Character          = new Player(session);
            session.CharacterRequested = null;

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
            var serverSwitch = new ServerPacket(0x18, PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.ServerSwitch);
            serverSwitch.Payload.Write((uint)0x18);
            serverSwitch.Payload.Write((uint)0x00);

            NetworkManager.SendPacket(ConnectionType.World, serverSwitch, session);

            new GameEventPopupString(session, ConfigManager.Config.Server.Welcome).Send();
            session.Character.Load();
        }
    }
}
