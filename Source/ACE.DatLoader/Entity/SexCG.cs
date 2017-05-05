using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.Entity
{
    public class SexCG
    {
        public string Name { get; set; }
        public uint Scale { get; set; }
        public uint SetupID { get; set; }
        public uint SoundTable { get; set; }
        public uint IconImage { get; set; }
        public uint BasePalette { get; set; }
        public uint SkinPalSet { get; set; }
        public uint PhysicsTable { get; set; }
        public uint MotionTable { get; set; }
        public uint CombatTable { get; set; }
        public ObjDesc BaseObjDesc { get; set; } = new ObjDesc();
        public List<uint> HairColorList { get; set; } = new List<uint>();
        public List<HairStyleCG> HairStyleList { get; set; } = new List<HairStyleCG>();
        public List<uint> EyeColorList { get; set; } = new List<uint>();
        public List<EyeStripCG> EyeStripList { get; set; } = new List<EyeStripCG>();
        public List<FaceStripCG> NoseStripList { get; set; } = new List<FaceStripCG>();
        public List<FaceStripCG> MouthStripList { get; set; } = new List<FaceStripCG>();
        public List<GearCG> HeadgearList { get; set; } = new List<GearCG>();
        public List<GearCG> ShirtList { get; set; } = new List<GearCG>();
        public List<GearCG> PantsList { get; set; } = new List<GearCG>();
        public List<GearCG> FootwearList { get; set; } = new List<GearCG>();
        public List<uint> ClothingColorsList { get; set; } = new List<uint>();
    }
}
