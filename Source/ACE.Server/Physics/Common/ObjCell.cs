using System.Collections.Generic;
using ACE.Server.Physics.Animation;

namespace ACE.Server.Physics.Common
{
    public class ObjCell
    {
        public int ID;
        public WaterType WaterType;
        public Position Pos;
        public int NumObjects;
        public List<PhysicsObj> ObjectList;
        public int NumLights;
        public List<int> LightList;
        public int NumShadowObjects;
        public List<ShadowObj> ShadowObjectList;
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

        public void hide_object(PhysicsObj obj)
        {

        }

        public void unhide_object(PhysicsObj obj)
        {

        }

        public void AddShadowObject(ShadowObj shadowObj)
        {
            ShadowObjectList.Add(shadowObj);
            NumShadowObjects++;     // can probably replace with .Count
        }

        public void AddPart(PhysicsPart part, int clipPlanes, AFrame frame, int numParts)
        {
            // could go in PartCell
        }
    }
}
