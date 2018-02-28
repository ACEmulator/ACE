using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.Entity;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics
{
    /// <summary>
    /// A list of physics parts
    /// </summary>
    public class PartArray
    {
        public uint State;
        public PhysicsObj Owner;
        public Sequence Sequence;
        public MotionTableManager MotionTableManager;
        public Setup Setup;
        public int NumParts;
        public List<PhysicsPart> Parts;
        public Vector3 Scale;
        public AnimFrame LastAnimFrame;

        public void AddPartsShadow(ObjCell objCell, int numShadowParts)
        {
            List<int> clipPlaneList = null;

            if (numShadowParts > 1)
                clipPlaneList = objCell.ClipPlanes;

            for (var i = 0; i < NumParts; i++)
            {
                if (Parts[i] != null)
                    objCell.AddPart(Parts[i], clipPlaneList, objCell.Pos.Frame, numShadowParts);
            }
        }

        public bool AllowsFreeHeading()
        {
            return Setup.AllowFreeHeading;
        }

        public void AnimationDone(bool success)
        {
            if (MotionTableManager != null)
                MotionTableManager.AnimationDone(success);
        }

        public bool CacheHasPhysicsBSP()
        {
            foreach (var part in Parts)
            {
                if (part.GfxObj.PhysicsBSP != null)
                {
                    State |= 0x10000;
                    return true;
                }
            }
            State &= 0xFFFEFFFF;
            return false;
        }

        public void CheckForCompletedMotions()
        {
            if (MotionTableManager != null)
                MotionTableManager.CheckForCompletedMotions();
        }

        public static PartArray CreateMesh(PhysicsObj owner, int setupDID)
        {
            var mesh = new PartArray();
            mesh.Owner = owner;
            mesh.Sequence.SetObject(owner);
            if (!mesh.SetMeshID(setupDID))
                return null;
            mesh.SetPlacementFrame(0x65);
            return mesh;
        }

        public static PartArray CreateParticle(PhysicsObj owner, int numParts, Sphere sortingSphere = null)
        {
            var particle = new PartArray();
            particle.Owner = owner;
            particle.Sequence.SetObject(owner);

            particle.Setup = Setup.MakeParticleSetup(numParts);

            if (particle.Setup == null || !particle.InitParts())
                return null;

            return particle;
        }

        public static PartArray CreateSetup(PhysicsObj owner, int setupDID, bool createParts)
        {
            var setup = new PartArray();
            setup.Owner = owner;
            setup.Sequence.SetObject(owner);
            if (!setup.SetSetupID(setupDID, createParts))
                return null;
            setup.SetPlacementFrame(0x65);
            return setup;
        }

        public void DestroyLights()
        {
            // gfx omitted from server
        }

        public void DestroyParts()
        {
            Parts = null;
            NumParts = 0;
        }

        public void DestroySetup()
        {
            Setup = null;
        }

        public Sequence DoInterpretedMotion(int motion, MovementParameters movementParameters)
        {
            if (MotionTableManager == null)
                return null;    // 7?

            var mvs = new MovementStruct(MovementType.InterpretedCommand, motion, movementParameters);
            return MotionTableManager.PerformMovement(mvs, Sequence);
        }

        public int DoObjDescChanges(int objDesc)
        {
            return -1;
        }

        public int DoObjDescChangesFromDefault(int objDesc)
        {
            return -1;
        }

        public TransitionState FindObjCollisions(Transition transition)
        {
            foreach (var part in Parts)
            {
                var result = part.FindObjCollisions(transition);
                if (result != TransitionState.OK)
                    return result;
            }
            return TransitionState.OK;
        }

        public BBox GetBoundingBox()
        {
            var bbox = new BBox();

            // accumulate from each part
            foreach (var part in Parts)
            {
                var partBox = part.GetBoundingBox();
                partBox.ConvertToGlobal(part.Pos);
                partBox.BuildBoundingBox(bbox);
            }
            return bbox;
        }

        public List<CylSphere> GetCylSphere()
        {
            return Setup.CylSphere;
        }

        public int GetDataID()
        {
            if (Setup.m_DID != 0)
                return Setup.m_DID;

            if (NumParts == 1)
                return Parts[0].GfxObj.ID;

            return 0;
        }

        public float GetHeight()
        {
            return Setup.Height * Scale.Z;
        }

        public int GetNumCylsphere()
        {
            return Setup.NumCylsphere;
        }

        public int GetNumSphere()
        {
            return Setup.NumSphere;
        }

        public float GetRadius()
        {
            return Setup.Radius * Scale.Z;
        }

        public Sphere GetSelectionSphere(Sphere selectionSphere)
        {
            if (Setup == null) return null;
            return new Sphere(selectionSphere.Center * Scale, selectionSphere.Radius * Scale.Z);
        }

        public int GetSetupID()
        {
            if (Setup != null)
                return Setup.m_DID;

            return 0;
        }

        public Sphere GetSortingSphere()
        {
            if (Setup == null)
                return PhysicsGlobals.DefaultSortingSphere;
            else
                return Setup.SortingSphere;
        }

        public List<Sphere> GetSphere()
        {
            return Setup.Sphere;
        }

        public float GetStepDownHeight()
        {
            if (Setup == null) return PhysicsGlobals.DefaultStepHeight;

            return Setup.StepDownHeight * Scale.Z;
        }

        public float GetStepUpHeight()
        {
            if (Setup == null) return PhysicsGlobals.DefaultStepHeight;

            return Setup.StepUpHeight * Scale.Z;
        }

        public void HandleEnterWorld()
        {
            if (MotionTableManager != null)
                MotionTableManager.HandleEnterWorld();
        }

        public void HandleExitWorld()
        {
            if (MotionTableManager != null)
                MotionTableManager.HandleExitWorld();
        }

        public void HandleMovement()
        {
            if (MotionTableManager != null)
                MotionTableManager.UseTime();
        }

        public bool HasAnims()
        {
            return Sequence.HasAnims();
        }

        public void InitDefaults()
        {
            if (Setup.DefaultAnimID != 0)
            {
                Sequence.ClearAnimations();
                var animData = new AnimData();
                animData.AnimId = Setup.DefaultAnimID;
                animData.LowFrame = 0;
                animData.HighFrame = Int32.MaxValue;
                Sequence.AppendAnimation(animData);
                WeenieDesc.Destroy(animData);
            }

            if (Owner != null)
                Owner.InitDefaults(Setup);
        }

        public void InitializeMotionTables()
        {
            if (MotionTableManager != null)
                MotionTableManager.InitializeState(Sequence);
        }

        public void InitLights()
        {
            // lighting omitted for server
        }

        public bool InitObjDescChanges()
        {
            var result = false;
            if (Setup == null) return false;
            foreach (var part in Parts)
            {
                if (part != null && part.InitObjDescChanges())
                    result = true;
            }
            return result;
        }

        public void InitPals()
        {
            // palettes omitted for server
        }

        public bool InitParts()
        {
            NumParts = Setup.NumParts;
            if (NumParts == 0) return false;

            Parts = new List<PhysicsPart>(NumParts);

            for (var i = 0; i < NumParts; i++)
                Parts.Add(null);

            if (Setup.Parts == null) return true;

            var created = 0;
            for (var i = 0; i < NumParts; i++)
            {
                Parts[i] = Setup.Parts[i].MakePhysicsPart();
                if (Parts[i] == null)
                    break;

                created++;
            }

            if (created == NumParts)
            {
                for (var i = 0; i < NumParts; i++)
                {
                    Parts[i].PhysObj = Owner;
                    Parts[i].PhysObjIndex = i;
                }
                if (Setup.DefaultScale != null)
                {
                    for (var i = 0; i < NumParts; i++)
                        Parts[i].GfxObjScale = Setup.DefaultScale[i];
                }
                return true;
            }

            return false;
        }

        public bool MorphToExistingObject(PartArray obj)
        {
            DestroyParts();
            Setup = obj.Setup;
            // add reference?
            Scale = new Vector3(obj.Scale.X, obj.Scale.Y, obj.Scale.Z);
            NumParts = obj.NumParts;
            Parts = new List<PhysicsPart>(obj.NumParts);
            InitPals();
            for (var i = 0; i < NumParts; i++)
            {
                Parts[i] = obj.Parts[i].MakePhysicsPart();
                Parts[i].PhysObj = Owner;
                Parts[i].PhysObjIndex = i;
                // removed palette references
            }
            return true;
        }

        public void RemoveParts(ObjCell cell)
        {
            foreach (var part in Parts)
            {
                if (part != null)
                    cell.RemovePart(part);
            }
        }

        public void SetCellID(int cellID)
        {
            foreach (var part in Parts)
            {
                if (part != null)
                    part.Pos.ObjCellID = cellID;
            }
        }

        public void SetFrame(AFrame frame)
        {
            UpdateParts(frame);
            // remove lights
        }

        public void SetLuminosityInternal(float luminosity)
        {
            // gfx omitted from server
        }

        public void SetLightingInternal(float luminosity, float diffuse)
        {
            // gfx omitted from server
        }

        public bool SetMeshID(int meshDID)
        {
            if (meshDID == 0) return false;
            var setup = Setup.MakeSimpleSetup(meshDID);
            if (setup == null) return false;
            DestroyParts();
            DestroySetup();
            Setup = setup;
            return InitParts();
        }

        public bool SetMotionTableID(int mtableID)
        {
            if (MotionTableManager != null)
            {
                if (MotionTableManager.GetMotionTableID(mtableID) == mtableID)
                    return true;

                MotionTableManager = null;
            }
            if (mtableID == 0) return true;

            MotionTableManager = MotionTableManager.Create(mtableID);
            if (MotionTableManager == null) return false;

            MotionTableManager.SetPhysicsObject(Owner);

            // chat blob?
            return true;
        }

        public void SetNoDrawInternal(bool noDraw)
        {
            if (Setup == null) return;
            foreach (var part in Parts)
            {
                if (part != null)
                    part.SetNoDraw(noDraw);
            }
        }

        public bool SetPart(List<DatLoader.Entity.AnimationPartChange> changes)
        {
            if (Setup == null) return false;

            var success = true;
            foreach (var change in changes)
            {
                var partIdx = change.PartIndex;

                if (partIdx < NumParts)
                {
                    if (Parts[partIdx] != null)
                    {
                        if (Parts[partIdx].SetPart((int)change.PartID))
                            continue;
                    }
                }
                success = false;
            }
            return success;
        }

        public void SetDiffusionInternal(float diff)
        {
            // gfx omitted from server
        }

        public void SetPartDiffusionInternal(int partIdx, float diff)
        {
            // gfx omitted from server
        }

        public bool SetPartLightingInternal(int partIdx, float luminosity, float diffuse)
        {
            return false;
        }


        public void SetPartLuminosityInternal(int partIdx, float luminosity)
        {
            // gfx omitted from server
        }

        public void SetPartTextureVelocityInternal(int partIdx, float du, float dv)
        {
            // gfx omitted from server
        }

        public void SetPartTranslucencyInternal(int partIdx, float translucency)
        {
            // gfx omitted from server
        }

        public bool SetPlacementFrame(int placementID)
        {
            PlacementType placementFrame = null;

            // try to get placementID
            Setup.PlacementFrames.TryGetValue(placementID, out placementFrame);
            if (placementFrame != null)
            {
                Sequence.SetPlacementFrame(placementFrame.AnimFrame, placementID);
                return true;
            }

            // if failed, try to get index 0
            Setup.PlacementFrames.TryGetValue(0, out placementFrame);
            if (placementFrame != null)
            {
                Sequence.SetPlacementFrame(placementFrame.AnimFrame, 0);
                return true;
            }

            // error out
            Sequence.SetPlacementFrame(null, 0);
            return false;
        }

        public bool SetScaleInternal(Vector3 newScale)
        {
            for (var i = 0; i < NumParts; i++)
            {
                var part = Parts[i];
                if (part != null)
                {
                    if (Setup.DefaultScale != null)
                        part.GfxObjScale = Setup.DefaultScale[i] * newScale;
                    else
                        part.GfxObjScale = Scale;
                }
            }
            return true;
        }

        public bool SetSetupID(int setupID, bool createParts)
        {
            if (Setup != null || Setup.m_DID == setupID)
                return true;

            Setup = Setup.Get(setupID);
            if (Setup == null) return false;

            DestroyParts();

            if (createParts && !InitParts()) return false;

            InitDefaults();
            return true;
        }

        public void SetTextureVelocityInternal(float du, float dv)
        {
            // gfx omitted from server
        }

        public void SetTranslucencyInternal(float translucency)
        {
            // gfx omitted from server
        }

        public Sequence StopCompletelyInternal()
        {
            if (MotionTableManager == null) return null;    // 7?
            var mvs = new MovementStruct(MovementType.StopCompletely);
            return MotionTableManager.PerformMovement(mvs, Sequence);
        }

        public Sequence StopInterpretedMotion(int motion, MovementParameters movementParameters)
        {
            if (MotionTableManager == null) return null;    // 7?
            var mvs = new MovementStruct(MovementType.StopInterpretedCommand);
            mvs.Motion = motion;
            mvs.Params = movementParameters;
            return MotionTableManager.PerformMovement(mvs, Sequence);
        }

        public void Update(double quantum, AFrame offsetFrame)
        {
            Sequence.Update(quantum, offsetFrame);
        }

        public void UpdateParts(AFrame frame)
        {
            var curFrame = Sequence.GetCurrAnimFrame();
            if (curFrame == null) return;
            var numParts = Math.Min(curFrame.NumParts, NumParts);
            for (var i = 0; i < numParts; i++)
                Parts[i].Pos.Frame.Combine(frame, curFrame.Frame[i], Scale);
        }

 
        public void UpdateViewerDistance(float cypt, Vector3 heading)
        {
            foreach (var part in Parts) part.UpdateViewerDistance();
        }

        public void calc_cross_cells_static(ObjCell cell, CellArray cellArray)
        {
            cell.find_transit_cells(NumParts, Parts, cellArray);
        }
    }
}
