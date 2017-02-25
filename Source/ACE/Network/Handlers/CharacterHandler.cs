using System;
using System.Linq;
using System.Security.Cryptography;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.Entity;
using ACE.Network.Enum;
using ACE.Network.Managers;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.Packets;

namespace ACE.Network.Handlers
{
    public static class CharacterHandler
    {
        [GameMessageAttribute(GameMessageOpcode.CharacterEnterWorldRequest, SessionState.AuthConnected)]
        public static void CharacterEnterWorldRequest(ClientPacketFragment fragment, Session session)
        {
            session.LoginSession.Enqueue(new GameMessageCharacterEnterWorldServerReady());
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterEnterWorld, SessionState.AuthConnected)]
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

            session.LoginSession.Enqueue(new PacketOutboundReferral(session.WorldConnectionKey));

            session.State = SessionState.WorldLoginRequest;
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterDelete, SessionState.AuthConnected)]
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

            session.LoginSession.Enqueue(new GameMessageCharacterDelete());

            DatabaseManager.Character.DeleteOrRestore(Time.GetUnixTime() + 3600ul, cachedCharacter.Guid.Low);

            var result = await DatabaseManager.Character.GetByAccount(session.Id);
            session.UpdateCachedCharacters(result);
            session.WorldSession.Enqueue(new GameMessageCharacterList(result, session.Account));
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterRestore, SessionState.AuthConnected)]
        public static void CharacterRestore(ClientPacketFragment fragment, Session session)
        {
            ObjectGuid guid = fragment.Payload.ReadGuid();

            var cachedCharacter = session.CachedCharacters.SingleOrDefault(c => c.Guid.Full == guid.Full);
            if (cachedCharacter == null)
                return;

            bool isAvailable = DatabaseManager.Character.IsNameAvailable(cachedCharacter.Name);
            if (!isAvailable)
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);    /* Name already in use. */
                return;
            }
            DatabaseManager.Character.DeleteOrRestore(0, guid.Low);

            session.LoginSession.Enqueue(new GameMessageCharacterRestore(guid, cachedCharacter.Name, 0u));
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterCreate, SessionState.AuthConnected)]
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
            session.LoginSession.Enqueue(new GameMessageCharacterCreateResponse(response, guid, charName));
        }
    }
}
