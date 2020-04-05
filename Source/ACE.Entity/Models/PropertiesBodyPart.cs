using System;

using ACE.Entity.Enum;

namespace ACE.Entity.Models
{
    public class PropertiesBodyPart
    {
        public DamageType DType { get; set; }
        public int DVal { get; set; }
        public float DVar { get; set; }
        public int BaseArmor { get; set; }
        public int ArmorVsSlash { get; set; }
        public int ArmorVsPierce { get; set; }
        public int ArmorVsBludgeon { get; set; }
        public int ArmorVsCold { get; set; }
        public int ArmorVsFire { get; set; }
        public int ArmorVsAcid { get; set; }
        public int ArmorVsElectric { get; set; }
        public int ArmorVsNether { get; set; }
        public int BH { get; set; }
        public float HLF { get; set; }
        public float MLF { get; set; }
        public float LLF { get; set; }
        public float HRF { get; set; }
        public float MRF { get; set; }
        public float LRF { get; set; }
        public float HLB { get; set; }
        public float MLB { get; set; }
        public float LLB { get; set; }
        public float HRB { get; set; }
        public float MRB { get; set; }
        public float LRB { get; set; }

        public PropertiesBodyPart Clone()
        {
            var result = new PropertiesBodyPart
            {
                DType = DType,
                DVal = DVal,
                DVar = DVar,
                BaseArmor = BaseArmor,
                ArmorVsSlash = ArmorVsSlash,
                ArmorVsPierce = ArmorVsPierce,
                ArmorVsBludgeon = ArmorVsBludgeon,
                ArmorVsCold = ArmorVsCold,
                ArmorVsFire = ArmorVsFire,
                ArmorVsAcid = ArmorVsAcid,
                ArmorVsElectric = ArmorVsElectric,
                ArmorVsNether = ArmorVsNether,
                BH = BH,
                HLF = HLF,
                MLF = MLF,
                LLF = LLF,
                HRF = HRF,
                MRF = MRF,
                LRF = LRF,
                HLB = HLB,
                MLB = MLB,
                LLB = LLB,
                HRB = HRB,
                MRB = MRB,
                LRB = LRB,
            };

            return result;
        }
    }
}
