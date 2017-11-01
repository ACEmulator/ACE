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
        public uint Bitfield { get; set; }
        public List<uint> Parts { get; set; } = new List<uint>();
        public List<uint> ParentIndex { get; set; } = new List<uint>();
        public List<AceVector3> DefaultScale { get; set; } = new List<AceVector3>();
        public Dictionary<int, LocationType> HoldingLocations { get; set; } = new Dictionary<int, LocationType>();
        public Dictionary<int, LocationType> ConnectionPoints { get; set; } = new Dictionary<int, LocationType>();
        public Dictionary<int, PlacementType> PlacementFrames { get; set; } = new Dictionary<int, PlacementType>();
        public List<CylSphere> CylSpheres { get; set; } = new List<CylSphere>();
        public List<CSphere> Spheres { get; set; } = new List<CSphere>();
        public float Height { get; set; }
        public float Radius { get; set; }
        public float StepDownHeight { get; set; }
        public float StepUpHeight { get; set; }
        public CSphere SortingSphere { get; set; } = new CSphere();
        public CSphere SelectionSphere { get; set; } = new CSphere();
        public Dictionary<int, LightInfo> Lights { get; set; } = new Dictionary<int, LightInfo>();
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

                m.Bitfield = datReader.ReadUInt32();

                // Get all the GraphicsObjects in this SetupModel. These are all the 01-types.
                uint numParts = datReader.ReadUInt32();
                for (int i = 0; i < numParts; i++)
                {
                    m.Parts.Add(datReader.ReadUInt32());
                }

                if ((m.Bitfield & 1) > 0)
                {
                    for (int i = 0; i < numParts; i++)
                    {
                        m.ParentIndex.Add(datReader.ReadUInt32());
                    }
                }

                if ((m.Bitfield & 2) > 0)
                {
                    for (int i = 0; i < numParts; i++)
                    {
                        m.DefaultScale.Add(new AceVector3(datReader.ReadSingle(), datReader.ReadSingle(), datReader.ReadSingle()));
                    }
                }

               int numHoldingLocations = datReader.ReadInt32();
                if (numHoldingLocations > 0)
                    for (int i = 0; i < numHoldingLocations; i++)
                    {
                        int key = datReader.ReadInt32();
                        m.HoldingLocations.Add(key, LocationType.Read(datReader)); 
                    }

                int numConnectionPoints = datReader.ReadInt32();
                if (numConnectionPoints > 0)
                    for (int i = 0; i < numConnectionPoints; i++)
                    {
                        int key = datReader.ReadInt32();
                        m.ConnectionPoints.Add(key, LocationType.Read(datReader));
                    }

                int placementsCount = datReader.ReadInt32();
                for (int i = 0; i < placementsCount; i++)
                {
                    int key = datReader.ReadInt32();
                    // there is a frame for each Part
                    m.PlacementFrames.Add(key, PlacementType.Read(m.Parts.Count, datReader));
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

                int numLights = datReader.ReadInt32();
                if (numLights > 0)
                    for (int i = 0; i < numLights; i++)
                    {
                        int key = datReader.ReadInt32();
                        m.Lights.Add(key, LightInfo.Read(datReader));
                    }

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
