using System.Collections.Generic;
using System.Numerics;
using ACE.Entity;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Collision;

namespace ACE.Server.Physics
{
    public class PhysicsPart
    {
        // This mostly seems to be used for rendering,
        // and probably doesn't need to be part of the server physics.

        public float CYpt;
        public Vector3 ViewerHeading;
        public DatLoader.FileTypes.GfxObjDegradeInfo Degrades;
        public int DegLevel;
        public int DegMode;
        public GfxObj GfxObj;
        public Vector3 GfxObjScale;
        public Common.Position Pos;
        public Common.Position DrawPos;
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
            GfxObjScale = new Vector3(1.0f, 1.0f, 1.0f); // 0?
            Pos = new Common.Position();
            DrawPos = new Common.Position();
            ViewerHeading = new Vector3(0.0f, 0.0f, 1.0f);
            PhysObjIndex = -1;
            DegMode = 1;
            CYpt = 65535;   // 2139095039
        }

        public BoundingBox GetBoundingBox()
        {
            return BoundingBox;
        }

        public bool IsPartOfPlayerObj()
        {
            return PhysObj.Equals(PlayerObject);
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

        public TransitionState FindObjCollisions(Transition transition)
        {
            if (GfxObj != null && GfxObj.PhysicsBSP != null)
            {
                transition.CacheLocalSpaceSphere(Pos, GfxObjScale.Z);
                return CGfxObj.FindObjCollisions(GfxObj, transition, GfxObjScale.Z);
            }
            return TransitionState.Invalid;
        }

        public int GetPhysObjID()
        {
            if (PhysObj == null) return 0;
            return PhysObj.ID;
        }

        public static PhysicsPart MakePhysicsPart(PhysicsPart template)
        {
            var part = new PhysicsPart();
            part.MorphToExistingObject(template);
            return part;
        }

        public static PhysicsPart MakePhysicsPart(int gfxObjID)
        {
            var part = new PhysicsPart();
            if (!part.SetPart(gfxObjID))
                return null;
            return part;
        }

        public PhysicsPart MakePhysicsPart()
        {
            return null;
        }

        public bool InitObjDescChanges()
        {
            return false;
        }

        public bool SetPart(int partID)
        {
            return false;
        }
    }
}
