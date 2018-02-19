using System;

using ACE.Entity.Enum;

namespace ACE.Entity
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CharacterOptions1Attribute : Attribute
    {
        public CharacterOptions1 Option { get; }

        public CharacterOptions1Attribute(CharacterOptions1 option)
        {
            Option = option;
        }
    }
}
