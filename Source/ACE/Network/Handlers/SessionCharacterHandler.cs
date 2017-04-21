using System;
using System.Linq;

using ACE.Common;
using ACE.Common.Extensions;
using ACE.Database;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Network;
using ACE.Network.Enum;
using ACE.Network.GameMessages;
using ACE.Network.GameMessages.Messages;
using ACE.Network.GameEvent.Events;
using ACE.Managers;

namespace ACE.Network
{
    public partial class Session
    {
        [GameMessageAttribute(GameMessageOpcode.CharacterEnterWorldRequest, SessionState.AuthConnected)]
        private void CharacterEnterWorldRequest(ClientMessage message)
        {
            EnqueueSend(new GameMessageCharacterEnterWorldServerReady());
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterEnterWorld, SessionState.AuthConnected)]
        private void CharacterEnterWorld(ClientMessage message)
        {
            ObjectGuid guid = message.Payload.ReadGuid();
            string account = message.Payload.ReadString16L();

            if (account != AccountName)
            {
                SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            var cachedCharacter = AccountCharacters.SingleOrDefault(c => c.Guid.Full == guid.Full);
            if (cachedCharacter == null)
            {
                SendCharacterError(CharacterError.EnterGameCharacterNotOwned);
                return;
            }

            InitSessionForWorldLogin(cachedCharacter);

            State = SessionState.WorldConnected;

            // check the value of the welcome message. Only display it if it is not empty
            if (!String.IsNullOrEmpty(ConfigManager.Config.Server.Welcome))
            {
                EnqueueSend(new GameEventPopupString(this, ConfigManager.Config.Server.Welcome));
            }

            LandblockManager.PlayerEnterWorld(this);
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterDelete, SessionState.AuthConnected)]
        public async void CharacterDelete(ClientMessage message)
        {
            string account = message.Payload.ReadString16L();
            uint characterSlot = message.Payload.ReadUInt32();

            if (account != AccountName)
            {
                SendCharacterError(CharacterError.Delete);
                return;
            }

            var cachedCharacter = AccountCharacters.SingleOrDefault(c => c.SlotId == characterSlot);
            if (cachedCharacter == null)
            {
                SendCharacterError(CharacterError.Delete);
                return;
            }

            // TODO: check if character is already pending removal

            EnqueueSend(new GameMessageCharacterDelete());

            DatabaseManager.Character.DeleteOrRestore(Time.GetUnixTime() + 3600ul, cachedCharacter.Guid.Low);

            var result = await DatabaseManager.Character.GetByAccount(AccountId);
            UpdateCachedCharacters(result);
            EnqueueSend(new GameMessageCharacterList(result, AccountName));
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterRestore, SessionState.AuthConnected)]
        private void CharacterRestore(ClientMessage message)
        {
            ObjectGuid guid = message.Payload.ReadGuid();

            var cachedCharacter = AccountCharacters.SingleOrDefault(c => c.Guid.Full == guid.Full);
            if (cachedCharacter == null)
                return;

            bool isAvailable = DatabaseManager.Character.IsNameAvailable(cachedCharacter.Name);
            if (!isAvailable)
            {
                SendCharacterCreateResponse(this, CharacterGenerationVerificationResponse.NameInUse);    /* Name already in use. */
                return;
            }

            DatabaseManager.Character.DeleteOrRestore(0, guid.Low);

            EnqueueSend(new GameMessageCharacterRestore(guid, cachedCharacter.Name, 0u));
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterCreate, SessionState.AuthConnected)]
        public async void CharacterCreate(ClientMessage message)
        {
            // known issues:
            // 1. getting the "next" character id is not thread-safe

            string account = message.Payload.ReadString16L();
            if (account != AccountName)
                return;

            Character character = Character.CreateFromClientFragment(message.Payload, AccountId);

            // TODO: profanity filter
            // sendCharacterCreateResponse(session, 4);

            bool isAvailable = DatabaseManager.Character.IsNameAvailable(character.Name);
            if (!isAvailable)
            {
                SendCharacterCreateResponse(this, CharacterGenerationVerificationResponse.NameInUse);
                return;
            }

            uint lowGuid = DatabaseManager.Character.GetMaxId();
            character.Id = lowGuid;
            character.AccountId = AccountId;

            if (!await DatabaseManager.Character.CreateCharacter(character))
            {
                SendCharacterCreateResponse(this, CharacterGenerationVerificationResponse.DatabaseDown);
                return;
            }

            CharacterCreateSetDefaultCharacterOptions(character);
            CharacterCreateSetDefaultCharacterPositions(character);
            DatabaseManager.Character.SaveCharacterOptions(character);
            DatabaseManager.Character.InitCharacterPositions(character);

            var guid = new ObjectGuid(lowGuid, GuidType.Player);
            AccountCharacters.Add(new CachedCharacter(guid, (byte)AccountCharacters.Count, character.Name, 0));

            SendCharacterCreateResponse(this, CharacterGenerationVerificationResponse.Ok, guid, character.Name);
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

        private void CharacterCreateSetDefaultCharacterPositions(Character character)
        {
            character.SetCharacterPosition(CharacterPositionExtensions.StartingPosition(character.Id));
        }

        private void SendCharacterCreateResponse(Session session, CharacterGenerationVerificationResponse response, ObjectGuid guid = default(ObjectGuid), string charName = "")
        {
            EnqueueSend(new GameMessageCharacterCreateResponse(response, guid, charName));
        }

        [GameMessageAttribute(GameMessageOpcode.CharacterLogOff, SessionState.WorldConnected)]
        private void CharacterLogOff(ClientMessage message)
        {
            log.DebugFormat("[{0}] Logging off", AccountName);
            SaveSession();
            Player.Logout();
            logOffRequestTime = DateTime.UtcNow;
        }
    }
}
