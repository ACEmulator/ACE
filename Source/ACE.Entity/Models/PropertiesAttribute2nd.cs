using System;

namespace ACE.Entity.Models
{
    public class PropertiesAttribute2nd
    {
        public uint InitLevel { get; set; }
        public uint LevelFromCP { get; set; }
        public uint CPSpent { get; set; }
        public uint CurrentLevel { get; set; }

        public PropertiesAttribute2nd Clone()
        {
            var result = new PropertiesAttribute2nd
            {
                InitLevel = InitLevel,
                LevelFromCP = LevelFromCP,
                CPSpent = CPSpent,
                CurrentLevel = CurrentLevel,
            };

            return result;
        }
    }
}
