using System;

using ACE.Entity.Enum;

namespace ACE.Entity
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CharacterOptions2Attribute : Attribute
    {
        public CharacterOptions2 Option { get; }

        public CharacterOptions2Attribute(CharacterOptions2 option)
        {
            Option = option;
        }
    }
}
