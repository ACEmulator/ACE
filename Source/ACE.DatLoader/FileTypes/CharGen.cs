using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;

namespace ACE.DatLoader.FileTypes
{
    public class CharGen
    {
        public int Did { get; set; }
        public List<List<Loc>> StarterAreas { get; set; } = new List<List<Loc>>();
        public Dictionary<int, HeritageGroupCG> HeritageGroups { get; set; } = new Dictionary<int, HeritageGroupCG>();

        public static CharGen ReadFromDat()
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(0x0E000002))
            {
                return (CharGen)DatManager.PortalDat.FileCache[0x0E000002];
            }
            else
            {
                // Create the datReader for the proper file
                var datReader = DatManager.PortalDat.GetReaderForFile(0x0E000002);
                var cg = new CharGen();

                cg.Did = datReader.ReadInt32();
                datReader.Offset = 8;

                /// STARTER AREAS. There are 5 dungeons per starting city, and one landscape span for Olthoi.
                int numStarterAreas = datReader.ReadByte();
                for (var i = 0; i < numStarterAreas; i++)
                {
                    var starterAreaName = datReader.ReadPString();

                    int numAreas = datReader.ReadByte();
                    var starterAreas = new List<Loc>();
                    for (var j = 0; j < numAreas; j++)
                    {
                        var starterArea = new Loc(datReader.ReadInt32(), datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle());
                        starterAreas.Add(starterArea);
                    }

                    cg.StarterAreas.Add(starterAreas);
                }

                /// HERITAGE GROUPS -- 11 standard player races and 2 Olthoi.
                datReader.Offset++; // Not sure what this byte 0x01 is indicating, but we'll skip it because we can.
                int heritageGroupCount = datReader.ReadByte();
                for (var i = 0; i < heritageGroupCount; i++)
                {
                    var heritage = new HeritageGroupCG();
                    var heritageIndex = datReader.ReadInt32();
                    heritage.Name = datReader.ReadPString();
                    heritage.IconImage = datReader.ReadUInt32();
                    heritage.SetupID = datReader.ReadUInt32();
                    heritage.EnvironmentSetupID = datReader.ReadUInt32();
                    heritage.AttributeCredits = datReader.ReadUInt32();
                    heritage.SkillCredits = datReader.ReadUInt32();

                    // Start Areas correspond go the CG.StarterAreas List.
                    int numPrimaryStartAreas = datReader.ReadByte();
                    for (var j = 0; j < numPrimaryStartAreas; j++)
                        heritage.PrimaryStartAreaList.Add(datReader.ReadInt32());

                    int numSecondaryStartAreas = datReader.ReadByte();
                    for (var j = 0; j < numSecondaryStartAreas; j++)
                        heritage.SecondaryStartAreaList.Add(datReader.ReadInt32());

                    // Racial Skills
                    int skillCount = datReader.ReadByte();
                    for (var j = 0; j < skillCount; j++)
                    {
                        var skill = new SkillCG();
                        skill.SkillNum = datReader.ReadUInt32();
                        skill.NormalCost = datReader.ReadUInt32();
                        skill.PrimaryCost = datReader.ReadUInt32();
                        heritage.SkillList.Add(skill);
                    }

                    // Adventurer, Bow Hunter, etc.
                    int templateCount = datReader.ReadByte();
                    for (var j = 0; j < templateCount; j++)
                    {
                        var template = new TemplateCG();
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
                        for (var k = 0; k < skillCount; k++)
                        {
                            template.NormalSkillsList.Add(datReader.ReadUInt32());
                        }
                        skillCount = datReader.ReadByte();
                        for (var k = 0; k < skillCount; k++)
                        {
                            template.PrimarySkillsList.Add(datReader.ReadUInt32());
                        }

                        heritage.TemplateList.Add(template);
                    }

                    datReader.Offset++; // 0x01 byte here. Not sure what/why, so skip it!
                    int numGenders = datReader.ReadByte(); // this is always 2, but let's read it anyways...
                    for (var j = 0; j < numGenders; j++)
                    {
                        var sex = new SexCG();
                        var genderID = datReader.ReadInt32();
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
                        for (var k = 0; k < numHairColors; k++)
                        {
                            sex.HairColorList.Add(datReader.ReadUInt32());
                        }

                        int numHairStyles = datReader.ReadByte();
                        for (var k = 0; k < numHairStyles; k++)
                        {
                            var hairstyle = new HairStyleCG();
                            hairstyle.IconImage = datReader.ReadUInt32();
                            hairstyle.Bald = Convert.ToBoolean(datReader.ReadByte());
                            hairstyle.AlternateSetup = datReader.ReadUInt32();
                            hairstyle.ObjDesc = ObjDesc.ReadFromDat(ref datReader);
                            sex.HairStyleList.Add(hairstyle);
                        }

                        int numEyeColors = datReader.ReadByte();
                        for (var k = 0; k < numEyeColors; k++)
                            sex.EyeColorList.Add(datReader.ReadUInt32());

                        int numEyeStrips = datReader.ReadByte();
                        for (var k = 0; k < numEyeStrips; k++)
                        {
                            var eyestrip = new EyeStripCG();
                            eyestrip.IconImage = datReader.ReadUInt32();
                            eyestrip.IconImageBald = datReader.ReadUInt32();
                            eyestrip.ObjDesc = ObjDesc.ReadFromDat(ref datReader);
                            eyestrip.ObjDescBald = ObjDesc.ReadFromDat(ref datReader);
                            sex.EyeStripList.Add(eyestrip);
                        }

                        int numNoseStrips = datReader.ReadByte(); // Breathe Right?
                        for (var k = 0; k < numNoseStrips; k++)
                        {
                            var nosestrip = new FaceStripCG();
                            nosestrip.IconImage = datReader.ReadUInt32();
                            nosestrip.ObjDesc = ObjDesc.ReadFromDat(ref datReader);
                            sex.NoseStripList.Add(nosestrip);
                        }

                        int numMouthStrips = datReader.ReadByte(); // Breathe Right?
                        for (var k = 0; k < numMouthStrips; k++)
                        {
                            var mouthstrip = new FaceStripCG();
                            mouthstrip.IconImage = datReader.ReadUInt32();
                            mouthstrip.ObjDesc = ObjDesc.ReadFromDat(ref datReader);
                            sex.MouthStripList.Add(mouthstrip);
                        }

                        int numHeadGear = datReader.ReadByte();
                        for (var k = 0; k < numHeadGear; k++)
                        {
                            var headgear = new GearCG();
                            headgear.Name = datReader.ReadPString();
                            headgear.ClothingTable = datReader.ReadUInt32();
                            headgear.WeenieDefault = datReader.ReadUInt32();
                            sex.HeadgearList.Add(headgear);
                        }

                        int numShirts = datReader.ReadByte();
                        for (var k = 0; k < numShirts; k++)
                        {
                            var shirt = new GearCG();
                            shirt.Name = datReader.ReadPString();
                            shirt.ClothingTable = datReader.ReadUInt32();
                            shirt.WeenieDefault = datReader.ReadUInt32();
                            sex.ShirtList.Add(shirt);
                        }

                        int numPants = datReader.ReadByte();
                        for (var k = 0; k < numPants; k++)
                        {
                            var pants = new GearCG();
                            pants.Name = datReader.ReadPString();
                            pants.ClothingTable = datReader.ReadUInt32();
                            pants.WeenieDefault = datReader.ReadUInt32();
                            sex.PantsList.Add(pants);
                        }

                        int numFootwear = datReader.ReadByte();
                        for (var k = 0; k < numFootwear; k++)
                        {
                            var footwear = new GearCG();
                            footwear.Name = datReader.ReadPString();
                            footwear.ClothingTable = datReader.ReadUInt32();
                            footwear.WeenieDefault = datReader.ReadUInt32();
                            sex.FootwearList.Add(footwear);
                        }

                        int numClothingColors = datReader.ReadByte();
                        for (var k = 0; k < numClothingColors; k++)
                            sex.ClothingColorsList.Add(datReader.ReadUInt32());

                        heritage.SexList.Add(genderID, sex);
                    }

                    cg.HeritageGroups.Add(heritageIndex, heritage);
                }
                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[0x0E000002] = cg;
                return cg;
            }
        }
    }
}
