using System;

namespace ACE.Entity.Models
{
    public class PropertiesAttribute
    {
        public uint InitLevel { get; set; }
        public uint LevelFromCP { get; set; }
        public uint CPSpent { get; set; }

        public PropertiesAttribute Clone()
        {
            var result = new PropertiesAttribute
            {
                InitLevel = InitLevel,
                LevelFromCP = LevelFromCP,
                CPSpent = CPSpent,
            };

            return result;
        }
    }
}
