using System.Collections.Generic;
using System.IO;
using System.Numerics;

using ACE.DatLoader.Entity;
using ACE.Entity.Enum;

namespace ACE.DatLoader.FileTypes
{
    /// <summary>
    /// These are client_portal.dat files starting with 0x02. 
    /// They are basically 3D model descriptions.
    /// </summary>
    /// <remarks>
    /// A big huge thank you to "Pea" for his trailblazing work on decoding this structure. Without his work on this, we might still be decoding models on cave walls.
    /// </remarks>
    [DatFileType(DatFileType.Setup)]
    public class SetupModel : FileType
    {
        public SetupFlags Flags { get; private set; }
        public bool AllowFreeHeading { get; private set; }
        public bool HasPhysicsBSP { get; private set; }
        public List<uint> Parts { get; } = new List<uint>();
        public List<uint> ParentIndex { get; } = new List<uint>();
        public List<Vector3> DefaultScale { get; } = new List<Vector3>();
        public Dictionary<int, LocationType> HoldingLocations { get; } = new Dictionary<int, LocationType>();
        public Dictionary<int, LocationType> ConnectionPoints { get; } = new Dictionary<int, LocationType>();
        public Dictionary<int, PlacementType> PlacementFrames { get; } = new Dictionary<int, PlacementType>();
        public List<CylSphere> CylSpheres { get; } = new List<CylSphere>();
        public List<Sphere> Spheres { get; } = new List<Sphere>();
        public float Height { get; private set; }
        public float Radius { get; private set; }
        public float StepUpHeight { get; private set; }
        public float StepDownHeight { get; private set; }
        public Sphere SortingSphere { get; private set; } = new Sphere();
        public Sphere SelectionSphere { get; private set; } = new Sphere();
        public Dictionary<int, LightInfo> Lights { get; } = new Dictionary<int, LightInfo>();
        public uint DefaultAnimation { get; private set; }
        public uint DefaultScript { get; private set; }
        public uint DefaultMotionTable { get; private set; }
        public uint DefaultSoundTable { get; private set; }
        public uint DefaultScriptTable { get; private set; }

        public override void Unpack(BinaryReader reader)
        {
            Id = reader.ReadUInt32();

            Flags = (SetupFlags)reader.ReadUInt32();

            AllowFreeHeading    = (Flags & SetupFlags.AllowFreeHeading) != 0;
            HasPhysicsBSP       = (Flags & SetupFlags.HasPhysicsBSP) != 0;

            // Get all the GraphicsObjects in this SetupModel. These are all the 01-types.
            uint numParts = reader.ReadUInt32();
            for (int i = 0; i < numParts; i++)
                Parts.Add(reader.ReadUInt32());

            if ((Flags & SetupFlags.HasParent) != 0)
            {
                for (int i = 0; i < numParts; i++)
                    ParentIndex.Add(reader.ReadUInt32());
            }

            if ((Flags & SetupFlags.HasDefaultScale) != 0)
            {
                for (int i = 0; i < numParts; i++)
                    DefaultScale.Add(reader.ReadVector3());
            }

            HoldingLocations.Unpack(reader);
            ConnectionPoints.Unpack(reader);

            int placementsCount = reader.ReadInt32();
            for (int i = 0; i < placementsCount; i++)
            {
                int key = reader.ReadInt32();
                // there is a frame for each Part
                var placementType = new PlacementType();
                placementType.Unpack(reader, (uint)Parts.Count);
                PlacementFrames.Add(key, placementType);
            }

            CylSpheres.Unpack(reader);

            Spheres.Unpack(reader);

            Height          = reader.ReadSingle();
            Radius          = reader.ReadSingle();
            StepUpHeight    = reader.ReadSingle();
            StepDownHeight  = reader.ReadSingle();

            SortingSphere.Unpack(reader);
            SelectionSphere.Unpack(reader);

            Lights.Unpack(reader);

            DefaultAnimation    = reader.ReadUInt32();
            DefaultScript       = reader.ReadUInt32();
            DefaultMotionTable  = reader.ReadUInt32();
            DefaultSoundTable   = reader.ReadUInt32();
            DefaultScriptTable  = reader.ReadUInt32();
        }

        public static SetupModel CreateSimpleSetup()
        {
            var setup = new SetupModel();

            setup.SortingSphere = Sphere.CreateDummySphere();
            setup.SelectionSphere = Sphere.CreateDummySphere();
            setup.AllowFreeHeading = true;

            return setup;
        }
    }
}
