using System.Collections.Generic;
using System.Numerics;

using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Entity;

namespace ACE.Server.Physics
{
    public class Setup
    {
        // static
        public SetupModel _dat;
        //public uint ID { get => _dat.Id; }
        //public uint Bitfield { get => _setup.Bitfield; }
        //public List<uint> ParentIndex { get => _setup.ParentIndex; }
        //public List<Vector3> DefaultScale { get => _dat.DefaultScale; }
        //public bool HasPhysicsBSP { get => _setup.HasPhysicsBSP; }
        //public bool AllowFreeHeading { get => _dat.AllowFreeHeading; }
        //public float Height { get => _dat.Height; }
        //public float Radius { get => _dat.Radius; }
        //public float StepDownHeight { get => _dat.StepDownHeight; }
        //public float StepUpHeight { get => _dat.StepUpHeight; }
        //public Dictionary<int, LocationType> HoldingLocations { get => _dat.HoldingLocations; }
        //public Dictionary<int, LocationType> ConnectionPoints { get => _setup.ConnectionPoints; }
        //public Dictionary<int, PlacementType> PlacementFrames { get => _dat.PlacementFrames; }
        //public uint DefaultAnimID { get => _dat.DefaultAnimation; }
        //public uint DefaultScriptID { get => _dat.DefaultScript; }
        //public uint DefaultMTableID { get => _dat.DefaultMotionTable; }
        //public uint DefaultSTableID { get => _dat.DefaultSoundTable; }
        //public uint DefaultPhsTableID { get => _dat.DefaultScriptTable; }

        public int NumCylsphere;
        public List<CylSphere> CylSphere;
        public int NumSphere;
        public List<Sphere> Sphere;
        public Sphere SortingSphere;
        public Sphere SelectionSphere;

        // dynamic
        public PhysicsObj Owner;
        public int NumParts;
        public List<uint> PartIDs { get => _dat.Parts; }
        public List<PhysicsPart> Parts;

        public Setup()
        {
            _dat = SetupModel.CreateSimpleSetup();
        }

        public Setup(SetupModel setupModel)
        {
            _dat = setupModel;
            //ID = setupModel.Id;
            //Bitfield = setupModel.Bitfield;
            //AllowFreeHeading = setupModel.AllowFreeHeading;
            //HasPhysicsBSP = setupModel.HasPhysicsBSP;
            //PartIDs = setupModel.Parts;
            Parts = new List<PhysicsPart>();
            foreach (var partID in PartIDs)
                Parts.Add(new PhysicsPart(partID));     // physicspart = static gfxobj + dynamic placement
            //ParentIndex = setupModel.ParentIndex;
            //DefaultScale = setupModel.DefaultScale;
            //HoldingLocations = setupModel.HoldingLocations;
            //ConnectionPoints = setupModel.ConnectionPoints;
            //PlacementFrames = setupModel.PlacementFrames;
            CylSphere = new List<CylSphere>();          // todo: don't create empty lists
            foreach (var cylsphere in setupModel.CylSpheres)
                CylSphere.Add(new CylSphere(cylsphere));
            Sphere = new List<Sphere>();
            foreach (var sphere in setupModel.Spheres)
                Sphere.Add(new Sphere(sphere));
            //Height = setupModel.Height;
            //Radius = setupModel.Radius;
            //StepDownHeight = setupModel.StepDownHeight;
            //StepUpHeight = setupModel.StepUpHeight;
            SortingSphere = new Sphere(setupModel.SortingSphere);
            SelectionSphere = new Sphere(setupModel.SelectionSphere);
            // lights
            //DefaultAnimID = setupModel.DefaultAnimation;
            //DefaultScriptID = setupModel.DefaultScript;
            //DefaultMTableID = setupModel.DefaultMotionTable;
            //DefaultSTableID = setupModel.DefaultSoundTable;
            //DefaultPhsTableID = setupModel.DefaultScriptTable;
            NumParts = PartIDs != null ? PartIDs.Count : 0;
            NumCylsphere = CylSphere != null ? CylSphere.Count : 0;
            NumSphere = Sphere != null ? Sphere.Count : 0;
        }

        public static Setup Get(uint setupID)
        {
            return new Setup(DBObj.GetSetup(setupID));
        }

        public LocationType GetHoldingLocation(int location_idx)
        {
            LocationType locationType = null;
            _dat.HoldingLocations.TryGetValue(location_idx, out locationType);
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

            var gfxObj = GfxObjCache.Get(gfxObjID);
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
            setup._dat.PlacementFrames.Add((int)gfxObjID, placementType);
            return setup;
        }

        public List<int> GetSubDataIDs()
        {
            // gets a list of all the data IDs from all parts and tables
            return null;
        }
    }
}
