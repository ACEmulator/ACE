using ACE.Entity;
using ACE.DatLoader.Entity;
using System;
using System.Collections.Generic;
using System.IO;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x02. 
    /// They are basically 3D model descriptions.
    /// </summary>
    /// <remarks>
    /// A big huge thank you to "Pea" for his trailblazing work on decoding this structure. Without his work on this, we might still be decoding models on cave walls.
    /// </remarks>
    public class SetupModel
    {
        public uint ModelId { get; set; }
        public uint TypeId { get; set; }
        public bool Type4 { get; set; }
        public bool Type8 { get; set; }
        public List<uint> SubObjectIds { get; set; } = new List<uint>();
        public List<uint> ObjectUnks { get; set; } = new List<uint>();
        public List<AceVector3> ObjectScales { get; set; } = new List<AceVector3>();
        public int LT94Count { get; set; }
        public int LT98Count { get; set; }
        public List<CylSphere> CylSpheres { get; set; } = new List<CylSphere>();
        public List<CSphere> Spheres { get; set; } = new List<CSphere>();
        public float Height { get; set; }
        public float Radius { get; set; }
        public float StepDownHeight { get; set; }
        public float StepUpHeight { get; set; }
        public CSphere SortingSphere { get; set; } = new CSphere();
        public CSphere SelectionSphere { get; set; } = new CSphere();
        public uint DefaultAnimation { get; set; }
        public uint DefaultScript { get; set; }
        public uint DefaultMotionTable { get; set; }
        public uint DefaultSoundTable { get; set; }
        public uint DefaultScriptTable { get; set; }

        public static SetupModel ReadFromDat(uint fileId)
        {
            // Check the FileCache so we don't need to hit the FileSystem repeatedly
            if (DatManager.PortalDat.FileCache.ContainsKey(fileId))
            {
                return (SetupModel)DatManager.PortalDat.FileCache[fileId];
            }
            else
            {
                DatReader datReader = DatManager.PortalDat.GetReaderForFile(fileId);
                SetupModel m = new SetupModel();
                m.ModelId = datReader.ReadUInt32();

                m.TypeId = datReader.ReadUInt32();

                if ((m.TypeId & 4) == 1)
                    m.Type4 = true;
                else
                    m.Type4 = false;

                if ((m.TypeId & 8) == 1)
                    m.Type8 = true;
                else
                    m.Type8 = false;

                // Get all the GraphicsObjects in this SetupModel. These are all the 01-types.
                uint numSubObjects = datReader.ReadUInt32();
                for (int i = 0; i < numSubObjects; i++)
                {
                    m.SubObjectIds.Add(datReader.ReadUInt32());
                }

                if ((m.TypeId & 1) == 1)
                {
                    for (int i = 0; i < numSubObjects; i++)
                    {
                        m.ObjectUnks.Add(datReader.ReadUInt32());
                    }
                }

                if ((m.TypeId & 2) == 1)
                {
                    for (int i = 0; i < numSubObjects; i++)
                    {
                        m.ObjectScales.Add(new AceVector3(datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle()));
                    }
                }

                m.LT94Count = datReader.ReadInt32();
                datReader.Offset += (9 * 4 * m.LT94Count); // skip over the LT94 Objects. Some sort of location/positional data.

                m.LT98Count = datReader.ReadInt32();
                datReader.Offset += (9 * 4 * m.LT98Count); // skip over the LT98 Objects. Some other sort of location/positional data.

                int placementsCount = datReader.ReadInt32();
                for (int i = 0; i < placementsCount; i++)
                {
                    uint key = datReader.ReadUInt32();
                    // there is a frame for each SubObject
                    datReader.Offset += m.SubObjectIds.Count * 7 * 4; // This is frame data. Vector3 + Quaternion.

                    uint hookCount = datReader.ReadUInt32();
                    for (int j = 0; i < hookCount; j++)
                    {
                        // TODO: Handle this! HOWEVER, no objects appear to actually use this. At least none currently parsed as of 2017-03-29.
                    }
                }

                int cylinderSphereCount = datReader.ReadInt32();
                for (int i = 0; i < cylinderSphereCount; i++)
                {
                    // Sphere is a Vector3 origin + float radius
                    AceVector3 origin = new AceVector3(datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle());
                    m.CylSpheres.Add(new CylSphere(origin, datReader.ReadSingle(), datReader.ReadSingle()));
                }

                int sphereCount = datReader.ReadInt32();
                for (int i = 0; i < sphereCount; i++)
                {
                    // Sphere is a Vector3 origin + float radius
                    AceVector3 origin = new AceVector3(datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle());
                    m.Spheres.Add(new CSphere(origin, datReader.ReadSingle()));
                }

                m.Height = datReader.ReadSingle();
                m.Radius = datReader.ReadSingle();
                m.StepDownHeight = datReader.ReadSingle();
                m.StepUpHeight = datReader.ReadSingle();

                m.SortingSphere = new CSphere(new AceVector3(datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle()), datReader.ReadSingle());
                m.SelectionSphere = new CSphere(new AceVector3(datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle()), datReader.ReadSingle());

                int lightCount = datReader.ReadInt32();
                // Light is made up of a Int32 uknown, Frame (Vector3 + Quaternion), int32 RGBColor, float unknown, float uknown and int32 unknown
                datReader.Offset += 12 * 4 * lightCount;

                m.DefaultAnimation = datReader.ReadUInt32();
                m.DefaultScript = datReader.ReadUInt32();
                m.DefaultMotionTable = datReader.ReadUInt32();
                m.DefaultSoundTable = datReader.ReadUInt32();
                m.DefaultScriptTable = datReader.ReadUInt32();

                // Store this object in the FileCache
                DatManager.PortalDat.FileCache[fileId] = m;
                return m;
            }
        }
    }
}
