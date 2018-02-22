using System.Collections.Generic;

namespace ACE.Server.Physics.Common
{
    public class ObjCell
    {
        public WaterType WaterType;
        public Position Pos;
        public int NumObjects;
        public List<PhysicsObj> ObjectList;
        public int NumLights;
        public List<int> LightList;
        public int NumShadowObjects;
        public List<int> ShadowObjectList;
        public int RestrictionObj;
        public List<int> ClipePlanes;
        public int NumStabs;
        public List<int> StabList;
        public int SeenOutside;
        public HashSet<int> VoyeurTable;
        public int MyLandBlock;

        public void RemovePart(PhysicsPart part)
        {

        }
    }
}
