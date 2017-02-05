using ACE.Database;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Diagnostics;
using ACE.Entity;

namespace ACE.Network
{
    public static class CharacterHandler
    {
        [Fragment(FragmentOpcode.CharacterEnterWorldRequest, SessionState.AuthConnected)]
        public static void CharacterEnterWorldRequest(ClientPacketFragment fragment, Session session)
        {
            var characterEnterWorldServerReady = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            characterEnterWorldServerReady.Fragments.Add(new ServerPacketFragment(9, FragmentOpcode.CharacterEnterWorldServerReady));

            NetworkManager.SendPacket(ConnectionType.Login, characterEnterWorldServerReady, session);
        }

        [Fragment(FragmentOpcode.CharacterEnterWorld, SessionState.AuthConnected)]
        public static void CharacterEnterWorld(ClientPacketFragment fragment, Session session)
        {
            uint guid      = fragment.Payload.ReadUInt32();
            string account = fragment.Payload.ReadString16L();

            if (account != session.Account)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            var cachedCharacter = session.CachedCharacters.SingleOrDefault(c => c.LowGuid == guid);
            if (cachedCharacter == null)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            session.CharacterRequested = cachedCharacter;

            // this isn't really that necessary since ACE doesn't split login/world to multiple daemons, handle it anyway
            byte[] connectionKey = new byte[sizeof(ulong)];
            RandomNumberGenerator.Create().GetNonZeroBytes(connectionKey);

            session.WorldConnectionKey = BitConverter.ToUInt64(connectionKey, 0);

            var referralPacket = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum | PacketHeaderFlags.Referral);
            referralPacket.Payload.Write(session.WorldConnectionKey);
            referralPacket.Payload.Write((ushort)2);
            referralPacket.Payload.WriteUInt16BE((ushort)ConfigManager.Config.Server.Network.WorldPort);

            string[] ConnectingIPAddress = session.EndPoint.Address.ToString().Split('.');
            if (ConfigManager.Config.Server.Network.SendInternalHostOnLocalNetwork && 
                (ConnectingIPAddress[0] == "10" 
                || (ConnectingIPAddress[0] == "172" && System.Convert.ToInt16(ConnectingIPAddress[1]) >= 16 && System.Convert.ToInt16(ConnectingIPAddress[1]) <= 31) 
                || (ConnectingIPAddress[0] == "192" && ConnectingIPAddress[1] == "168")))
            {
                referralPacket.Payload.Write(ConfigManager.InternalHost);
            }
            else
            {
                referralPacket.Payload.Write(ConfigManager.Host);
            }

            referralPacket.Payload.Write(0ul);
            referralPacket.Payload.Write((ushort)0x18);
            referralPacket.Payload.Write((ushort)0);
            referralPacket.Payload.Write(0u);

            NetworkManager.SendPacket(ConnectionType.Login, referralPacket, session);

            session.State = SessionState.WorldLoginRequest;
        }

        [Fragment(FragmentOpcode.CharacterDelete, SessionState.AuthConnected)]
        public static async void CharacterDelete(ClientPacketFragment fragment, Session session)
        {
            string account     = fragment.Payload.ReadString16L();
            uint characterSlot = fragment.Payload.ReadUInt32();

            if (account != session.Account)
            {
                session.SendCharacterError(CharacterError.Delete);
                return;
            }

            var cachedCharacter = session.CachedCharacters.SingleOrDefault(c => c.SlotId == characterSlot);
            if (cachedCharacter == null)
            {
                session.SendCharacterError(CharacterError.Delete);
                return;
            }

            // TODO: check if character is already pending removal

            var characterDelete         = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            var characterDeleteFragment = new ServerPacketFragment(9, FragmentOpcode.CharacterDelete);
            characterDelete.Fragments.Add(characterDeleteFragment);

            NetworkManager.SendPacket(ConnectionType.Login, characterDelete, session);

            DatabaseManager.Character.DeleteOrRestore(WorldManager.GetUnixTime() + 3600ul, cachedCharacter.LowGuid);

            var result = await DatabaseManager.Character.GetByAccount(session.Id);
            AuthenticationHandler.CharacterListSelectCallback(result, session);
        }

        [Fragment(FragmentOpcode.CharacterRestore, SessionState.AuthConnected)]
        public static void CharacterRestore(ClientPacketFragment fragment, Session session)
        {
            uint guid = fragment.Payload.ReadUInt32();

            var cachedCharacter = session.CachedCharacters.SingleOrDefault(c => c.LowGuid == guid);
            if (cachedCharacter == null)
                return;

            DatabaseManager.Character.DeleteOrRestore(0, guid);

            var characterRestore         = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            var characterRestoreFragment = new ServerPacketFragment(9, FragmentOpcode.CharacterRestoreResponse);
            characterRestoreFragment.Payload.Write(1u /* Verification OK flag */);
            characterRestoreFragment.Payload.Write(guid);
            characterRestoreFragment.Payload.WriteString16L(cachedCharacter.Name);
            characterRestoreFragment.Payload.Write(0u /* secondsGreyedOut */);
            characterRestore.Fragments.Add(characterRestoreFragment);

            NetworkManager.SendPacket(ConnectionType.Login, characterRestore, session);
        }

        [Fragment(FragmentOpcode.CharacterCreate, SessionState.AuthConnected)]
        public static async void CharacterCreate(ClientPacketFragment fragment, Session session)
        {
            // known issues:
            // 1. getting the "next" character id is not thread-safe

            string account = fragment.Payload.ReadString16L();

            if (account != session.Account)
            {
                return;
            }

            Character character = Character.CreateFromClientFragment(fragment, session.Id);
            
            // TODO: profanity filter 
            // sendCharacterCreateResponse(session, 4);
            
            bool isAvailable = DatabaseManager.Character.IsNameAvailable(character.Name);
            if (!isAvailable)
            {
                SendCharacterCreateResponse(session, 3);    /* Name already in use. */
                return;
            }
            
            uint guid = DatabaseManager.Character.GetMaxId() + 1;
            character.Id = guid;
            character.AccountId = session.Id;

            await DatabaseManager.Character.CreateCharacter(character);
            session.CachedCharacters.Add(new CachedCharacter(guid, (byte)session.CachedCharacters.Count, character.Name, 0));

            SendCharacterCreateResponse(session, 1, guid, character.Name);
        }

        private static void SendCharacterCreateResponse(Session session, uint responseCode, uint guid = 0, string charName = null)
        {
            var charCreateResponse = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            var charCreateFragment = new ServerPacketFragment(9, FragmentOpcode.CharacterCreateResponse);
            if (responseCode == 1)
            {
                charCreateFragment.Payload.Write(responseCode);
                charCreateFragment.Payload.Write(guid);
                charCreateFragment.Payload.WriteString16L(charName);
                charCreateFragment.Payload.Write(0u);
            }
            else
            {
                charCreateFragment.Payload.Write(responseCode);
            }
            charCreateResponse.Fragments.Add(charCreateFragment);
            NetworkManager.SendPacket(ConnectionType.Login, charCreateResponse, session);
        }

    }

}
