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

        // Eyes
        public uint GetEyeTexture(uint eyesStrip, bool isBald)
        {
            ObjDesc eyes;
            if (isBald)
                eyes = EyeStripList[Convert.ToInt32(eyesStrip)].ObjDescBald;
            else
                eyes = EyeStripList[Convert.ToInt32(eyesStrip)].ObjDesc;
            return eyes.TextureChanges[0].NewTexture;
        }
        public uint GetDefaultEyeTexture(uint eyesStrip, bool isBald)
        {
            ObjDesc eyes;
            if (isBald)
                eyes = EyeStripList[Convert.ToInt32(eyesStrip)].ObjDescBald;
            else
                eyes = EyeStripList[Convert.ToInt32(eyesStrip)].ObjDesc;
            return eyes.TextureChanges[0].OldTexture;
        }

        // Nose
        public uint GetNoseTexture(uint noseStrip)
        {
            ObjDesc nose = NoseStripList[Convert.ToInt32(noseStrip)].ObjDesc;
            return nose.TextureChanges[0].NewTexture;
        }
        public uint GetDefaultNoseTexture(uint noseStrip)
        {
            ObjDesc nose = NoseStripList[Convert.ToInt32(noseStrip)].ObjDesc;
            return nose.TextureChanges[0].OldTexture;
        }

        // Mouth
        public uint GetMouthTexture(uint mouthStrip)
        {
            ObjDesc mouth = MouthStripList[Convert.ToInt32(mouthStrip)].ObjDesc;
            return mouth.TextureChanges[0].NewTexture;
        }
        public uint GetDefaultMouthTexture(uint mouthStrip)
        {
            ObjDesc mouth = MouthStripList[Convert.ToInt32(mouthStrip)].ObjDesc;
            return mouth.TextureChanges[0].OldTexture;
        }

        // Hair (Head)
        public uint GetHeadObject(uint hairStyle)
        {
            HairStyleCG hairstyle = HairStyleList[Convert.ToInt32(hairStyle)];
            return hairstyle.ObjDesc.AnimPartChanges[0].PartID;
        }
        public uint GetHairTexture(uint hairStyle)
        {
            HairStyleCG hairstyle = HairStyleList[Convert.ToInt32(hairStyle)];
            return hairstyle.ObjDesc.TextureChanges[0].NewTexture;
        }
        public uint GetDefaultHairTexture(uint hairStyle)
        {
            HairStyleCG hairstyle = HairStyleList[Convert.ToInt32(hairStyle)];
            return hairstyle.ObjDesc.TextureChanges[0].OldTexture;
        }

        // Headgear
        public uint GetHeadgearWeenie(uint headgearStyle)
        {
            return HeadgearList[Convert.ToInt32(headgearStyle)].WeenieDefault;
        }
        public uint GetHeadgearClothingTable(uint headgearStyle)
        {
            return HeadgearList[Convert.ToInt32(headgearStyle)].ClothingTable;
        }

        // Shirt
        public uint GetShirtWeenie(uint shirtStyle)
        {
            return ShirtList[Convert.ToInt32(shirtStyle)].WeenieDefault;
        }
        public uint GetShirtClothingTable(uint shirtStyle)
        {
            return ShirtList[Convert.ToInt32(shirtStyle)].ClothingTable;
        }

        // Pants
        public uint GetPantsWeenie(uint pantsStyle)
        {
            return PantsList[Convert.ToInt32(pantsStyle)].WeenieDefault;
        }
        public uint GetPantsClothingTable(uint pantsStyle)
        {
            return PantsList[Convert.ToInt32(pantsStyle)].ClothingTable;
        }

        // Footwear
        public uint GetFootwearWeenie(uint footwearStyle)
        {
            return FootwearList[Convert.ToInt32(footwearStyle)].WeenieDefault;
        }
        public uint GetFootwearClothingTable(uint footwearStyle)
        {
            return FootwearList[Convert.ToInt32(footwearStyle)].ClothingTable;
        }
    }
}