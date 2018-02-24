using System.Collections.Generic;
using System.Numerics;
using ACE.Entity;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics
{
    public class PartArray
    {
        public static readonly Sphere DefaultSortingSphere;

        public static readonly float DefaultStepHeight = 0.0099999998f;

        public uint PAState;
        public PhysicsObj Owner;
        public Sequence Sequence;
        public MotionTableManager MotionTableManager;
        public Setup Setup;
        public int NumParts;
        public List<PhysicsPart> Parts;
        public Vector3 Scale;
        public AnimFrame LastAnimFrame;
        
        public int AllowsFreeHeading()
        {
            return Setup.AllowFreeHeading;
        }

        public void AnimationDone(int success)
        {
            if (MotionTableManager != null)
                MotionTableManager.AnimationDone(success);
        }

        public bool CacheHasPhysicsBSP()
        {
            if (Parts == null)
            {
                PAState &= 0xFFFEFFFF;
                return false;
            }

            foreach (var part in Parts)
            {
                //foreach (var gfxObj in part.GfxObj)
                //{
                    if (part.GfxObj.PhysicsBSP == null)
                    {
                        PAState &= 0xFFFEFFFF;
                        return false;
                    }
                //}
            }
            PAState |= 0x10000;
            return true;
        }

        public void CheckForCompletedMotions()
        {
            if (MotionTableManager != null)
                MotionTableManager.CheckForCompletedMotions();
        }

        public void CreateMesh(int setupDID)
        {
            // what object is this referencing?
            // var mesh = new Mesh();
        }

        public Particle CreateParticle(PhysicsObj owner, int numParts, Sphere sortingSphere)
        {
            var particle = new Particle();
            var sequence = new Sequence(particle.StartFrame);
            particle.Owner = owner;
            sequence.SetObject(owner);
            var setup = Setup.MakeParticleSetup(numParts);
            // v4?
            if (particle.InitParts())
                return particle;

            return null;
        }

        public Setup CreateSetup(PhysicsObj owner, int setupDID, bool createParts)
        {
            var setup = new Setup();
            var sequence = new Sequence(setup.Parts);
            setup.Owner = owner;
            sequence.SetObject(owner);
            if (SetSetupID(setupDID, createParts))
            {
                //SetPlacementFrame(setup.PlacementFrames);
                return setup;
            }
            return null;
        }

        public void DestroyParts()
        {
            Parts.Clear();
            NumParts = 0;
        }

        public int DoInterpretedMotion(int motion, MovementParameters movementParameters)
        {
            if (MotionTableManager == null) return 7;

            var frame = new Frame();
            frame.Origin = Vector3.Zero;
            frame.Orientation = Quaternion.Identity;    // correct?
            // cache frame
            var mvs = new MovementStruct();
            return MotionTableManager.PerformMovement(mvs, Sequence);   // mvs.Motion?
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

        public void GetBoundingBox()
        {
            // should be building the bounding box for each part?
            foreach (var part in Parts)
            {
                var bbox = new BoundingBox();
                var pBox = part.GetBoundingBox();
                bbox.Min = new Vector3(pBox.Min.X, pBox.Min.Y, pBox.Min.Z);
                bbox.Max = new Vector3(pBox.Max.X, pBox.Max.Y, pBox.Max.Z);
                var maxZ = pBox.Max.Z;
                // get next box?
                bbox.ConvertToGlobal();
                bbox.CalcSize();
            }
        }

        public CylSphere GetCylSphere()
        {
            return Setup.CylSphere;
        }

        public float GetHeight()
        {
            return Setup.Height * Scale.Z;
        }

        public int GetNumCylsphere()
        {
            return Setup.NumCylsphere;
        }

        public float GetRadius()
        {
            return Setup.Radius * Scale.Z;
        }

        public Sphere GetSelectionSphere(Sphere selectionSphere)
        {
            if (Setup == null) return null;

            return new Sphere(new Vector3(selectionSphere.Center.X * Scale.X, selectionSphere.Center.Y * Scale.Y, selectionSphere.Center.Z * Scale.Z), selectionSphere.Radius * Scale.Z);
        }

        public Sphere GetSortingSphere()
        {
            if (Setup == null)
                return DefaultSortingSphere;
            else
                return Setup.SortingSphere;
        }

        public Sphere GetSphere()
        {
            return Setup.Sphere;
        }

        public float GetStepDownHeight()
        {
            if (Setup == null) return DefaultStepHeight;

            return Setup.StepDownHeight * Scale.Z;
        }

        public float GetStepUpHeight()
        {
            if (Setup == null) return DefaultStepHeight;

            return Setup.StepUpHeight * Scale.Z;
        }

        public void HandleMovement()
        {
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
                var defaultAnimId = Setup.DefaultAnimID;
                animData.AnimId = 0;
                animData.LowFrame = -1;
                animData.HighFrame = 1106247680;
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
        }

        public bool InitParts()
        {
            NumParts = Setup.NumParts;
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

        public void SetFrame(Frame frame)
        {
            UpdateParts(frame);
            // remove lights
        }

        public bool SetMeshID(int meshDID)
        {
            if (meshDID == 0) return false;
            var setup = Setup.MakeSimpleSetup(meshDID);
            if (setup == null) return false;
            DestroyParts();
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

            // chat blob?
            return false;
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

        public bool SetPlacementFrame(int placementID)
        {
            PlacementType placementFrame = null;
            Setup.PlacementFrames.TryGetValue(placementID, out placementFrame);
            if (placementFrame != null)
            {
                //while (placementID != placementFrame.AnimFrame)
                // figure out dictionary key iteration
            }
            return false;
        }

        public bool SetScaleInternal(Vector3 newScale)
        {
            // is needed?
            return false;
        }

        public bool SetSetupID(int setupID, bool createParts)
        {
            if (Setup == null || Setup.m_DID != setupID)
            {
                // get new qualified DID?
                DestroyParts();
                if (Setup != null)
                    Setup = null;
                // get Setup from DBObj
                if (createParts)
                {
                    if (!InitParts())
                        return false;
                }
                InitDefaults();
            }
            return true;
        }

        public int StopCompletelyInternal()
        {
            if (MotionTableManager == null) return 7;
            var frame = new Frame();
            frame.Origin = Vector3.Zero;
            frame.Orientation = new Quaternion(0, 0, 0, 1065353216);    // should this be 1/identity?
            // add frame to cache
            var mvs = new MovementStruct();
            return MotionTableManager.PerformMovement(mvs, Sequence);   // how to add frame data?
        }

        public int StopInterpretedMotion(int motion, MovementParameters movementParameters)
        {
            if (MotionTableManager == null) return 7;
            var frame = new Frame();
            frame.Origin = Vector3.Zero;
            frame.Orientation = new Quaternion(0, 0, 0, 1065353216);    // should this be 1/identity?
            // add frame to cache
            var mvs = new MovementStruct();
            return MotionTableManager.PerformMovement(mvs, Sequence);   // how to add frame/input data?
        }

        public void Update(float quantum, Frame offsetFrame)
        {
            Sequence.Update(quantum, offsetFrame);
        }

        public void UpdateParts(Frame frame)
        {
            var animFrame = Sequence.GetCurrAnimFrame();
            if (animFrame == null) return;
            var numParts = NumParts;
            if (numParts > animFrame.NumParts)
                numParts = animFrame.NumParts;
            for (var i = 0; i < numParts; i++)
            {
                Parts[i].Pos.Frame.Combine(frame, animFrame.Frame[i], Scale);
            }
        }
    }
}
