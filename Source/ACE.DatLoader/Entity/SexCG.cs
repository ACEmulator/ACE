using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.Entity
{
    public class SexCG : IUnpackable
    {
        public string Name { get; private set; }
        public uint Scale { get; private set; }
        public uint SetupID { get; private set; }
        public uint SoundTable { get; private set; }
        public uint IconImage { get; private set; }
        public uint BasePalette { get; private set; }
        public uint SkinPalSet { get; private set; }
        public uint PhysicsTable { get; private set; }
        public uint MotionTable { get; private set; }
        public uint CombatTable { get; private set; }
        public ObjDesc BaseObjDesc { get; } = new ObjDesc();
        public List<uint> HairColorList { get; } = new List<uint>();
        public List<HairStyleCG> HairStyleList { get; } = new List<HairStyleCG>();
        public List<uint> EyeColorList { get; } = new List<uint>();
        public List<EyeStripCG> EyeStripList { get; } = new List<EyeStripCG>();
        public List<FaceStripCG> NoseStripList { get; } = new List<FaceStripCG>();
        public List<FaceStripCG> MouthStripList { get; } = new List<FaceStripCG>();
        public List<GearCG> HeadgearList { get; } = new List<GearCG>();
        public List<GearCG> ShirtList { get; } = new List<GearCG>();
        public List<GearCG> PantsList { get; } = new List<GearCG>();
        public List<GearCG> FootwearList { get; } = new List<GearCG>();
        public List<uint> ClothingColorsList { get; } = new List<uint>();

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
        public uint? GetHeadObject(uint hairStyle)
        {
            HairStyleCG hairstyle = HairStyleList[Convert.ToInt32(hairStyle)];

            // Gear Knights, both Olthoi types have multiple anim part changes.
            if (hairstyle.ObjDesc.AnimPartChanges.Count == 1)
                return hairstyle.ObjDesc.AnimPartChanges[0].PartID;
            else
                return null;
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

        public void Unpack(BinaryReader reader)
        {
            Name            = reader.ReadString();
            Scale           = reader.ReadUInt32();
            SetupID         = reader.ReadUInt32();
            SoundTable      = reader.ReadUInt32();
            IconImage       = reader.ReadUInt32();
            BasePalette     = reader.ReadUInt32();
            SkinPalSet      = reader.ReadUInt32();
            PhysicsTable    = reader.ReadUInt32();
            MotionTable     = reader.ReadUInt32();
            CombatTable     = reader.ReadUInt32();

            BaseObjDesc.Unpack(reader);

            HairColorList.UnpackSmartArray(reader);
            HairStyleList.UnpackSmartArray(reader);
            EyeColorList.UnpackSmartArray(reader);
            EyeStripList.UnpackSmartArray(reader);
            NoseStripList.UnpackSmartArray(reader);
            MouthStripList.UnpackSmartArray(reader);

            HeadgearList.UnpackSmartArray(reader);
            ShirtList.UnpackSmartArray(reader);
            PantsList.UnpackSmartArray(reader);
            FootwearList.UnpackSmartArray(reader);
            ClothingColorsList.UnpackSmartArray(reader);
        }
    }
}
