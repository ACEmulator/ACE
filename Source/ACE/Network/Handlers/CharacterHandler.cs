using ACE.Database;
using ACE.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Diagnostics;

namespace ACE.Network
{
    public static class CharacterHandler
    {
        [Fragment(FragmentOpcode.CharacterEnterWorldRequest)]
        public static void CharacterEnterWorldRequest(ClientPacketFragment fragment, Session session)
        {
            var characterEnterWorldServerReady = new ServerPacket(0x0B, PacketHeaderFlags.EncryptedChecksum);
            characterEnterWorldServerReady.Fragments.Add(new ServerPacketFragment(9, FragmentOpcode.CharacterEnterWorldServerReady));

            NetworkManager.SendPacket(ConnectionType.Login, characterEnterWorldServerReady, session);
        }

        [Fragment(FragmentOpcode.CharacterEnterWorld)]
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
            referralPacket.Payload.Write(ConfigManager.Host);
            referralPacket.Payload.Write(0ul);
            referralPacket.Payload.Write((ushort)0x18);
            referralPacket.Payload.Write((ushort)0);
            referralPacket.Payload.Write(0u);

            NetworkManager.SendPacket(ConnectionType.Login, referralPacket, session);
        }

        [Fragment(FragmentOpcode.CharacterDelete)]
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

        [Fragment(FragmentOpcode.CharacterRestore)]
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

        [Fragment(FragmentOpcode.CharacterCreate)]
        public static void CharacterCreate(ClientPacketFragment fragment, Session session)
        {
            string account = fragment.Payload.ReadString16L();

            if (account != session.Account)
            {
                return;
            }

            fragment.Payload.Skip(4);   /* Unknown constant (1) */

            uint race             = fragment.Payload.ReadUInt32();
            uint gender           = fragment.Payload.ReadUInt32();
            uint eyes             = fragment.Payload.ReadUInt32();
            uint nose             = fragment.Payload.ReadUInt32();
            uint mouth            = fragment.Payload.ReadUInt32();
            uint hairColor        = fragment.Payload.ReadUInt32();
            uint eyeColor         = fragment.Payload.ReadUInt32();
            uint hairStyle        = fragment.Payload.ReadUInt32();
            uint headgearStyle    = fragment.Payload.ReadUInt32();
            uint headgearColor    = fragment.Payload.ReadUInt32();
            uint shirtStyle       = fragment.Payload.ReadUInt32();
            uint shirtColor       = fragment.Payload.ReadUInt32();
            uint pantsStyle       = fragment.Payload.ReadUInt32();
            uint pantsColor       = fragment.Payload.ReadUInt32();
            uint footwearStyle    = fragment.Payload.ReadUInt32();
            uint footwearColor    = fragment.Payload.ReadUInt32();
            double skinHue        = fragment.Payload.ReadDouble();
            double hairHue        = fragment.Payload.ReadDouble();
            double headgearHue    = fragment.Payload.ReadDouble();
            double shirtHue       = fragment.Payload.ReadDouble();
            double pantsHue       = fragment.Payload.ReadDouble();
            double footwearHue    = fragment.Payload.ReadDouble();
            uint templateOption   = fragment.Payload.ReadUInt32();
            uint strength         = fragment.Payload.ReadUInt32();
            uint endurance        = fragment.Payload.ReadUInt32();
            uint coordination     = fragment.Payload.ReadUInt32();
            uint quickness        = fragment.Payload.ReadUInt32();
            uint focus            = fragment.Payload.ReadUInt32();
            uint self             = fragment.Payload.ReadUInt32();
            uint slot             = fragment.Payload.ReadUInt32();
            uint classId          = fragment.Payload.ReadUInt32();

            List<NewCharacterSkill> skills = new List<NewCharacterSkill>();

            uint numOfSkills = fragment.Payload.ReadUInt32();
            for (uint i = 0; i < numOfSkills; i++)
            {
                if (new[] {0u, 1u, 2u, 3u, 4u, 5u, 8u, 9u, 10u, 11u, 12u, 13u, 17u, 25u, 26u, 42u, 53u}.Contains(i))   /* Inactive / retired  skills */
                {
                    fragment.Payload.Skip(4);
                    continue;
                }

                var newSkill = new NewCharacterSkill();
                newSkill.Skill = (CharacterSkill) i;
                newSkill.Status = (SkillStatus) fragment.Payload.ReadUInt32();
                skills.Add(newSkill);
            }

            string characterName    = fragment.Payload.ReadString16L();
            uint startArea          = fragment.Payload.ReadUInt32();
            bool isAdmin            = Convert.ToBoolean(fragment.Payload.ReadUInt32());
            bool isEnvoy            = Convert.ToBoolean(fragment.Payload.ReadUInt32());
            uint totalSkillPoints   = fragment.Payload.ReadUInt32();

            // TODO : profanity filter 
            // sendCharacterCreateError(session, 4);

            var isAvailable = DatabaseManager.Character.IsNameAvailable(characterName);
            if (!isAvailable)
            {
                sendCharacterCreateResponse(session, 3);    /* Name already in use. */
                return;
            }

            uint guid = DatabaseManager.Character.GetMaxId();
            DatabaseManager.Character.CreateCharacter(guid, session.Id, characterName, templateOption, startArea, isAdmin, isEnvoy);

            // TODO : Persist appearance, stats and skills.

            session.CachedCharacters.Add(new CachedCharacter(guid, (byte)session.CachedCharacters.Count, characterName, 0));

            sendCharacterCreateResponse(session, 1, guid, characterName);
        }

        private static void sendCharacterCreateResponse(Session session, uint responseCode, uint guid = 0, string charName = null)
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

    public class NewCharacterSkill
    {
        public CharacterSkill Skill { get; set; }
        public SkillStatus Status { get; set; }
    }

    public enum CharacterSkill
    {
        MELEE_DEFENSE = 6,
        MISSILE_DEFENSE = 7,
        ARCANE_LORE = 14,
        MAGIC_DEFENSE = 15,
        MANA_CONVERSION = 16,
        ITEM_TINKERING = 18,
        ASSESS_PERSON = 19,
        DECEPTION = 20,
        HEALING = 21,
        JUMP = 22,
        LOCKPICK = 23,
        RUN = 24,
        ASSESS_CREATURE = 27,
        WEAPON_TINKERING = 28,
        ARMOR_TINKERING = 29,
        MAGIC_ITEM_TINKERING = 30,
        CREATURE_ENHANCEMENT = 31,
        ITEM_ENHANCEMENT = 32,
        LIFE_MAGIC = 33,
        WAR_MAGIC = 34,
        LEADERSHIP = 35,
        LOYALTY = 36,
        FLETCHING = 37,
        ALCHEMY = 38,
        COOKING = 39,
        SALVAGING = 40,
        TWO_HANDED_COMBAT = 41,
        VOID_MAGIC = 43,
        HEAVY_WEAPONS = 44,
        LIGHT_WEAPONS = 45,
        FINESSE_WEAPONS = 46,
        MISSILE_WEAPONS = 47,
        SHIELD = 48,
        DUAL_WIELD = 49,
        RECKLESSNESS = 50,
        SNEAK_ATTACK = 51,
        DIRTY_FIGHTING = 52,
        SUMMONING = 54
    }

    public enum SkillStatus
    {
        INVALID_RETIRED,
        UNTRAINED,
        TRAINED,
        SPECIALIZED 
    }
}
