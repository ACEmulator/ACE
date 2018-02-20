using System.Collections.Generic;
using System.Numerics;
using ACE.DatLoader.FileTypes;

namespace ACE.Server.Physics
{
    public class PhysicsPart
    {
        public float CYpt;
        public Vector3 ViewerHeading;
        public GfxObjDegradeInfo Degrades;
        public int DegLevel;
        public int DegMode;
        public List<GfxObj> GfxObj;
        public Vector3 GfxObjScale;
        public Vector3 Position;
        public Vector3 DrawPos;
        //public Material Material;
        public List<Surface> Surfaces;
        public int OriginalPaletteID;
        public float CurTranslucency;
        public float CurDiffuse;
        public float CurLuminosity;
        public Palette ShiftPal;
        public int CurrentRenderFrameNum;
        public PhysicsObj PhysObj;
        public int PhysObjIndex;
    }
}
