using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Common;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;

namespace ACE.Server.Physics
{
    public class Setup
    {
        public uint ID;
        public int NumParts;
        public List<PhysicsPart> Parts;
        public List<uint> ParentIndex;
        public List<Vector3> DefaultScale;
        public int NumCylsphere;
        public List<CylSphere> CylSphere;
        public int NumSphere;
        public List<Sphere> Sphere;
        public bool HasPhysicsBSP;
        public bool AllowFreeHeading;
        public float Height;
        public float Radius;
        public float StepDownHeight;
        public float StepUpHeight;
        public Sphere SortingSphere;
        public Sphere SelectionSphere;
        public int NumLights;
        public List<int> Lights;
        public Vector3 AnimScale;
        public Dictionary<int, LocationType> HoldingLocations;
        public Dictionary<int, LocationType> ConnectionPoints;
        public Dictionary<int, PlacementType> PlacementFrames;
        public uint DefaultAnimID;
        public uint DefaultScriptID;
        public uint DefaultMTableID;
        public uint DefaultSTableID;
        public uint DefaultPhsTableID;
        public PhysicsObj Owner;
        public uint Bitfield;

        public List<uint> PartIDs;

        public Setup()
        {
            SortingSphere = new Sphere(Vector3.Zero, 0.0f);
            SelectionSphere = new Sphere(Vector3.Zero, 0.0f);
            AnimScale = new Vector3(1.0f, 1.0f, 1.0f);
            AllowFreeHeading = true;

            HoldingLocations = new Dictionary<int, LocationType>();
            PlacementFrames = new Dictionary<int, PlacementType>();
        }

        public Setup(SetupModel setupModel)
        {
            ID = setupModel.Id;
            Bitfield = setupModel.Bitfield;
            AllowFreeHeading = setupModel.AllowFreeHeading;
            HasPhysicsBSP = setupModel.HasPhysicsBSP;
            PartIDs = setupModel.Parts;
            Parts = new List<PhysicsPart>();
            foreach (var partID in PartIDs)
                Parts.Add(new PhysicsPart(partID));
            ParentIndex = setupModel.ParentIndex;
            DefaultScale = setupModel.DefaultScale;
            HoldingLocations = setupModel.HoldingLocations;
            ConnectionPoints = setupModel.ConnectionPoints;
            PlacementFrames = setupModel.PlacementFrames;
            CylSphere = new List<CylSphere>();
            foreach (var cylsphere in setupModel.CylSpheres)
                CylSphere.Add(new CylSphere(cylsphere));
            Sphere = new List<Sphere>();
            foreach (var sphere in setupModel.Spheres)
                Sphere.Add(new Sphere(sphere));
            Height = setupModel.Height;
            Radius = setupModel.Radius;
            StepDownHeight = setupModel.StepDownHeight;
            StepUpHeight = setupModel.StepUpHeight;
            SortingSphere = new Sphere(setupModel.SortingSphere);
            SelectionSphere = new Sphere(setupModel.SelectionSphere);
            // lights
            DefaultAnimID = setupModel.DefaultAnimation;
            DefaultScriptID = setupModel.DefaultScript;
            DefaultMTableID = setupModel.DefaultMotionTable;
            DefaultSTableID = setupModel.DefaultSoundTable;
            DefaultPhsTableID = setupModel.DefaultScriptTable;
            NumParts = PartIDs != null ? PartIDs.Count : 0;
            NumCylsphere = CylSphere != null ? CylSphere.Count : 0;
            NumSphere = Sphere != null ? Sphere.Count : 0;
        }

        public static Setup Get(uint setupID)
        {
            return (Setup)DBObj.Get(new QualifiedDataID(7, setupID));
        }

        public LocationType GetHoldingLocation(int location_idx)
        {
            LocationType locationType = null;
            HoldingLocations.TryGetValue(location_idx, out locationType);
            return locationType;
        }

        public static Setup MakeParticleSetup(int numParticles)
        {
            var setup = new Setup();
            setup.NumParts = numParticles;
            return setup;
        }

        public static Setup MakeSimpleSetup(uint gfxObjID)
        {
            var setup = new Setup();
            setup.NumParts = 1;
            setup.Parts = new List<PhysicsPart>(1);

            var gfxObj = (Collision.GfxObj)DBObj.Get(new QualifiedDataID(6, gfxObjID));
            if (gfxObj != null)
            {
                if (gfxObj.PhysicsSphere != null)
                    setup.SortingSphere = gfxObj.PhysicsSphere;
                else
                    setup.SortingSphere = gfxObj.DrawingSphere;
            }

            var part = new PhysicsPart();
            part.GfxObj = gfxObj;
            setup.Parts.Add(part);

            var placementType = new PlacementType();
            //placementType.AnimFrame.NumParts = 1;
            setup.PlacementFrames.Add((int)gfxObjID, placementType);
            return setup;
        }

        public List<int> GetSubDataIDs()
        {
            // gets a list of all the data IDs from all parts and tables
            return null;
        }
    }
}
