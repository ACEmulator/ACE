using ACE.Entity.Enum;
using System;

namespace ACE.Entity
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CharacterOptions1Attribute : Attribute
    {
        public CharacterOptions1 Option { get; private set; }

        public CharacterOptions1Attribute(CharacterOptions1 option)
        {
            Option = option;
        }
    }
}
