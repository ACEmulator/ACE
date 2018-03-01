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
        public GfxObj GfxObj;
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
        public BoundingBox BoundingBox;

        public static PhysicsObj PlayerObject;

        public PhysicsPart()
        {
            GfxObjScale = new Vector3(1.0f, 1.0f, 1.0f);
            Pos = new Position();
            DrawPos = new Position();
            ViewerHeading = new Vector3(0.0f, 0.0f, 1.0f);
            PhysObjIndex = -1;
            DegMode = 1;
            CYpt = Int16.MaxValue;
        }

        public TransitionState FindObjCollisions(Transition transition)
        {
            if (GfxObj != null && GfxObj.PhysicsBSP != null)
            {
                transition.SpherePath.CacheLocalSpaceSphere(Pos, GfxObjScale.Z);
                return GfxObj.FindObjCollisions(GfxObj, transition, GfxObjScale.Z);
            }
            return TransitionState.Invalid;
        }

        public BBox GetBoundingBox()
        {
            return GfxObj.GfxBoundBox;
        }

        public int GetPhysObjID()
        {
            if (PhysObj == null)
                return 0;

            return PhysObj.ID;
        }

        public bool InitObjDescChanges()
        {
            return false;
        }

        public bool IsPartOfPlayerObj()
        {
            return PhysObj.Equals(PlayerObject);
        }

        public PhysicsPart MakePhysicsPart()
        {
            return null;
        }

        public static PhysicsPart MakePhysicsPart(int gfxObjID)
        {
            var part = new PhysicsPart();
            if (!part.SetPart(gfxObjID))
                return null;
            return part;
        }

        public static PhysicsPart MakePhysicsPart(PhysicsPart template)
        {
            var part = new PhysicsPart();
            part.MorphToExistingObject(template);
            return part;
        }

        public bool MorphToExistingObject(PhysicsPart template)
        {
            GfxObj = template.GfxObj;   // TODO: deep copy
            var scale = new Vector3(template.GfxObjScale.X, template.GfxObjScale.Y, template.GfxObjScale.Z);
            Pos.ObjCellID = template.Pos.ObjCellID;
            // frame copy constructor?
            Pos.Frame = template.Pos.Frame;
            DrawPos.ObjCellID = template.DrawPos.ObjCellID;
            DrawPos.Frame = template.DrawPos.Frame;
            OriginalPaletteID = template.OriginalPaletteID;
            // removed surfaces
            return true;
        }

        public bool SetNoDraw(bool noDraw)
        {
            return false;
        }

        public bool SetPart(int partID)
        {
            return false;
        }

        public void UpdateViewerDistance()
        {

        }
    }
}
