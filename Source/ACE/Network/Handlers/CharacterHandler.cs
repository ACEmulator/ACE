using ACE.Database;
using ACE.Extensions;
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
            ObjectGuid guid = fragment.Payload.ReadGuid();
            string account  = fragment.Payload.ReadString16L();

            if (account != session.Account)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            var cachedCharacter = session.CachedCharacters.SingleOrDefault(c => c.Guid.Full == guid.Full);
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
            referralPacket.Payload.Write(ConfigManager.Host);
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

            DatabaseManager.Character.DeleteOrRestore(Time.GetUnixTime() + 3600ul, cachedCharacter.Guid.Low);

            var result = await DatabaseManager.Character.GetByAccount(session.Id);
            AuthenticationHandler.CharacterListSelectCallback(result, session);
        }

        [Fragment(FragmentOpcode.CharacterRestore, SessionState.AuthConnected)]
        public static void CharacterRestore(ClientPacketFragment fragment, Session session)
        {
            ObjectGuid guid = fragment.Payload.ReadGuid();

            var cachedCharacter = session.CachedCharacters.SingleOrDefault(c => c.Guid.Full == guid.Full);
            if (cachedCharacter == null)
                return;

            bool isAvailable = DatabaseManager.Character.IsNameAvailable(cachedCharacter.Name);
            if (!isAvailable)
            {
                SendCharacterCreateResponse(session, 3);    /* Name already in use. */
                return;
            }
            DatabaseManager.Character.DeleteOrRestore(0, guid.Low);

            var characterRestore         = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            var characterRestoreFragment = new ServerPacketFragment(9, FragmentOpcode.CharacterRestoreResponse);
            characterRestoreFragment.Payload.Write(1u /* Verification OK flag */);
            characterRestoreFragment.Payload.WriteGuid(guid);
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
                return;

            Character character = Character.CreateFromClientFragment(fragment.Payload, session.Id);
            
            // TODO: profanity filter 
            // sendCharacterCreateResponse(session, 4);
            
            bool isAvailable = DatabaseManager.Character.IsNameAvailable(character.Name);
            if (!isAvailable)
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);
                return;
            }

            uint lowGuid = DatabaseManager.Character.GetMaxId();
            character.Id        = lowGuid;
            character.AccountId = session.Id;

            if (!await DatabaseManager.Character.CreateCharacter(character))
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.DatabaseDown);
                return;
            }

            var guid = new ObjectGuid(lowGuid, GuidType.Player);
            session.CachedCharacters.Add(new CachedCharacter(guid, (byte)session.CachedCharacters.Count, character.Name, 0));

            SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Ok, guid, character.Name);
        }

        private static void SendCharacterCreateResponse(Session session, CharacterGenerationVerificationResponse response, ObjectGuid guid = null, string charName = "")
        {
            var charCreateResponse = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            var charCreateFragment = new ServerPacketFragment(9, FragmentOpcode.CharacterCreateResponse);
            charCreateFragment.Payload.Write((uint)response);

            if (response == CharacterGenerationVerificationResponse.Ok)
            {
                charCreateFragment.Payload.WriteGuid(guid);
                charCreateFragment.Payload.WriteString16L(charName);
                charCreateFragment.Payload.Write(0u);
            }

            charCreateResponse.Fragments.Add(charCreateFragment);
            NetworkManager.SendPacket(ConnectionType.Login, charCreateResponse, session);
        }
    }
}
