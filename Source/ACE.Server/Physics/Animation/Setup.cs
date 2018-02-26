using System.Collections.Generic;
using System.Numerics;
using ACE.DatLoader.Entity;

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
        public CylSphere CylSphere;
        public int NumSphere;
        public Sphere Sphere;
        public int HasPhysicsBSP;
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
        public HashSet<LocationType> HoldingLocations;
        public HashSet<LocationType> ConnectionPoints;
        public Dictionary<int, PlacementType> PlacementFrames;
        public int DefaultAnimID;
        public int DefaultScriptID;
        public int DefaultMTableID;
        public int DefaultSTableID;
        public int DefaultPhsTableID;
        public PhysicsObj Owner;

        public static Setup MakeSimpleSetup(int meshDID)
        {
            return null;
        }

        public static Setup MakeParticleSetup(int numParts)
        {
            return null;
        }

        public static LocationType GetHoldingLocation(int location_idx)
        {
            return null;
        }
    }
}
