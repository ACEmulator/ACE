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

            DatabaseManager.Character.ExecutePreparedStatement(CharacterPreparedStatement.CharacterDeleteOrRestore, WorldManager.GetUnixTime() + 3600ul, cachedCharacter.LowGuid);

            var result = await DatabaseManager.Character.SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterListSelect, session.Id);
            AuthenticationHandler.CharacterListSelectCallback(result, session);
        }

        [Fragment(FragmentOpcode.CharacterRestore)]
        public static void CharacterRestore(ClientPacketFragment fragment, Session session)
        {
            uint guid = fragment.Payload.ReadUInt32();

            var cachedCharacter = session.CachedCharacters.SingleOrDefault(c => c.LowGuid == guid);
            if (cachedCharacter == null)
                return;

            DatabaseManager.Character.ExecutePreparedStatement(CharacterPreparedStatement.CharacterDeleteOrRestore, 0, guid);

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
        public static async void CharacterCreate(ClientPacketFragment fragment, Session session)
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

            var characterSkills = new List<Tuple<Skill, SkillStatus>>();

            uint numOfSkills = fragment.Payload.ReadUInt32();
            for (uint i = 0; i < numOfSkills; i++)
            {
                var skill = new Tuple<Skill, SkillStatus>((Skill) i, (SkillStatus) fragment.Payload.ReadUInt32());
                characterSkills.Add(skill);
            }

            string characterName    = fragment.Payload.ReadString16L();
            uint startArea          = fragment.Payload.ReadUInt32();
            bool isAdmin            = Convert.ToBoolean(fragment.Payload.ReadUInt32());
            bool isEnvoy            = Convert.ToBoolean(fragment.Payload.ReadUInt32());
            uint totalSkillPoints   = fragment.Payload.ReadUInt32();

            // TODO: profanity filter 
            // sendCharacterCreateResponse(session, 4);

            var result = await DatabaseManager.Character.SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterUniqueNameSelect, characterName);
            Debug.Assert(result != null);

            uint charsWithName = result.Read<uint>(0, "cnt");
            if (charsWithName > 0)
            {
                SendCharacterCreateResponse(session, 3);    /* Name already in use. */
                return;
            }

            result = await DatabaseManager.Character.SelectPreparedStatementAsync(CharacterPreparedStatement.CharacterMaxIndex);
            Debug.Assert(result != null);

            uint guid = result.Read<uint>(0, "MAX(`guid`)") + 1;

            DatabaseManager.Character.ExecutePreparedStatement(CharacterPreparedStatement.CharacterInsert, guid, session.Id, characterName, templateOption, startArea, isAdmin, isEnvoy);
            DatabaseManager.Character.ExecutePreparedStatement(CharacterPreparedStatement.CharacterAppearanceInsert, guid, race, gender, eyes, nose, mouth, eyeColor, hairColor, hairStyle, hairHue, skinHue);
            DatabaseManager.Character.ExecutePreparedStatement(CharacterPreparedStatement.CharacterStatsInsert, guid, strength, endurance, coordination, quickness, focus, self);

            for (int i = 0; i < characterSkills.Count; i++)
            {
                var skill = characterSkills[i];
                DatabaseManager.Character.ExecutePreparedStatement(CharacterPreparedStatement.CharacterSkillsInsert, guid, skill.Item1, skill.Item2, 0u);
            }

            DatabaseManager.Character.ExecutePreparedStatement(CharacterPreparedStatement.CharacterStartupGearInsert, guid, headgearStyle, headgearColor, headgearHue, 
                                                                                                                            shirtStyle, shirtColor, shirtHue, 
                                                                                                                            pantsStyle, pantsColor, pantsHue, 
                                                                                                                            footwearStyle, footwearColor, footwearHue);

            session.CachedCharacters.Add(new CachedCharacter(guid, (byte)session.CachedCharacters.Count, characterName));

            SendCharacterCreateResponse(session, 1, guid, characterName);
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
