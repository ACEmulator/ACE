using System.Collections.Generic;

using ACE.Common.Extensions;
using ACE.Entity.Enum;

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
        [GameAction(GameActionType.SetCharacterOptions)]
        public static void Handle(ClientMessage message, Session session)
        {
            int characterOptions1Flag = 0;
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

            // TODO: Not sure on the order of the next 3 or how to parse them

            // if ((flags & (uint)CharacterOptionDataFlag.SquelchList) != 0) { }

            // if ((flags & (uint)CharacterOptionDataFlag.GenericQualitiesData) != 0) { }

            // if ((flags & (uint)CharacterOptionDataFlag.GameplayOptions) != 0) { }

            // Set the options on the player object
            Dictionary<CharacterOption, bool> optionValues = new Dictionary<CharacterOption, bool>(); // Have to use a list since I can't change the values of the actual list while enumerating over it.
            foreach (var option in session.Player.CharacterOptions)
            {
                if (option.Key.GetCharacterOptions1Attribute() != null)
                {
                    if (((uint)option.Key.GetCharacterOptions1Attribute().Option & characterOptions1Flag) != 0)
                        optionValues.Add(option.Key, true);
                    else
                        optionValues.Add(option.Key, false);
                }
                else if (option.Key.GetCharacterOptions2Attribute() != null)
                {
                    if (((uint)option.Key.GetCharacterOptions2Attribute().Option & characterOptions2Flag) != 0)
                        optionValues.Add(option.Key, true);
                    else
                        optionValues.Add(option.Key, false);
                }
            }

            foreach (var option in optionValues)
                session.Player.SetCharacterOption(option.Key, option.Value);

            // TODO: Set other options from the packet

            // Save the options
            session.Player.HandleActionSaveCharacter();
        }
    }
}
