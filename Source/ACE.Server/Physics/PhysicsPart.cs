using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Collision;

namespace ACE.Server.Physics
{
    public class PhysicsPart
    {
        public float CYpt;
        public Vector3 ViewerHeading;
        public DatLoader.FileTypes.GfxObjDegradeInfo Degrades;
        public int DegLevel;
        public int DegMode;
        public List<GfxObj> GfxObj;
        public Vector3 GfxObjScale;
        public Position Pos;
        public Position DrawPos;
        //public Material Material;
        public List<uint> Surfaces;
        public int OriginalPaletteID;
        public float CurTranslucency;
        public float CurDiffuse;
        public float CurLuminosity;
        public DatLoader.FileTypes.Palette ShiftPal;
        public int CurrentRenderFrameNum;
        public PhysicsObj PhysObj;
        public int PhysObjIndex;
        public BBox BoundingBox;

        public static PhysicsObj PlayerObject;

        public PhysicsPart()
        {
            InitEmpty();
        }

        public PhysicsPart(uint partID)
        {
            InitEmpty();
            SetPart(partID);
        }

        public TransitionState FindObjCollisions(Transition transition)
        {
            if (GfxObj != null && GfxObj[0].PhysicsBSP != null)
            {
                transition.SpherePath.CacheLocalSpaceSphere(Pos, GfxObjScale.Z);
                return GfxObj[0].FindObjCollisions(GfxObj[0], transition, GfxObjScale.Z);
            }
            return TransitionState.OK;  // should be invalid?
        }

        public BBox GetBoundingBox()
        {
            return GfxObj[0].GfxBoundBox;
        }

        public uint GetPhysObjID()
        {
            if (PhysObj == null)
                return 0;

            return PhysObj.ID;
        }

        public void InitEmpty()
        {
            GfxObjScale = new Vector3(1.0f, 1.0f, 1.0f);
            Pos = new Position();
            DrawPos = new Position();
            ViewerHeading = new Vector3(0.0f, 0.0f, 1.0f);
            PhysObjIndex = -1;
            DegMode = 1;
            CYpt = Int16.MaxValue;
        }

        public bool InitObjDescChanges()
        {
            return false;
        }

        public bool IsPartOfPlayerObj()
        {
            return PhysObj.Equals(PlayerObject);
        }

        public bool LoadGfxObjArray(uint rootObjectID/*, GfxObjDegradeInfo newDegrades*/)
        {
            var gfxObj = (DatLoader.FileTypes.GfxObj)DBObj.Get(new QualifiedDataID(6, rootObjectID));
            GfxObj = new List<GfxObj>() { new GfxObj(gfxObj) };
            // degrades omitted
            return GfxObj != null;
        }

        public static PhysicsPart MakePhysicsPart(uint gfxObjID)
        {
            return new PhysicsPart(gfxObjID);
        }

        public static PhysicsPart MakePhysicsPart(PhysicsPart template)
        {
            var part = new PhysicsPart();
            part.MorphToExistingObject(template);
            return part;
        }

        public bool MorphToExistingObject(PhysicsPart template)
        {
            // copy constructor?
            GfxObj = template.GfxObj;   
            GfxObjScale = template.GfxObjScale;
            Pos = template.Pos;
            DrawPos = template.DrawPos;
            OriginalPaletteID = template.OriginalPaletteID;
            // removed surfaces
            return true;
        }

        public void SetNoDraw(bool noDraw)
        {
            // graphics omitted from server
        }

        public bool SetPart(uint gfxObjID)
        {
            return LoadGfxObjArray(gfxObjID);
        }

        public void UpdateViewerDistance()
        {
            // client rendering?
        }
    }
}
