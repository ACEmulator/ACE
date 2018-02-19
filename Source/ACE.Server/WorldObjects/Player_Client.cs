using System;

using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;

namespace ACE.Server.WorldObjects
{
    partial class Player
    {
        public bool GetCharacterOption(CharacterOption option)
        {
            if (option.GetCharacterOptions1Attribute() != null)
                return GetCharacterOptions1((CharacterOptions1)Enum.Parse(typeof(CharacterOptions1), option.ToString()));

            return GetCharacterOptions2((CharacterOptions2)Enum.Parse(typeof(CharacterOptions2), option.ToString()));
        }

        public void SetCharacterOption(CharacterOption option, bool value)
        {
            if (option.GetCharacterOptions1Attribute() != null)
                SetCharacterOptions1((CharacterOptions1)Enum.Parse(typeof(CharacterOptions1), option.ToString()), value);
            else
                SetCharacterOptions2((CharacterOptions2)Enum.Parse(typeof(CharacterOptions2), option.ToString()), value);
        }

        public bool GetCharacterOptions1(CharacterOptions1 option)
        {
            return (GetProperty(PropertyInt.CharacterOptions1) & (uint)option) != 0;
        }

        public void SetCharacterOptions1(CharacterOptions1 option, bool value)
        {
            var options = GetProperty(PropertyInt.CharacterOptions1) ?? 0;

            if (value)
                options |= (int)option;
            else
                options &= ~(int)option;

            SetProperty(PropertyInt.CharacterOptions1, options);
        }

        public void SetCharacterOptions1(int value)
        {
            SetProperty(PropertyInt.CharacterOptions1, value);
        }

        public bool GetCharacterOptions2(CharacterOptions2 option)
        {
            return (GetProperty(PropertyInt.CharacterOptions2) & (uint)option) != 0;
        }

        public void SetCharacterOptions2(CharacterOptions2 option, bool value)
        {
            var options = GetProperty(PropertyInt.CharacterOptions2) ?? 0;

            if (value)
                options |= (int)option;
            else
                options &= ~(int)option;

            SetProperty(PropertyInt.CharacterOptions2, options);
        }

        public void SetCharacterOptions2(int value)
        {
            SetProperty(PropertyInt.CharacterOptions2, value);
        }
    }
}
