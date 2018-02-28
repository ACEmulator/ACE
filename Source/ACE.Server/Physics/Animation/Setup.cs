using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class Setup
    {
        public int m_DID;
        public int NumParts;
        public List<PhysicsPart> Parts;
        public int ParentIndex;
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
        public int DefaultAnimID;
        public int DefaultScriptID;
        public int DefaultMTableID;
        public int DefaultSTableID;
        public int DefaultPhsTableID;
        public PhysicsObj Owner;

        public Setup()
        {
            SortingSphere = new Sphere(Vector3.Zero, 0.0f);
            SelectionSphere = new Sphere(Vector3.Zero, 0.0f);
            AnimScale = new Vector3(1.0f, 1.0f, 1.0f);
            AllowFreeHeading = true;

            HoldingLocations = new Dictionary<int, LocationType>();
            PlacementFrames = new Dictionary<int, PlacementType>();
        }

        public static Setup Get(int setupID)
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

        public static Setup MakeSimpleSetup(int gfxObjID)
        {
            var setup = new Setup();
            setup.NumParts = 1;
            setup.Parts = new List<PhysicsPart>(1);

            var gfxObj = GfxObj.Get(gfxObjID);
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
            placementType.AnimFrame.NumParts = 1;
            setup.PlacementFrames.Add(gfxObjID, placementType);
            return setup;
        }

        public List<int> GetSubDataIDs()
        {
            // gets a list of all the data IDs from all parts and tables
            return null;
        }
    }
}
