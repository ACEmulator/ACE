using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.DatLoader.FileTypes
{
    public class CharGen
    {
        private static CharGen instance;

        public int Did { get; set; }
        public List<List<Loc>> StarterAreas { get; set; } = new List<List<Loc>>();
        public Dictionary<int, HeritageGroupCG> HeritageGroups { get; set; } = new Dictionary<int, HeritageGroupCG>();

        public static CharGen ReadFromDat(DatReader datReader)
        {
            if (instance == null) // We'll store the CG data into the instance the first time it's loaded. No need to read it multiple times.
            {
                CharGen cg = new CharGen();

                cg.Did = datReader.ReadInt32();
                datReader.Offset = 8;

                /* STARTER AREA */
                int numStarterAreas = datReader.ReadByte();
                for (int i = 0; i < numStarterAreas; i++)
                {
                    string starterAreaName = datReader.ReadPString();

                    int numAreas = datReader.ReadByte();
                    List<Loc> starterAreas = new List<Loc>();
                    for (int j = 0; j < numAreas; j++)
                    {
                        Loc starterArea = new Loc(datReader.ReadInt32(), datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle());
                        starterAreas.Add(starterArea);
                    }

                    cg.StarterAreas.Add(starterAreas);
                }

                /* HERITAGE GROUPS */
                datReader.Offset++; // Not sure what this byte 0x01 is indicating, but we'll skip it because we can.
                int heritageGroupCount = datReader.ReadByte();
                for (int i = 0; i < heritageGroupCount; i++)
                {
                    HeritageGroupCG heritage = new HeritageGroupCG();
                    int heritageIndex = datReader.ReadInt32();
                    heritage.Name = datReader.ReadPString();
                    heritage.IconImage = datReader.ReadUInt32();
                    heritage.SetupID = datReader.ReadUInt32();
                    heritage.EnvironmentSetupID = datReader.ReadUInt32();
                    heritage.AttributeCredits = datReader.ReadUInt32();
                    heritage.SkillCredits = datReader.ReadUInt32();

                    // Start Areas correspond go the CG.StarterAreas List.
                    int numPrimaryStartAreas = datReader.ReadByte();
                    for (int j = 0; j < numPrimaryStartAreas; j++)
                        heritage.PrimaryStartAreaList.Add(datReader.ReadInt32());

                    int numSecondaryStartAreas = datReader.ReadByte();
                    for (int j = 0; j < numSecondaryStartAreas; j++)
                        heritage.SecondaryStartAreaList.Add(datReader.ReadInt32());

                    // Racial Skills
                    int skillCount = datReader.ReadByte();
                    for (int j = 0; j < skillCount; j++)
                    {
                        SkillCG skill = new SkillCG();
                        skill.SkillNum = datReader.ReadUInt32();
                        skill.NormalCost = datReader.ReadUInt32();
                        skill.PrimaryCost = datReader.ReadUInt32();
                        heritage.SkillList.Add(skill);
                    }

                    // Adventurer, Bow Hunter, etc.
                    int templateCount = datReader.ReadByte();
                    for (int j = 0; j < templateCount; j++)
                    {
                        TemplateCG template = new TemplateCG();
                        template.Name = datReader.ReadPString();
                        template.IconImage = datReader.ReadUInt32();
                        template.Title = datReader.ReadUInt32();
                        // Attributes
                        template.Strength = datReader.ReadUInt32();
                        template.Endurance = datReader.ReadUInt32();
                        template.Coordination = datReader.ReadUInt32();
                        template.Quickness = datReader.ReadUInt32();
                        template.Focus = datReader.ReadUInt32();
                        template.Self = datReader.ReadUInt32();

                        skillCount = datReader.ReadByte();
                        for (int k = 0; k < skillCount; k++)
                        {
                            template.NormalSkillsList.Add(datReader.ReadUInt32());
                        }
                        skillCount = datReader.ReadByte();
                        for (int k = 0; k < skillCount; k++)
                        {
                            template.PrimarySkillsList.Add(datReader.ReadUInt32());
                        }

                        heritage.TemplateList.Add(template);
                    }

                    datReader.Offset++; // 0x01 byte here. Not sure what/why, so skip it!
                    int numGenders = datReader.ReadByte(); // this is always 2, but let's read it anyways...
                    for (int j = 0; j < numGenders; j++)
                    {
                        SexCG sex = new SexCG();
                        int genderID = datReader.ReadInt32();
                        sex.Name = datReader.ReadPString();
                        sex.Scale = datReader.ReadUInt32();
                        sex.SetupID = datReader.ReadUInt32();
                        sex.SoundTable = datReader.ReadUInt32();
                        sex.IconImage = datReader.ReadUInt32();
                        sex.BasePalette = datReader.ReadUInt32();
                        sex.SkinPalSet = datReader.ReadUInt32();
                        sex.PhysicsTable = datReader.ReadUInt32();
                        sex.MotionTable = datReader.ReadUInt32();
                        sex.CombatTable = datReader.ReadUInt32();

                        sex.BaseObjDesc = ObjDesc.ReadFromDat(ref datReader);

                        int numHairColors = datReader.ReadByte();
                        for (int k = 0; k < numHairColors; k++)
                        {
                            sex.HairColorList.Add(datReader.ReadUInt32());
                        }

                        int numHairStyles = datReader.ReadByte();
                        for (int k = 0; k < numHairStyles; k++)
                        {
                            HairStyleCG hairstyle = new HairStyleCG();
                            hairstyle.IconImage = datReader.ReadUInt32();
                            hairstyle.Bald = Convert.ToBoolean(datReader.ReadByte());
                            hairstyle.AlternateSetup = datReader.ReadUInt32();
                            hairstyle.ObjDesc = ObjDesc.ReadFromDat(ref datReader);
                            sex.HairStyleList.Add(hairstyle);
                        }

                        int numEyeColors = datReader.ReadByte();
                        for (int k = 0; k < numEyeColors; k++)
                            sex.EyeColorList.Add(datReader.ReadUInt32());

                        int numEyeStrips = datReader.ReadByte();
                        for (int k = 0; k < numEyeStrips; k++)
                        {
                            EyeStripCG eyestrip = new EyeStripCG();
                            eyestrip.IconImage = datReader.ReadUInt32();
                            eyestrip.IconImageBald = datReader.ReadUInt32();
                            eyestrip.ObjDesc = ObjDesc.ReadFromDat(ref datReader);
                            eyestrip.ObjDescBald = ObjDesc.ReadFromDat(ref datReader);
                            sex.EyeStripList.Add(eyestrip);
                        }

                        int numNoseStrips = datReader.ReadByte(); // Breathe Right?
                        for (int k = 0; k < numNoseStrips; k++)
                        {
                            FaceStripCG nosestrip = new FaceStripCG();
                            nosestrip.IconImage = datReader.ReadUInt32();
                            nosestrip.ObjDesc = ObjDesc.ReadFromDat(ref datReader);
                            sex.NoseStripList.Add(nosestrip);
                        }

                        int numMouthStrips = datReader.ReadByte(); // Breathe Right?
                        for (int k = 0; k < numMouthStrips; k++)
                        {
                            FaceStripCG mouthstrip = new FaceStripCG();
                            mouthstrip.IconImage = datReader.ReadUInt32();
                            mouthstrip.ObjDesc = ObjDesc.ReadFromDat(ref datReader);
                            sex.MouthStripList.Add(mouthstrip);
                        }

                        int numHeadGear = datReader.ReadByte();
                        for (int k = 0; k < numHeadGear; k++)
                        {
                            GearCG headgear = new GearCG();
                            headgear.Name = datReader.ReadPString();
                            headgear.ClothingTable = datReader.ReadUInt32();
                            headgear.WeenieDefault = datReader.ReadUInt32();
                            sex.HeadgearList.Add(headgear);
                        }

                        int numShirts = datReader.ReadByte();
                        for (int k = 0; k < numShirts; k++)
                        {
                            GearCG shirt = new GearCG();
                            shirt.Name = datReader.ReadPString();
                            shirt.ClothingTable = datReader.ReadUInt32();
                            shirt.WeenieDefault = datReader.ReadUInt32();
                            sex.ShirtList.Add(shirt);
                        }

                        int numPants = datReader.ReadByte();
                        for (int k = 0; k < numPants; k++)
                        {
                            GearCG pants = new GearCG();
                            pants.Name = datReader.ReadPString();
                            pants.ClothingTable = datReader.ReadUInt32();
                            pants.WeenieDefault = datReader.ReadUInt32();
                            sex.PantsList.Add(pants);
                        }

                        int numFootwear = datReader.ReadByte();
                        for (int k = 0; k < numFootwear; k++)
                        {
                            GearCG footwear = new GearCG();
                            footwear.Name = datReader.ReadPString();
                            footwear.ClothingTable = datReader.ReadUInt32();
                            footwear.WeenieDefault = datReader.ReadUInt32();
                            sex.FootwearList.Add(footwear);
                        }

                        int numClothingColors = datReader.ReadByte();
                        for (int k = 0; k < numClothingColors; k++)
                            sex.ClothingColorsList.Add(datReader.ReadUInt32());

                        heritage.SexList.Add(genderID, sex);
                    }

                    cg.HeritageGroups.Add(heritageIndex, heritage);
                }
                instance = cg;
            }
           
            return instance;
        }
    }
}
