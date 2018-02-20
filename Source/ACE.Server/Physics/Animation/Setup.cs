using System.Collections.Generic;
using System.Numerics;

namespace ACE.Server.Physics.Animation
{
    public class Setup
    {
        public int NumParts;
        public List<int> Parts;
        public int ParentIndex;
        public Vector3 DefaultScale;
        public int NumCylsphere;
        public CylSphere CylSphere;
        public int NumSphere;
        public Sphere Sphere;
        public int HasPhysicsBSP;
        public int AllowFreeHeading;
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
        public HashSet<PlacementType> PlacementFrames;
        public int DefaultAnimID;
        public int DefaultScriptID;
        public int DefaultMTableID;
        public int DefaultSTableID;
        public int DefaultPhsTableID;
    }
}
