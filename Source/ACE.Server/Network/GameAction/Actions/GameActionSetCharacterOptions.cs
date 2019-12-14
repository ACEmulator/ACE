using System.Collections.Generic;

using ACE.Common.Extensions;
using ACE.Entity.Enum;
using ACE.Server.Network.Enum;

using log4net;

namespace ACE.Server.Network.GameAction.Actions
{
    // NOTE: The client doesn't send this packet if the only options that were changed are normally called in the GameActionSetSingleCharacterOption packet.
    // For example, if the user selects/unselects "Auto Repeat Attacks", the GameActionSetSingleCharacterOption packet is sent. Then if the user clicks on Apply
    // this packet (GameActionSetCharacterOptions) will not be sent.
    // On the other hand, if the user selects/unselects "Auto Repeat Attacks", then selects/unselects "Disable Most Weather Effects" (this won't trigger the SetSingleCharacterOption packet),
    // then clicks Apply, this packet (GameActionSetCharacterOptions) WILL be sent.
    // The options that trigger a GameActionSetSingleCharacterOption packet are denoted by having a value set (as in <enum_field> = <val>) in the CharacterOptions enum.

    public static class GameActionSetCharacterOptions
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [GameAction(GameActionType.SetCharacterOptions)]
        public static void Handle(ClientMessage message, Session session)
        {
            if (!session.Player.FirstEnterWorldDone)
            {
                // if a player is stuck in pink bubble state during login,
                // and they press the 'logout' button before first entering world,
                // their client will have not registered their character options from the server yet,
                // and their client will send the default character options upon clicking the logout button,
                // overwriting their custom options on the server with the defaults. this code avoids that situation

                log.Warn($"{session.Player.Name} sent GameAction 0x1A1 - SetCharacterOptions before FirstEnterWorldDone, ignoring...");
                return;
            }

            int characterOptions1Flag;
            int characterOptions2Flag = 0;
            uint spellbookFilters = 0;
            Dictionary<uint, int> desiredComponents = new Dictionary<uint, int>();

            // Thanks to tfarley (aclogview) for guidance on how to parse some of these flags.  The protocol docs are incomplete.

            // Flags
            uint flags = message.Payload.ReadUInt32();

            characterOptions1Flag = message.Payload.ReadInt32();
            session.Player.SetCharacterOptions1(characterOptions1Flag);

            // TODO: Read shortcuts into object so it's available in the Handle method.
            if ((flags & (uint)CharacterOptionDataFlag.Shortcut) != 0)
            {
                uint numShortcuts = message.Payload.ReadUInt32();
                for (int i = 0; i < numShortcuts; i++)
                {
                    message.Payload.ReadInt32(); // index
                    message.Payload.ReadUInt32(); // objectId (guid?)
                    message.Payload.ReadUInt32(); // spellId
                }
            }

            uint numTab1Spells = message.Payload.ReadUInt32();

            if (numTab1Spells > 0)
            {
                var tab1Spells = new uint[numTab1Spells];
                for (int i = 0; i < numTab1Spells; i++)
                    tab1Spells[i] = message.Payload.ReadUInt32();  // SpellID
            }

            // TODO: I think this has been replaced by the "SpellLists8" struct, but we need to verify
            if ((flags & (uint)CharacterOptionDataFlag.MultiSpellList) != 0)
            {
                // Reads in 4 tabs of spells?
                for (int i = 0; i < 4; i++)
                {
                    uint count = message.Payload.ReadUInt32();
                    for (int j = 0; j < count; j++)
                        message.Payload.ReadUInt32(); // spellId
                }
            }

            // TODO: I think this has been replaced by the "SpellLists8" struct, but we need to verify
            if ((flags & (uint)CharacterOptionDataFlag.ExtendedMultiSpellLists) != 0)
            {
                // Reads in 6 tabs of spells?
                for (int i = 0; i < 6; i++)
                {
                    uint count = message.Payload.ReadUInt32();
                    for (int j = 0; j < count; j++)
                        message.Payload.ReadUInt32(); // spellId
                }
            }

            // TODO: Read into an object so it's available to the Handle method
            if ((flags & (uint)CharacterOptionDataFlag.SpellLists8) != 0)
            {
                // Reads in 7 tabs of spells?
                for (int i = 0; i < 7; i++)
                {
                    uint count = message.Payload.ReadUInt32();
                    for (int j = 0; j < count; j++)
                        message.Payload.ReadUInt32(); // spellId
                }
            }

            if ((flags & (uint)CharacterOptionDataFlag.DesiredComps) != 0)
            {
                uint sizeInfo = message.Payload.ReadUInt32(); // sizeInfo
                uint num = sizeInfo & 0xFFFF;
                for (int i = 0; i < num; ++i)
                {
                    desiredComponents.Add(message.Payload.ReadUInt32(), message.Payload.ReadInt32());
                }
            }

            if ((flags & (uint)CharacterOptionDataFlag.SpellbookFilters) != 0)
            {
                spellbookFilters = message.Payload.ReadUInt32();
            }
            else
            {
                spellbookFilters = 0x3FFF;
            }

            if ((flags & (uint)CharacterOptionDataFlag.CharacterOptions2) != 0)
            {
                characterOptions2Flag = message.Payload.ReadInt32();
                session.Player.SetCharacterOptions2(characterOptions2Flag);
            }

            // TODO: Read into an object so it's available in the Handle method.
            if ((flags & (uint)CharacterOptionDataFlag.TimestampFormat) != 0)
            {
                message.Payload.ReadString16L(); // TODO: verify this is correct
            }

            // SquelchList doesn't get used by the client, so should never be set.
            // if ((flags & (uint)CharacterOptionDataFlag.SquelchList) != 0) { }

            // TODO: Read these properly and do something with the values
            // This functionality taken from aclogview.
            if ((flags & (uint)CharacterOptionDataFlag.GenericQualitiesData) != 0)
            {
                // GenericQualitiesData m_pPlayerOptionsData

                // We're not going to use these just yet...
                uint genericQualitiesHeader = message.Payload.ReadUInt32();
                if ((genericQualitiesHeader & (uint)GenericQualitiesPackHeader.Packed_IntStats) != 0)
                {
                    uint sizeInfo = message.Payload.ReadUInt32();
                    uint _currNum = sizeInfo & 0xFFFF;
                    for (int i = 0; i < _currNum; ++i)
                        message.Payload.Skip(8); // 4 bytes for key, 4 for value
                }
                if ((genericQualitiesHeader & (uint)GenericQualitiesPackHeader.Packed_BoolStats) != 0)
                {
                    uint sizeInfo = message.Payload.ReadUInt32();
                    uint _currNum = sizeInfo & 0xFFFF;
                    for (int i = 0; i < _currNum; ++i)
                        message.Payload.Skip(8); // 4 bytes for key, 4 for value
                }
                if ((genericQualitiesHeader & (uint)GenericQualitiesPackHeader.Packed_FloatStats) != 0)
                {
                    uint sizeInfo = message.Payload.ReadUInt32();
                    uint _currNum = sizeInfo & 0xFFFF;
                    for (int i = 0; i < _currNum; ++i)
                    {
                        message.Payload.Skip(4); // 4 bytes for key
                        message.Payload.ReadString16L(); // read the PStringChar
                    }
                }
                if ((genericQualitiesHeader & (uint)GenericQualitiesPackHeader.Packed_StringStats) != 0)
                {
                    uint sizeInfo = message.Payload.ReadUInt32();
                    uint _currNum = sizeInfo & 0xFFFF;
                    for (int i = 0; i < _currNum; ++i)
                        message.Payload.Skip(8); // 4 bytes for key, 4 for value
                }
            }

            // Window / UI Layout, Opacity, etc
            if ((flags & (uint)CharacterOptionDataFlag.GameplayOptions) != 0)
            {
                // This is the  last message... So it should be all that is left.
                int size = (int)(message.Payload.BaseStream.Length - message.Payload.BaseStream.Position);

                byte[] gameplayOptions = new byte[size];
                gameplayOptions = message.Payload.ReadBytes(size);
                session.Player.SetCharacterGameplayOptions(gameplayOptions);
            }
        }
    }
}
