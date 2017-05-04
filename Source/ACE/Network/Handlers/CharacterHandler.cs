using System;
using System.Linq;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.GameEvent.Events;
using ACE.Managers;

namespace ACE.Network.Handlers
{
    public static class CharacterHandler
    {
        [GameMessageAttribute(GameMessageOpcode.CharacterEnterWorldRequest, SessionState.AuthConnected)]
        public static void CharacterEnterWorldRequest(ClientMessage message, Session session)
        {
            session.Network.EnqueueSend(new GameMessageCharacterEnterWorldServerReady());
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterEnterWorld, SessionState.AuthConnected)]
        public static void CharacterEnterWorld(ClientMessage message, Session session)
        {
            ObjectGuid guid = message.Payload.ReadGuid();
            string account = message.Payload.ReadString16L();

            if (account != session.Account)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            var cachedCharacter = session.AccountCharacters.SingleOrDefault(c => c.Guid.Full == guid.Full);
            if (cachedCharacter == null)
            {
                session.SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            session.CharacterRequested = cachedCharacter;

            session.InitSessionForWorldLogin();

            session.State = SessionState.WorldConnected;

            // check the value of the welcome message. Only display it if it is not empty
            if (!String.IsNullOrEmpty(ConfigManager.Config.Server.Welcome))
            {
                session.Network.EnqueueSend(new GameEventPopupString(session, ConfigManager.Config.Server.Welcome));
            }

            LandblockManager.PlayerEnterWorld(session);
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterDelete, SessionState.AuthConnected)]
        public static async void CharacterDelete(ClientMessage message, Session session)
        {
            string account = message.Payload.ReadString16L();
            uint characterSlot = message.Payload.ReadUInt32();

            if (account != session.Account)
            {
                session.SendCharacterError(CharacterError.Delete);
                return;
            }

            var cachedCharacter = session.AccountCharacters.SingleOrDefault(c => c.SlotId == characterSlot);
            if (cachedCharacter == null)
            {
                session.SendCharacterError(CharacterError.Delete);
                return;
            }

            // TODO: check if character is already pending removal

            session.Network.EnqueueSend(new GameMessageCharacterDelete());

            DatabaseManager.Character.DeleteOrRestore(Time.GetUnixTime() + 3600ul, cachedCharacter.Guid.Low);

            var result = await DatabaseManager.Character.GetByAccount(session.Id);
            session.UpdateCachedCharacters(result);
            session.Network.EnqueueSend(new GameMessageCharacterList(result, session.Account));
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterRestore, SessionState.AuthConnected)]
        public static void CharacterRestore(ClientMessage message, Session session)
        {
            ObjectGuid guid = message.Payload.ReadGuid();

            var cachedCharacter = session.AccountCharacters.SingleOrDefault(c => c.Guid.Full == guid.Full);
            if (cachedCharacter == null)
                return;

            bool isAvailable = DatabaseManager.Character.IsNameAvailable(cachedCharacter.Name);
            if (!isAvailable)
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);    /* Name already in use. */
                return;
            }

            DatabaseManager.Character.DeleteOrRestore(0, guid.Low);

            session.Network.EnqueueSend(new GameMessageCharacterRestore(guid, cachedCharacter.Name, 0u));
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterCreate, SessionState.AuthConnected)]
        public static async void CharacterCreate(ClientMessage message, Session session)
        {
            // known issues:
            // 1. getting the "next" character id is not thread-safe

            string account = message.Payload.ReadString16L();
            if (account != session.Account)
                return;

            Character character = Character.CreateFromClientFragment(message.Payload, session.Id);

            // TODO: profanity filter
            // sendCharacterCreateResponse(session, 4);

            bool isAvailable = DatabaseManager.Character.IsNameAvailable(character.Name);
            if (!isAvailable)
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.NameInUse);
                return;
            }

            uint lowGuid = DatabaseManager.Character.GetMaxId();
            character.Id = lowGuid;
            character.AccountId = session.Id;

            if (!await DatabaseManager.Character.CreateCharacter(character))
            {
                SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.DatabaseDown);
                return;
            }

            CharacterCreateSetDefaultCharacterOptions(character);
            CharacterCreateSetDefaultCharacterPositions(character);
            DatabaseManager.Character.SaveCharacterOptions(character);
            DatabaseManager.Character.InitCharacterPositions(character);

            var guid = new ObjectGuid(lowGuid, GuidType.Player);
            session.AccountCharacters.Add(new CachedCharacter(guid, (byte)session.AccountCharacters.Count, character.Name, 0));

            SendCharacterCreateResponse(session, CharacterGenerationVerificationResponse.Ok, guid, character.Name);
        }

        private static void CharacterCreateSetDefaultCharacterOptions(Character character)
        {
            character.SetCharacterOption(CharacterOption.VividTargetingIndicator, true);
            character.SetCharacterOption(CharacterOption.Display3dTooltips, true);
            character.SetCharacterOption(CharacterOption.ShowCoordinatesByTheRadar, true);
            character.SetCharacterOption(CharacterOption.DisplaySpellDurations, true);
            character.SetCharacterOption(CharacterOption.IgnoreFellowshipRequests, true);
            character.SetCharacterOption(CharacterOption.ShareFellowshipExpAndLuminance, true);
            character.SetCharacterOption(CharacterOption.LetOtherPlayersGiveYouItems, true);
            character.SetCharacterOption(CharacterOption.RunAsDefaultMovement, true);
            character.SetCharacterOption(CharacterOption.AutoTarget, true);
            character.SetCharacterOption(CharacterOption.AutoRepeatAttacks, true);
            character.SetCharacterOption(CharacterOption.UseChargeAttack, true);
            character.SetCharacterOption(CharacterOption.LeadMissileTargets, true);
            character.SetCharacterOption(CharacterOption.ListenToAllegianceChat, true);
            character.SetCharacterOption(CharacterOption.ListenToGeneralChat, true);
            character.SetCharacterOption(CharacterOption.ListenToTradeChat, true);
            character.SetCharacterOption(CharacterOption.ListenToLFGChat, true);
        }

        public static void CharacterCreateSetDefaultCharacterPositions(Character character)
        {
            character.SetCharacterPosition(CharacterPositionExtensions.StartingPosition(character.Id));
        }

        private static void SendCharacterCreateResponse(Session session, CharacterGenerationVerificationResponse response, ObjectGuid guid = default(ObjectGuid), string charName = "")
        {
            session.Network.EnqueueSend(new GameMessageCharacterCreateResponse(response, guid, charName));
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterLogOff, SessionState.WorldConnected)]
        public static void CharacterLogOff(ClientMessage message, Session session)
        {
            session.LogOffPlayer();
        }
    }
}
