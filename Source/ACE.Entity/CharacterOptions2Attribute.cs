using ACE.Entity.Enum;
using System;

namespace ACE.Entity
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CharacterOptions2Attribute : Attribute
    {
        public CharacterOptions2 Option { get; private set; }

        public CharacterOptions2Attribute(CharacterOptions2 option)
        {
            Option = option;
        }
    }
}
