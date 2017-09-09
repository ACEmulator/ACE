using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class CombatManeuver
    {
        public uint Style { get; set; }
        public uint AttackHeight { get; set; }
        public uint AttackType { get; set; }
        public uint MinSkillLevel { get; set; }
        public uint Motion { get; set; }

        public static CombatManeuver Read(DatReader datReader)
        {
            CombatManeuver obj = new CombatManeuver();

            obj.Style = datReader.ReadUInt32();
            obj.AttackHeight = datReader.ReadUInt32();
            obj.AttackType = datReader.ReadUInt32();
            obj.MinSkillLevel = datReader.ReadUInt32();
            obj.Motion = datReader.ReadUInt32();

            return obj;
        }
    }
}
