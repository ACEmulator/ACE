using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.Entity.Enum;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Combat;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Hooks;
using ACE.Server.Physics.Sound;

using AnimData = ACE.Entity.AnimData;
using Random = ACE.Server.Physics.Common.Random;

namespace ACE.Server.Physics
{
    /// <summary>
    /// The base class for all physics objects
    /// </summary>
    public class PhysicsObj
    {
        public int ID;
        public PartArray PartArray;
        public Vector3 PlayerVector;
        public float PlayerDistance;
        public float CYpt;
        public SoundTable SoundTable;
        public bool ExaminationObject;
        public ScriptManager ScriptManager;
        public PhysicsScriptTable PhysicsScriptTable;
        public PlayScript DefaultScript;
        public float DefaultScriptIntensity;
        public PhysicsObj Parent;
        public ChildList Children;
        public Position Position;
        public ObjCell Cell;
        public int NumShadowObjects;
        public Dictionary<int, ShadowObj> ShadowObjects;
        public PhysicsState State;
        public TransientStateFlags TransientState;
        public float Elasticity;
        public float Translucency;
        public float TranslucencyOriginal;
        public float Friction;
        public float MassInv;
        public MovementManager MovementManager;
        public PositionManager PositionManager;
        public bool LastMoveWasAutonomous;
        public bool JumpedThisFrame;
        public double UpdateTime;
        public Vector3 Velocity;
        public Vector3 Acceleration;
        public Vector3 Omega;
        public LinkedList<PhysicsObjHook> Hooks;
        public List<AnimHook> AnimHooks;
        public float Scale;
        public float AttackRadius;
        public DetectionManager DetectionManager;
        public AttackManager AttackManager;
        public TargetManager TargetManager;
        public ParticleManager ParticleManager;
        public WeenieObject WeenieObj;
        public Plane ContactPlane;
        public int ContactPlaneCellID;
        public Vector3 SlidingNormal;
        public Vector3 CachedVelocity;
        public Dictionary<long, CollisionRecord> CollisionTable;
        public bool CollidingWithEnvironment;
        public int[] UpdateTimes;

        public static CellArray CellArray;
        public static ObjectMaint ObjMaint;
        public static PhysicsObj PlayerObject;

        public static readonly int UpdateTimeLength = 9;

        public PhysicsObj()
        {
            PlayerVector = new Vector3(0, 0, 1);
            PlayerDistance = float.MaxValue;
            CYpt = float.MaxValue;
            Elasticity = PhysicsGlobals.DefaultElasticity;
            Translucency = PhysicsGlobals.DefaultTranslucency;
            Friction = PhysicsGlobals.DefaultFriction;
            State = PhysicsGlobals.DefaultState;
            TranslucencyOriginal = PhysicsGlobals.DefaultTranslucency;
            MassInv = 1.0f / PhysicsGlobals.DefaultMass;
            Velocity = Vector3.Zero;
            Acceleration = Vector3.Zero;
            Omega = Vector3.Zero;
            Scale = PhysicsGlobals.DefaultScale;
            SlidingNormal = Vector3.Zero;
            CachedVelocity = Vector3.Zero;
            UpdateTimes = new int[UpdateTimeLength];
        }

        ~PhysicsObj()
        {
            Destroy();
        }

        public void Destroy()
        {
            if (State.HasFlag(PhysicsState.Static) && (State.HasFlag(PhysicsState.HasDefaultAnim) || State.HasFlag(PhysicsState.HasDefaultScript)))
                RemoveStaticAnimatingObject();

            if (PhysicsScriptTable != null)
                PhysicsScriptTable.Release();

            // PartArray.SetCellID ?

            State = PhysicsGlobals.DefaultState;
        }

        public void AddObjectToSingleCell(ObjCell objCell)
        {
            if (Cell != null)
            {
                if (Cell.Equals(objCell)) return;
                RemoveObjectFromSingleCell(Cell);
            }

            change_cell(objCell);

            if (objCell == null) return;

            NumShadowObjects = 1;
            var shadowObj = new ShadowObj(this, objCell);
            objCell.AddShadowObject(shadowObj);

            if (PartArray != null)
                PartArray.AddPartsShadow(shadowObj);
        }

        public void AddPartToShadowCells(PhysicsPart part)
        {
            if (Cell != null) part.Pos.ObjCellID = Cell.ID;
            foreach (var shadowObj in ShadowObjects.Values)
            {
                var shadowCell = shadowObj.Cell;
                if (shadowCell != null)
                {
                    shadowCell.AddPart(part, 0,
                        shadowCell.Pos.Frame, ShadowObjects.Count);
                }
            }
        }

        public ObjCell AdjustPosition(Position pos, Vector3 low_pt, bool dontCreateCells, bool searchCells)
        {
            return null;
        }

        public void CacheHasPhysicsBSP()
        {

        }

        public void CallPES(List<int> pes, double delta)    // long double?
        {

        }

        public void CallPESInternal(int pes, float curValue)
        {

        }

        public void CheckForCompletedMotions()
        {

        }

        public bool CheckPositionInternal(ObjCell newCell, Position newPos, Transition transition, SetPosition setPos)
        {
            return true;
        }

        public void ConstrainTo(Position pos, float startDistance, float maxDistance)
        {

        }

        public InterpretedMotionState DoInterpretedMotion(int motion, MovementParameters movementParams)
        {
            return null;
        }

        public int DoMotion(int motion, MovementParameters movementParams)
        {
            return -1;
        }

        public int DoObjDescChanges(int objDesc)
        {
            return -1;
        }

        public int DoObjDescChangesFromDefault(int objDesc)
        {
            return -1;
        }

        public void DrawRecursive()
        {

        }

        public int FindObjCollisions(Transition transition)
        {
            return -1;
        }

        public SetPositionError ForceIntoCell(ObjCell newCell, Position pos)
        {
            return SetPositionError.OK;
        }

        public double GetAutonomyBlipDistance()
        {
            if ((Position.ObjCellID & 0xFFFF) < 0x100) return 100.0f;

            return PlayerObject.Equals(this) ? 25.0f : 20.0f;
        }

        public BoundingBox GetBoundingBox()
        {
            if (PartArray == null) return null;
            return PartArray.GetBoundingBox();
        }

        public int GetDataID()
        {
            if (PartArray == null) return 0;
            return PartArray.GetDataID();
        }

        public double GetHeight()
        {
            if (PartArray == null) return 0.0f;
            return PartArray.GetHeight();
        }

        public double GetMaxConstraintDistance()
        {
            return (Position.ObjCellID & 0xFFFF) < 0x100 ? 50.0f : 20.0f;
        }

        public PhysicsObj GetObjectA(int objectID)
        {
            if (ObjMaint == null) return null;
            return ObjMaint.GetObjectA(objectID);
        }

        public double GetRadius()
        {
            if (PartArray != null)
                return PartArray.GetRadius();
            else
                return 0;
        }

        public Sphere GetSelectionSphere(Sphere selectionSphere)
        {
            if (PartArray != null)
                return PartArray.GetSelectionSphere(selectionSphere);
            else
                return null;
        }

        public int GetSetupID()
        {
            if (PartArray == null) return 0;
            return PartArray.GetSetupID();
        }

        public double GetStartConstraintDistance()
        {
            return (Position.ObjCellID & 0xFFFF) < 0x100 ? 5.0f : 10.0f;
        }

        public double GetStepDownHeight()
        {
            if (PartArray == null) return 0;
            return PartArray.GetStepDownHeight();
        }

        public double GetStepUpHeight()
        {
            if (PartArray == null) return 0;
            return PartArray.GetStepUpHeight();
        }

        public void HandleUpdateTarget(TargetInfo targetInfo)
        {

        }

        public bool HasAnims()
        {
            return false;
        }

        public void Hook_AnimDone()
        {

        }

        /// <summary>
        /// Initializes the physics object from defaults in setup
        /// </summary>
        public void InitDefaults(Setup setup)
        {
            if (setup.DefaultScriptID != 0)
                play_script_internal(setup.DefaultScriptID);

            if (setup.DefaultMTableID != 0)
                SetMotionTableID(setup.DefaultMTableID);

            if (setup.DefaultSTableID != 0)
            {
                var qdid = new QualifiedDataID(setup.DefaultSTableID, 0x22);
                SoundTable = (SoundTable)DBObj.Get(qdid);
            }

            if (setup.DefaultPhsTableID != 0)
            {
                var qdid = new QualifiedDataID(setup.DefaultPhsTableID, 0x2C);
                PhysicsScriptTable = (PhysicsScriptTable)DBObj.Get(qdid);
            }

            if (State.HasFlag(PhysicsState.Static))
            {
                if (setup.DefaultAnimID != 0)
                    State |= PhysicsState.HasDefaultAnim;

                if (setup.DefaultScriptID != 0)
                    State |= PhysicsState.HasDefaultScript;

                Physics.AddStaticAnimatingObject(this);
            }
        }

        /// <summary>
        /// Initializes the PartArray from input dataDID
        /// </summary>
        public bool InitNullObject(int dataDID)
        {
            bool createdParts;
            if (PartArray != null)
                createdParts = PartArray.SetSetupID(dataDID, true);
            else
                createdParts = InitPartArrayObject(dataDID, true);

            if (createdParts)
            {
                if (PartArray != null)
                {
                    PartArray.SetPlacementFrame(0x65);
                    if (!State.HasFlag(PhysicsState.IgnoreCollisions))
                        PartArray.SetFrame(Position.Frame);
                }
                return true;
            }

            return createdParts;
        }

        /// <summary>
        /// Initializes a static or dynamic object from input ID
        /// </summary>
        public bool InitObjectBegin(int objectIID, bool dynamic)
        {
            ID = objectIID;

            if (!dynamic)
                State |= PhysicsState.Static;
            else
                State &= ~PhysicsState.Static;

            TransientState &= ~TransientStateFlags.Active;
            UpdateTime = Timer.CurrentTime;

            return true;
        }

        /// <summary>
        /// Sets the placement frame for a part frame
        /// </summary>
        public bool InitObjectEnd()
        {
            if (PartArray != null)
            {
                PartArray.SetPlacementFrame(0x65);

                if (!State.HasFlag(PhysicsState.ParticleEmitter))
                    PartArray.SetFrame(Position.Frame);
            }
            return true;
        }

        /// <summary>
        /// Initializes a new PartArray from a dataDID
        /// </summary>
        public bool InitPartArrayObject(int dataDID, bool createParts)
        {
            if (dataDID == 0) return false;     // stru_843D84
            var ethereal = false;

            var divineType = MasterDBMap.DivineType(dataDID);
            if (divineType == 6)
            {
                PartArray = PartArray.CreateMesh(dataDID);
            }
            else
            {
                if (divineType != 7)
                {
                    if ((dataDID & 0xFF000000) != 0)
                        return false;

                    ethereal = true;

                    if (!makeAnimObject(dataDID | 0x2000000, createParts))
                        return false;
                }

                if (!ethereal)
                    PartArray = PartArray.CreateSetup(this, dataDID, createParts);
            }

            if (PartArray == null) return false;
            CacheHasPhysicsBSP();
            if (ethereal)
            {
                State |= PhysicsState.Ethereal;
                TransientState &= ~TransientStateFlags.CheckEthereal;
                SetTranslucencyInternal(0.25f);
                State |= PhysicsState.IgnoreCollisions;
            }

            return true;
        }

        /// <summary>
        /// Initializes the motion tables for the physics parts
        /// </summary>
        public void InitializeMotionTables()
        {
            if (PartArray != null)
                PartArray.InitializeMotionTables();
        }

        public InterpretedMotionState InqInterpretedMotionState()
        {
            return null;
        }

        public RawMotionState InqRawMotionState()
        {
            return null;
        }

        public void InterpolateTo(Position p, bool keepHeading)
        {

        }

        public bool IsFullyConstrained()
        {
            return false;
        }

        public bool IsInterpolating()
        {
            return false;
        }

        public bool IsMovingTo()
        {
            return false;
        }

        public void MakeMovementManager(bool init_motion)
        {

        }

        public void MakePositionManager()
        {

        }

        public int MorphToExistingObject(PhysicsObj obj)
        {
            return -1;
        }

        public void MotionDone(int motion, bool success)
        {

        }

        public int MoveOrTeleport(Position pos, int timestamp, Vector3 velocity)
        {
            return -1;
        }

        public void MoveToObject(int objectID, MovementParameters movementParams)
        {

        }

        public void MoveToObject_Internal(int objectID, int topLevelID, float objRadius, float objHeight, MovementParameters movementParams)
        {

        }

        public void RemoveLinkAnimations()
        {

        }

        public void RemoveObjectFromSingleCell(ObjCell objCell)
        {

        }

        public void RemovePartFromShadowCells(PhysicsPart part)
        {

        }

        public void RemoveStaticAnimatingObject()
        {

        }

        public void RestoreLighting()
        {

        }

        public void SetDiffusion(float start, float end, double delta)
        {
            if (delta < PhysicsGlobals.EPSILON)
            {
                if (PartArray != null)
                    PartArray.SetDiffusionInternal(end);
                return;
            }
            var hook = new FPHook(HookType.Velocity, PhysicsTimer.CurrentTime, delta, start, end, 0);
            Hooks.AddLast(hook);
        }
        

        public void SetLighting(float luminosity, float diffuse)
        {
            if (PartArray != null)
                PartArray.SetLightingInternal(luminosity, diffuse);
        }

        public void SetLuminosity(float start, float end, double delta)
        {
            if (delta < PhysicsGlobals.EPSILON)
            {
                if (PartArray != null)
                    PartArray.SetLuminosityInternal(end);
                return;
            }
            var hook = new FPHook(HookType.MotionTable | HookType.Setup, PhysicsTimer.CurrentTime, delta, start, end, 0);
            Hooks.AddLast(hook);
        }

        public bool SetMotionTableID(int mtableID)
        {
            if (PartArray == null) return false;
            if (!PartArray.SetMotionTableID(mtableID)) return false;

            MovementManager = null;
            if (mtableID != 0) MakeMovementManager(true);

            return true;
        }

        public void SetNoDraw(bool noDraw)
        {
            if (PartArray != null)
                PartArray.SetNoDrawInternal(noDraw);
        }

        public static void SetObjectMaintainer(ObjectMaint objMaint)
        {
            ObjMaint = objMaint;
        }

        public void SetPartDiffusion(int part, float start, float end, double delta)
        {
            if (delta < PhysicsGlobals.EPSILON)
            {
                if (PartArray != null)
                    PartArray.SetPartDiffusionInternal(part, end);
                return;
            }
            var hook = new FPHook(HookType.Velocity | HookType.MotionTable, PhysicsTimer.CurrentTime, delta, start, end, part);
            Hooks.AddLast(hook);
        }

        public bool SetPartLighting(int partIdx, float luminosity, float diffuse)
        {
            if (PartArray != null)
                return PartArray.SetPartLightingInternal(partIdx, luminosity, diffuse);
            else
                return false;
        }

        public void SetPartLuminosity(int part, float start, float end, double delta)
        {
            if (delta < PhysicsGlobals.EPSILON)
            {
                if (PartArray != null)
                    PartArray.SetPartLuminosityInternal(part, end);
                return;
            }
            var hook = new FPHook(HookType.Velocity | HookType.Setup, PhysicsTimer.CurrentTime, delta, start, end, part);
            Hooks.AddLast(hook);
        }

        public void SetPartTextureVelocity(int partIdx, float du, float dv)
        {
            if (PartArray != null)
                PartArray.SetPartTextureVelocityInternal(partIdx, du, dv);
        }

        public void SetPartTranslucency(int partIdx, float startTrans, float endTrans, double delta)
        {
            if (delta < PhysicsGlobals.EPSILON)
            {
                if (PartArray != null)
                    PartArray.SetPartTranslucencyInternal(partIdx, endTrans);
                return;
            }
            var hook = new FPHook(HookType.MotionTable, PhysicsTimer.CurrentTime, delta, startTrans, endTrans, partIdx);
            Hooks.AddLast(hook);
        }

        public bool SetPlacementFrame(int frameID, bool sendEvent)
        {
            return SetPlacementFrameInternal(frameID);
        }

        public bool SetPlacementFrameInternal(int frameID)
        {
            bool result = false;
            if (PartArray != null)
            {
                result = PartArray.SetPlacementFrame(frameID);
                if (!State.HasFlag(PhysicsState.ParticleEmitter))
                    PartArray.SetFrame(Position.Frame);
            }
            return result;
        }

        public static void SetPlayer(PhysicsObj newPlayer)
        {
            PlayerObject = newPlayer;
        }

        public SetPositionError SetPosition(SetPosition setPos)
        {
            var transition = Transition.MakeTransition();
            if (transition == null)
                return SetPositionError.GeneralFailure;

            transition.InitObject(this, ObjectInfoState.Default);

            if (PartArray != null && PartArray.GetNumSphere() != 0)
                transition.InitSphere(PartArray.GetNumSphere(), PartArray.GetSphere(), Scale);
            else
                transition.InitSphere(1, PhysicsGlobals.DummySphere, 1.0f);

            var result = SetPositionInternal(setPos, transition);
            transition.CleanupTransition();

            return result;
        }

        public SetPositionError SetPositionInternal(Transition transition)
        {
            var prevOnWalkable = (TransientState & TransientStateFlags.OnWalkable) != 0;
            var transitCell = transition.SpherePath.CurCell;
            var prevContact = (TransientState & TransientStateFlags.Contact) != 0;
            var curPos = transition.SpherePath.CurPos;

            if (transitCell == null)
            {
                prepare_to_leave_visibility();
                store_position(curPos);

                ObjMaint.GotoLostCell(Position.ObjCellID);

                TransientState &= ~TransientStateFlags.Active;
                return SetPositionError.GeneralFailure;
            }

            if (transitCell == Cell)
            {
                Position.ObjCellID = curPos.ObjCellID;
                if (PartArray != null && !State.HasFlag(PhysicsState.ParticleEmitter))
                    PartArray.SetCellID(curPos.ObjCellID);
                if (Children != null)
                {
                    for (var i = 0; i < Children.NumObjects; i++)
                    {
                        var child = Children.Objects[i];
                        child.Position.ObjCellID = curPos.ObjCellID;
                        if (child.PartArray != null && !child.State.HasFlag(PhysicsState.ParticleEmitter))
                            child.PartArray.SetCellID(curPos.ObjCellID);
                    }
                }
            }
            else
            {
                change_cell(transitCell);
            }

            set_frame(curPos.Frame);

            var collisions = transition.CollisionInfo;

            ContactPlaneCellID = collisions.ContactPlaneCellID;
            ContactPlane = collisions.ContactPlane;

            if (collisions.ContactPlaneValid)
                TransientState |= TransientStateFlags.Contact;
            else
                TransientState &= ~TransientStateFlags.Contact;

            calc_acceleration();

            if (collisions.ContactPlaneIsWater)
                TransientState |= TransientStateFlags.WaterContact;
            else
                TransientState &= ~TransientStateFlags.WaterContact;

            if (TransientState.HasFlag(TransientStateFlags.Contact))
            {
                if (ContactPlane.Normal.Z >= PhysicsGlobals.FloorZ)
                    set_on_walkable(false);
                else
                    set_on_walkable(true);
            }
            else
            {
                TransientState &= ~TransientStateFlags.OnWalkable;

                if (MovementManager != null && prevOnWalkable)
                    MovementManager.LeaveGround();

                calc_acceleration();
            }

            if (collisions.SlidingNormalValid)
                TransientState |= TransientStateFlags.Sliding;
            else
                TransientState &= ~TransientStateFlags.Sliding;

            handle_all_collisions(collisions, prevContact, prevOnWalkable);

            if (Cell != null)
            {
                if (State.HasFlag(PhysicsState.HasPhysicsBSP))
                {
                    calc_cross_cells();
                    return SetPositionError.GeneralFailure;
                }

                if (transition.CellArray.Cells.Count > 0)
                {
                    remove_shadows_from_cells();
                    add_shadows_to_cell(transition.CellArray);

                    return SetPositionError.GeneralFailure;
                }
            }

            return SetPositionError.OK;
        }

        public SetPositionError SetPositionInternal(Position pos, SetPosition setPos, Transition transition)
        {
            if (Cell == null) prepare_to_enter_world();

            var newCell = AdjustPosition(pos, transition.SpherePath.LocalSphere[0].Center, setPos.Flags.HasFlag(SetPositionFlags.DontCreateCells), true);

            if (newCell == null)
            {
                prepare_to_leave_visibility();
                store_position(pos);
                ObjMaint.GotoLostCell(Position.ObjCellID);
                set_active(false);
                return SetPositionError.OK;
            }

            if (WeenieObj != null && (WeenieObj.IsStorage() || WeenieObj.IsCorpse()))
                return ForceIntoCell(newCell, pos);

            //if (setPos.Flags.HasFlag(SetPositionFlags.DontCreateCells))
                //transition.CellArray.DoNotLoadCells = true;

            if (!CheckPositionInternal(newCell, pos, transition, setPos))
                return handle_all_collisions(transition.CollisionInfo, false, false) ?
                    SetPositionError.Collided : SetPositionError.NoValidPosition;

            if (transition.SpherePath.CurCell == null) return SetPositionError.NoCell;

            if (SetPositionInternal(transition) != SetPositionError.OK)
                return SetPositionError.GeneralFailure;

            return SetPositionError.OK;
        }

        public SetPositionError SetPositionInternal(SetPosition setPos, Transition transition)
        {
            if (setPos.Flags.HasFlag(SetPositionFlags.RandomScatter))
                return SetScatterPositionInternal(setPos, transition);

            // frame copy constructor
            var result = SetPositionInternal(setPos.Pos, setPos, transition);
            if (result != SetPositionError.OK && setPos.Flags.HasFlag(SetPositionFlags.Scatter))
                return SetScatterPositionInternal(setPos, transition);

            return result;
        }

        public SetPositionError SetPositionSimple(Position pos, bool sliding)
        {
            var flags = sliding ? 4114 : 4098;  // ??
            var setPos = new SetPosition();
            setPos.Pos = pos;
            setPos.Flags = (SetPositionFlags)flags;

            return SetPosition(setPos);
        }

        public void SetScale(float scale, double delta)
        {
            if (delta < PhysicsGlobals.EPSILON)
            {
                if (PartArray != null)
                    PartArray.SetScaleInternal(new Vector3(scale, scale, scale));
                return;
            }
            var hook = new FPHook((HookType)0, PhysicsTimer.CurrentTime, delta, Scale, scale, 0);
            Hooks.AddLast(hook);
        }

        public void SetScaleStatic(float scale)
        {
            Scale = scale;
            if (PartArray != null)
                PartArray.SetScaleInternal(new Vector3(scale, scale, scale));
        }

        public SetPositionError SetScatterPositionInternal(SetPosition setPos, Transition transition)
        {
            var result = SetPositionError.GeneralFailure;

            for (var i = 0; i < setPos.NumTries; i++)
            {
                Position newPos = null;
                var origin = newPos.Frame.Origin;
                newPos = setPos.Pos;    // ??

                newPos.Frame.Origin.X += Random.RollDice(-1.0f, 1.0f) * setPos.RadX;
                newPos.Frame.Origin.Y += Random.RollDice(-1.0f, 1.0f) * setPos.RadY;

                result = SetPositionInternal(newPos, setPos, transition);
                if (result == SetPositionError.OK) break;
            }
            return result;
        }

        public void SetTextureVelocity(float du, float dv)
        {
            if (PartArray != null)
                PartArray.SetTextureVelocityInternal(du, dv);
        }

        public void SetTranslucency(float translucency, double delta)
        {
            if (delta < PhysicsGlobals.EPSILON)
            {
                Translucency = translucency < TranslucencyOriginal ?
                    translucency : TranslucencyOriginal;

                if (PartArray != null) PartArray.SetTranslucencyInternal(Translucency);
                return;
            }
            var hook = new FPHook(HookType.Setup, PhysicsTimer.CurrentTime, delta, 0.0f, translucency, 0);
            Hooks.AddLast(hook);
        }

        public void SetTranslucency2(float startTrans, float endTrans, double delta)
        {
            if (delta < PhysicsGlobals.EPSILON)
            {
                Translucency = endTrans < TranslucencyOriginal ? endTrans : TranslucencyOriginal;

                if (PartArray != null) PartArray.SetTranslucencyInternal(startTrans);
                return;
            }
            var hook = new FPHook(HookType.Setup, PhysicsTimer.CurrentTime, delta, startTrans, endTrans, 0);
            Hooks.AddLast(hook);
        }

        public void SetTranslucencyHierarchial(float translucency)
        {
            Translucency = translucency < TranslucencyOriginal ?
                   translucency : TranslucencyOriginal;

            if (PartArray != null)
                PartArray.SetTranslucencyInternal(translucency);

            for (var i = 0; i < Children.NumObjects; i++)
                Children.Objects[i].SetTranslucencyHierarchial(translucency);
        }

        public void SetTranslucencyInternal(float translucency)
        {
            Translucency = translucency < TranslucencyOriginal ?
                   translucency : TranslucencyOriginal;

            if (PartArray != null)
                PartArray.SetTranslucencyInternal(translucency);
        }

        public bool ShouldDrawParticles(float degradeDistance)
        {
            return false;
        }

        public void StopCompletely(bool sendEvent)
        {

        }

        public void StopCompletely_Internal()
        {

        }

        public void StopInterpolating()
        {

        }

        public int StopInterpretedMotion(int motion, MovementParameters movementParams)
        {
            return -1;
        }

        public int StopMotion(int motion, MovementParameters movementParams, bool sendEvent)
        {
            return -1;
        }

        public void TurnToHeading(MovementParameters movementParams)
        {

        }

        public void TurnToObject(int objectID, MovementParameters movementParams)
        {

        }

        public void UpdateChild(PhysicsObj childObj, int partIdx, AFrame childFrame)
        {
            var frame = partIdx >= PartArray.NumParts ?
                AFrame.Combine(Position.Frame, childFrame) :AFrame.Combine(PartArray.Parts[partIdx].Pos.Frame, childFrame);

            childObj.set_frame(frame);

            if (childObj.ParticleManager != null) childObj.ParticleManager.UpdateParticles();
            if (childObj.ScriptManager != null) childObj.ScriptManager.UpdateScripts();
        }

        public void UpdateChildrenInternal()
        {
            if (PartArray == null || Children == null || Children.NumObjects == 0) return;

            for (var i = 0; i < Children.Objects.Count; i++)
                UpdateChild(Children.Objects[i], Children.PartNumbers[i], Children.Frames[i]);
        }

        public void UpdateObjectInternal(double quantum)
        {
            if (!TransientState.HasFlag(TransientStateFlags.Active) || Cell == null)
                return;

            if (TransientState.HasFlag(TransientStateFlags.CheckEthereal))
                set_ethereal(false, false);

            JumpedThisFrame = false;
            var newPos = new Position(Position.ObjCellID);
            UpdatePositionInternal(quantum, newPos.Frame);

            if (PartArray != null && PartArray.GetNumSphere() != 0)
            {
                if (newPos.Frame.Origin.Equals(Position.Frame.Origin))  // epsilon compare?
                {
                    CachedVelocity = Vector3.Zero;
                    set_frame(newPos.Frame);
                }
                else
                {
                    if (State.HasFlag(PhysicsState.AlignPath))
                    {
                        var offset = newPos.Frame.Origin - Position.Frame.Origin;
                        newPos.Frame.set_vector_heading(offset.Normalize());
                    }
                    else if (State.HasFlag(PhysicsState.Sledding) && Velocity != Vector3.Zero)
                        newPos.Frame.set_vector_heading(Velocity.Normalize());
                }
                var transit = transition(Position, newPos, false);
                if (transit != null && transit.SpherePath.CurCell != null)
                {
                    CachedVelocity = Position.GetOffset(transit.SpherePath.CurPos) / (float)quantum;
                    SetPositionInternal(transit);
                }
                else
                    _UpdateObjectInternal(ref newPos);
            }
            else
            {
                if (MovementManager == null && TransientState.HasFlag(TransientStateFlags.OnWalkable))
                    TransientState &= ~TransientStateFlags.Active;

                _UpdateObjectInternal(ref newPos);
            }

            if (DetectionManager != null) DetectionManager.CheckDetection();

            if (TargetManager != null) TargetManager.HandleTargetting();

            if (MovementManager != null) MovementManager.UseTime();

            if (PartArray != null) PartArray.HandleMovement();

            if (PositionManager != null) PositionManager.UseTime();

            if (ParticleManager != null) ParticleManager.UpdateParticles();

            if (ScriptManager != null) ScriptManager.UpdateScripts();
        }

        public void _UpdateObjectInternal(ref Position newPos)
        {
            newPos.Frame.Origin = Position.Frame.Origin;
            set_initial_frame(newPos.Frame);
            CachedVelocity = Vector3.Zero;
        }

        public void UpdatePartsInternal()
        {
            if (PartArray == null || State.HasFlag(PhysicsState.ParticleEmitter))
                return;

            PartArray.SetFrame(Position.Frame);
        }

        public void UpdatePhysicsInternal(float quantum, AFrame frameOffset)
        {
            var velocity_mag2 = Velocity.LengthSquared();

            if (velocity_mag2 <= 0.0f)
            {
                if (MovementManager != null && TransientState.HasFlag(TransientStateFlags.OnWalkable))
                    TransientState &= ~TransientStateFlags.Active;
            }
            else
            {
                if (velocity_mag2 > PhysicsGlobals.MaxVelocitySquared)
                {
                    Velocity = Velocity.Normalize() * PhysicsGlobals.MaxVelocity;
                    velocity_mag2 = PhysicsGlobals.MaxVelocitySquared;
                }
                calc_friction(quantum, velocity_mag2);

                if (velocity_mag2 - PhysicsGlobals.SmallVelocitySquared < PhysicsGlobals.EPSILON)
                    Velocity = Vector3.Zero;

                var movement = Acceleration * 0.5f * quantum * quantum + Velocity * quantum;
                frameOffset.Origin += movement;
            }

            Velocity += Acceleration * quantum;
            frameOffset.Rotate(Omega * quantum);
        }

        public void UpdatePositionInternal(double quantum, AFrame newFrame)
        {
            if (State.HasFlag(PhysicsState.Hidden))
            {
                if (PartArray != null) PartArray.Update(quantum, newFrame);

                if (TransientState.HasFlag(TransientStateFlags.OnWalkable))
                    newFrame.Origin *= Scale;
                else
                    newFrame.Origin *= 0.0f;
            }
            if (PositionManager != null) PositionManager.AdjustOffset(newFrame, quantum);

            if (!State.HasFlag(PhysicsState.Hidden))
                UpdatePhysicsInternal((float)quantum, AFrame.Combine(newFrame, Position.Frame));
        }

        public void UpdateViewerDistance(float cypt, Vector3 heading)
        {
            if (PartArray != null) PartArray.UpdateViewerDistance(cypt, heading);
            CYpt = cypt;
        }

        public void UpdateViewerDistance()
        {
            var min_2D_degrade_dist_sq = State.HasFlag(PhysicsState.ParticleEmitter) ?
                Render.ParticleDistance2DSquared : Render.ObjectDistance2DSquared;

            var viewerHeading = Render.ViewerPos.GetOffset(Position);

            // v11??
            var lenSq = viewerHeading.LengthSquared();
            CYpt = viewerHeading.Length();

            if (PartArray != null)
                PartArray.UpdateViewerDistance(CYpt, viewerHeading);
        }

        public void UpdateViewerDistanceRecursive()
        {
            UpdateViewerDistance();

            if (Children == null || Children.NumObjects == 0) return;

            foreach (var child in Children.Objects)
                child.UpdateViewerDistanceRecursive();
        }

        public void add_anim_hook(AnimHook hook)
        {
            AnimHooks.Add(hook);
        }

        public bool add_child(PhysicsObj obj, int where)
        {
            if (PartArray == null || obj.Equals(this)) return false;

            var setup = Setup.GetHoldingLocation(where);
            if (setup == null) return false;

            if (Children == null) Children = new ChildList();
            Children.AddChild(obj, setup.Frame, setup.PartID, where);
            return true;
        }

        public bool add_child(PhysicsObj obj, int partIdx, AFrame frame)
        {
            if (obj.Equals(this)) return false;

            if (PartArray == null || partIdx != -1 && partIdx >= PartArray.NumParts)
                return false;

            if (Children == null) Children = new ChildList();
            Children.AddChild(obj, frame, partIdx, 0);
            return true;
        }

        public void add_obj_to_cell(ObjCell newCell, AFrame newFrame)
        {
            enter_cell(newCell);

            Position.Frame = newFrame;
            if (PartArray != null && !State.HasFlag(PhysicsState.ParticleEmitter))
                PartArray.SetFrame(Position.Frame);

            UpdateChildrenInternal();
            calc_cross_cells_static();
        }

        public void add_particle_shadow_to_cell()
        {
            NumShadowObjects = 1;

            var shadowObj = new ShadowObj(this, Cell);
            ShadowObjects.Add(1, shadowObj);

            if (PartArray != null)
                PartArray.AddPartsShadow(shadowObj);
        }

        public void add_shadows_to_cell(CellArray cellArray)
        {
            if (State.HasFlag(PhysicsState.ParticleEmitter))
                add_particle_shadow_to_cell();
            else
            {
                foreach (var cell in cellArray.Cells)
                {
                    var shadowObj = new ShadowObj(this, cell);
                    ShadowObjects.Add(cell.ID, shadowObj);

                    if (cell != null) cell.AddShadowObject(shadowObj);

                    if (PartArray != null)
                        PartArray.AddPartsShadow(shadowObj);

                }
            }
            if (Children != null)
            {
                foreach (var child in Children.Objects)
                    child.add_shadows_to_cell(cellArray);
            }
        }

        public void add_voyeur(int objectID, float radius, float quantum)
        {
            if (TargetManager == null) TargetManager = new TargetManager(this);
            TargetManager.AddVoyeur(objectID, radius, quantum);
        }

        public void animate_static_object()
        {

        }

        public void attack(int attackCone)
        {

        }

        public static int build_collision_profile(int prof, PhysicsObj obj, Vector3 velocity, int amIInContact, int objIsMissile, int objHasContact)
        {
            return -1;
        }

        public void calc_acceleration()
        {
            if (TransientState.HasFlag(TransientStateFlags.Contact) && TransientState.HasFlag(TransientStateFlags.OnWalkable))
                Omega = Acceleration = Vector3.Zero;
            else
            {
                if (State.HasFlag(PhysicsState.Gravity))
                    Acceleration = new Vector3(0, 0, PhysicsGlobals.Gravity);
                else
                    Acceleration = Vector3.Zero;
            }
        }

        public void calc_cross_cells()
        {
            CellArray.SetDynamic();

            if (State.HasFlag(PhysicsState.HasPhysicsBSP))
                find_bbox_cell_list(CellArray);
            else
            {
                if (PartArray != null && PartArray.GetNumCylsphere() != 0)
                    ObjCell.find_cell_list(Position, PartArray.GetNumCylsphere(), PartArray.GetCylSphere(), CellArray, null);
                else
                {
                    var sphere = PartArray != null ? PartArray.GetSortingSphere() : PhysicsGlobals.DummySphere;
                    ObjCell.find_cell_list(Position, sphere, CellArray, null);
                }
            }
            remove_shadows_from_cells();
            add_shadows_to_cell(CellArray);
        }

        public void calc_cross_cells_static()
        {
            CellArray.SetStatic();

            if (PartArray != null && PartArray.GetNumCylsphere() != 0 && !State.HasFlag(PhysicsState.HasPhysicsBSP))
                ObjCell.find_cell_list(Position, PartArray.GetNumCylsphere(), PartArray.GetCylSphere(), CellArray, null);
            else
                find_bbox_cell_list(CellArray);

            remove_shadows_from_cells();
            add_shadows_to_cell(CellArray);
        }

        public void calc_friction(double quantum, float velocity_mag2)
        {
            if (!TransientState.HasFlag(TransientStateFlags.OnWalkable)) return;

            var angle = Vector3.Dot(Velocity, ContactPlane.Normal);
            if (angle >= 0.25f) return;

            Velocity -= ContactPlane.Normal * angle;

            var friction = Friction;
            if (State.HasFlag(PhysicsState.Sledding))
            {
                if (velocity_mag2 < 1.5625f)
                    friction = 1.0f;

                else if (velocity_mag2 >= 6.25f && ContactPlane.Normal.Z > 0.99999536f)
                    friction = 0.2f;
            }

            var scalar = (float)Math.Pow(1.0f - friction, quantum);
            Velocity *= scalar;
        }

        public void cancel_moveto()
        {

        }

        public void change_cell(ObjCell newCell)
        {

        }

        public int check_attack(Position attackerPos, float attackerScale, int attackCone, float attackerAttackRadius)
        {
            return -1;
        }

        public int check_collision(PhysicsObj obj)
        {
            return -1;
        }

        public bool check_contact(bool contact)
        {
            return false;
        }

        public void clear_sequence_anims()
        {

        }

        public void clear_target()
        {

        }

        public void clear_transient_states()
        {

        }

        public int create_blocking_particle_emitter(int emitterInfoID, int partIdx, AFrame offset, int emitterID)
        {
            return -1;
        }

        public int create_particle_emitter(int emitterInfoID, int partIdx, AFrame offset, int emitterID)
        {
            return -1;
        }

        public int destroy_particle_emitter(int emitterID)
        {
            return -1;
        }

        public void destroy_particle_manager()
        {

        }

        public void enter_cell(ObjCell newCell)
        {

        }

        public int enter_world(Position pos)
        {
            return -1;
        }

        public int enter_world(int slide)
        {
            return -1;
        }

        public bool ethereal_check_for_collisions()
        {
            return false;
        }

        public void exit_world()
        {

        }

        public void find_bbox_cell_list(CellArray cellArray)
        {

        }

        public int get_curr_frame_number()
        {
            if (PartArray != null)
                return PartArray.Sequence.GetCurrFrameNumber();
            else
                return 0;
        }

        public double get_distance_to_object(PhysicsObj obj, bool use_cyls)
        {
            if (!use_cyls)
                return Position.Distance(obj.Position);

            var height = obj.PartArray != null ? obj.PartArray.GetHeight() : 0.0f;
            var radius = obj.PartArray != null ? obj.PartArray.GetRadius() : 0.0f;

            var curHeight = PartArray != null ? PartArray.GetHeight() : 0.0f;
            var curRadius = PartArray != null ? PartArray.GetRadius() : 0.0f;

            return Position.CylinderDistance(curRadius, curHeight, radius, height, obj.Position);
        }

        public AFrame get_frame()
        {
            return Position.Frame;
        }

        public double get_heading()
        {
            return Position.Frame.get_heading();
        }

        public int get_landscape_coord(int x, int y)
        {
            return LandDefs.gid_to_lcoord(Position.ObjCellID, x, y);
        }

        public Vector3 get_local_physics_velocity()
        {
            return Position.GlobalToLocalVec(Velocity);
        }

        public MotionInterp get_minterp()
        {
            if (MovementManager == null)
            {
                MovementManager = MovementManager.Create(this, WeenieObj);
                MovementManager.EnterDefaultState();
                if (State.HasFlag(PhysicsState.Static))
                {
                    if (TransientState.HasFlag(TransientStateFlags.Active))
                    {
                        // loword = cmd, hiword = param
                        // refactor...
                        UpdateTime = Timer.CurrentTime;
                    }
                    TransientState |= TransientStateFlags.Active;
                }
            }
            return MovementManager.get_minterp();
        }

        public int get_num_emitters()
        {
            if (ParticleManager == null)
                return 0;

            return ParticleManager.GetNumEmitters();
        }

        public ObjectInfo get_object_info(Transition transition, bool adminMove)
        {
            var objInfo = new ObjectInfo();
            if (State.HasFlag(PhysicsState.EdgeSlide))
                objInfo.State |= ObjectInfoState.EdgeSlide;

            if (!adminMove)
            {
                if (TransientState.HasFlag(TransientStateFlags.Contact))
                {
                    var isWater = TransientState.HasFlag(TransientStateFlags.WaterContact);

                    if (check_contact(true))
                    {
                        transition.InitContactPlane(ContactPlaneCellID, ContactPlane, isWater);

                        objInfo.State |= ObjectInfoState.Contact;
                        if (TransientState.HasFlag(TransientStateFlags.OnWalkable))
                            objInfo.State |= ObjectInfoState.OnWalkable;
                    }
                    else
                        transition.InitLastKnownContactPlane(ContactPlaneCellID, ContactPlane, isWater);
                }

                if (TransientState.HasFlag(TransientStateFlags.Sliding))
                    transition.InitSlidingNormal(SlidingNormal);
            }

            if (PartArray != null && PartArray.AllowsFreeHeading())
                objInfo.State |= ObjectInfoState.FreeRotate;

            if (State.HasFlag(PhysicsState.Missile))
                objInfo.State |= ObjectInfoState.PathClipped;

            return objInfo;

        }

        public PositionManager get_position_manager()
        {
            MakePositionManager();

            return PositionManager;
        }

        public int get_sticky_object()
        {
            if (PositionManager == null) return 0;

            return PositionManager.GetStickyObjectID();
        }

        public double get_target_quantum()
        {
            if (TargetManager == null || TargetManager.TargetInfo == null)
                return 0.0f;

            return TargetManager.TargetInfo.Quantum;
        }

        public Vector3 get_velocity()
        {
            return CachedVelocity;
        }

        public double get_walkable_z()
        {
            return PhysicsGlobals.FloorZ;
        }

        public bool handle_all_collisions(CollisionInfo collisions, bool prev_has_contact, bool prev_on_walkable)
        {
            var apply_bounce = !prev_on_walkable || !TransientState.HasFlag(TransientStateFlags.OnWalkable);
            var retval = false;
            foreach (var collideObject in collisions.CollideObject)
            {
                if (collideObject != null && track_object_collision(collideObject, prev_has_contact))
                    retval = true;
            }
            report_collision_end(false);

            if (CollidingWithEnvironment)
            {
                CollidingWithEnvironment = collisions.CollidedWithEnvironment;
            }
            else if (collisions.CollidedWithEnvironment || !prev_on_walkable && TransientState.HasFlag(TransientStateFlags.OnWalkable))
            { 
                retval = report_environment_collision(prev_has_contact);
            }
            if (collisions.FramesStationaryFall <= 1)
            {
                if (apply_bounce && collisions.CollisionNormalValid)
                {
                    if (State.HasFlag(PhysicsState.Inelastic))
                        Velocity = Vector3.Zero;
                    else
                    {
                        var collisionAngle = Vector3.Dot(Velocity, collisions.CollisionNormal);
                        if (collisionAngle < 0.0f)
                        {
                            var elasticAngle = -(collisionAngle * (Elasticity + 1.0f));
                            Velocity.X += collisions.CollisionNormal.X * elasticAngle;
                            Velocity.Y += collisions.CollisionNormal.Y;
                            Velocity.Z += collisions.CollisionNormal.Z;
                        }
                    }
                }
            }
            else
            {
                Velocity = Vector3.Zero;
                if (collisions.FramesStationaryFall == 3)
                {
                    TransientState &= ~TransientStateFlags.Active;
                    return retval;
                }
            }
            if (collisions.FramesStationaryFall == 0)
                TransientState &= ~TransientStateFlags.Active;
            else if (collisions.FramesStationaryFall == 1)
                TransientState |= TransientStateFlags.StationaryFall;
            else if (collisions.FramesStationaryFall == 2)
                TransientState |= TransientStateFlags.StationaryStop;
            else
                TransientState |= TransientStateFlags.StationaryStuck;

            return retval;
        }

        public bool is_completely_visible()
        {
            if (Cell == null || NumShadowObjects == 0)
                return false;

            foreach (var shadowObj in ShadowObjects.Values)
            {
                if (shadowObj.Cell == null)
                    return false;
            }
            return true;
        }

        public bool is_newer(int timestamp, int newTime)
        {
            if (Math.Abs(newTime - timestamp) > Int16.MaxValue)
                return newTime < timestamp;
            else
                return timestamp < newTime;
        }

        public bool is_valid_walkable(Vector3 normal)
        {
            return normal.Z >= PhysicsGlobals.FloorZ;
        }

        public void leave_cell(bool is_changing_cell)
        {
            if (Cell == null) return;
            Cell.RemoveObject(this);
            foreach (var child in Children.Objects)
                child.leave_cell(is_changing_cell);

            if (PartArray != null)
                PartArray.RemoveLightsFromCell(Cell);
            Cell = null;
        }

        public void leave_visibility()
        {
            prepare_to_leave_visibility();
            store_position(Position);
            ObjMaint.GotoLostCell(Position.ObjCellID);
            TransientState &= ~TransientStateFlags.Active;
        }

        public void leave_world()
        {
            report_collision_end(true);
            if (ObjMaint != null)
            {
                ObjMaint.RemoveFromLostCell(this);
                ObjMaint.RemoveObjectToBeDestroyed(ID);
            }
            TransientState &= ~TransientStateFlags.Active;
            remove_shadows_from_cells();
            leave_cell(false);

            Position.ObjCellID = 0;
            if (PartArray != null && !State.HasFlag(PhysicsState.ParticleEmitter))
                PartArray.SetCellID(0);

            TransientState &= ~TransientStateFlags.Contact;
            calc_acceleration();
            var walkable = TransientState.HasFlag(TransientStateFlags.OnWalkable);
            TransientState &= ~(TransientStateFlags.OnWalkable | TransientStateFlags.WaterContact);
            if (MovementManager != null && walkable) MovementManager.LeaveGround();
            calc_acceleration();
            TransientState = 0;
        }

        public bool makeAnimObject(int setupID, bool createParts)
        {
            PartArray = PartArray.CreateSetup(this, setupID, createParts);
            return PartArray != null;
        }

        public static PhysicsObj makeNullObject(int objectIID, bool dynamic)
        {
            var obj = new PhysicsObj();
            obj.InitObjectBegin(objectIID, dynamic);
            return obj;
        }

        public static PhysicsObj makeObject(PhysicsObj template)
        {
            var dataID = 0;
            if (template.PartArray != null)
                dataID = template.PartArray.GetDataID();
            else
                template = new PhysicsObj();

            var obj = makeObject(dataID, 0, true);
            if (obj == null) return null;

            obj.MorphToExistingObject(template);

            if (obj.PartArray != null && obj.PartArray.Setup != null)
                obj.play_script_internal(obj.PartArray.Setup.DefaultScriptID);

            return obj;
        }

        public static PhysicsObj makeObject(int dataDID, int objectIID, bool dynamic)
        {
            var obj = new PhysicsObj();
            obj.InitObjectBegin(objectIID, dynamic);
            obj.InitPartArrayObject(dataDID, true);
            obj.InitObjectEnd();
            return obj;
        }

        public static PhysicsObj makeParticleObject(int numParts)
        {
            var particle = new PhysicsObj();
            particle.State = PhysicsState.Static | PhysicsState.ReportCollisions;
            particle.PartArray = PartArray.CreateParticle(particle, numParts, null);
            return particle;
        }

        public bool motions_pending()
        {
            return MovementManager != null && MovementManager.MotionsPending();
        }

        public bool movement_is_autonomous()
        {
            return LastMoveWasAutonomous;
        }

        public bool newer_event(int stamp, int newTime)
        {
            var updateTime = UpdateTimes[stamp];

            if (Math.Abs(newTime - updateTime) > Int16.MaxValue)
                return newTime < updateTime;
            else
                return updateTime < newTime;
        }

        public bool obj_within_block()
        {
            return true;
        }

        public bool on_ground()
        {
            return TransientState.HasFlag(TransientStateFlags.Contact) && TransientState.HasFlag(TransientStateFlags.OnWalkable);
        }

        public int play_default_script(int partIdx)
        {
            return -1;
        }

        public int play_default_script()
        {
            return -1;
        }

        public int play_script(int scriptID)
        {
            return -1;
        }

        public int play_script(PlayScript scriptType, float mod)
        {
            return -1;
        }

        public int play_script_internal(int scriptID)
        {
            return -1;
        }

        public int play_sound(int soundType, float volume)
        {
            return -1;
        }

        public void prepare_to_enter_world()
        {

        }

        public bool prepare_to_leave_visibility()
        {
            return true;
        }

        public void process_fp_hook(int type, float currValue, Object userData)
        {

        }

        public void process_hooks()
        {

        }

        public void queue_netblob(int blob)
        {

        }

        public void recalc_cross_cells()
        {

        }

        public void receive_detection_update(DetectionInfo info)
        {

        }

        public void receive_target_update(TargetInfo info)
        {

        }

        public bool renter_visibility()
        {
            return false;
        }

        public void remove_parts(ObjCell objCell)
        {

        }

        public void remove_shadows_from_cells()
        {

        }

        public int remove_voyeur(int objectID)
        {
            return -1;
        }

        public bool report_attacks(AttackInfo attackInfo)
        {
            return false;
        }

        public void report_collision_end(bool forceEnd)
        {

        }

        public void report_collision_start()
        {

        }

        public bool report_environment_collision(bool prev_has_contact)
        {
            return false;
        }

        public void report_exhaustion()
        {

        }

        public bool report_object_collision(PhysicsObj obj, bool prev_has_contact)
        {
            return false;
        }

        public bool report_object_collision_end(int objectID)
        {
            return false;
        }

        /// <summary>
        /// Sets the active transient state flags
        /// </summary>
        public bool set_active(bool active)
        {
            if (active)
            {
                if (State.HasFlag(PhysicsState.Static))
                    return false;

                if (TransientState.HasFlag(TransientStateFlags.Active))
                    UpdateTime = Timer.CurrentTime;

                TransientState |= TransientStateFlags.Active;
                return true;
            }
            else
            {
                TransientState &= ~TransientStateFlags.Active;
                return true;
            }
        }

        /// <summary>
        /// Sets the cell ID for an object and all its parts
        /// </summary>
        public void set_cell_id(int newCellID)
        {
            Position.ObjCellID = newCellID;
            if (!State.HasFlag(PhysicsState.ParticleEmitter) && PartArray != null)
                PartArray.SetCellID(newCellID);
        }

        /// <summary>
        /// Sets the cell ID for this object and all its children
        /// </summary>
        public void set_cell_id_recursive(int newCellID)
        {
            set_cell_id(newCellID);
            for (var i = 0; i < Children.Objects.Count; i++)
            {
                var child = Children.Objects[i];
                if (child != null)
                    child.set_cell_id_recursive(newCellID);
            }
        }

        /// <summary>
        /// Sets all of the description fields for this object
        /// </summary>
        /// <param name="desc">The physics description fields</param>
        /// <param name="setMovement">If true, unpacks the movement from the physics description</param>
        /// <returns>success boolean</returns>
        public bool set_description(PhysicsDesc desc, bool setMovement)
        {
            var mTableID = desc.GetMTableID();
            if (!SetMotionTableID(mTableID)) return false;

            if (desc.STableID != 0)
            {
                var qdid = new QualifiedDataID(desc.STableID, 0x22);
                SoundTable = (SoundTable)DBObj.Get(qdid);
            }

            if (desc.PhsTableID != 0)
            {
                var qdid = new QualifiedDataID(desc.PhsTableID, 0x2C);
                PhysicsScriptTable = (PhysicsScriptTable)DBObj.Get(qdid);
            }

            var packedMovement = HookAppraisalProfile.GetValidLocations(desc);
            if (packedMovement)
            {
                LastMoveWasAutonomous = desc.get_autonomous_movement();
                if (setMovement)
                {
                    var tradeRoomID = ChatRoomTracker.GetGlobalTradeRoomID(desc);
                    unpack_movement(packedMovement, tradeRoomID);
                }
            }
            else
            {
                var animFrameID = desc.get_animframe_id();
                unpack_movement(packedMovement, animFrameID);
            }
            set_state(desc.State, true);

            Scale = desc.ObjectScale;
            if (PartArray != null)
                PartArray.SetScaleInternal(new Vector3(Scale, Scale, Scale));

            if (desc.Friction >= 0.0f && desc.Friction <= 1.0f)
                Friction = desc.Friction;

            set_elasticity(desc.Elasticity);

            TranslucencyOriginal = desc.Translucency;
            if (desc.Translucency != 0.0f)
            {
                Translucency = desc.Translucency;
                if (PartArray != null)
                    PartArray.SetTranslucencyInternal(desc.Translucency);
            }

            set_velocity(desc.Velocity, true);

            DefaultScriptIntensity = desc.DefaultScriptIntensity;
            DefaultScript = desc.DefaultScript;

            for (var i = 0; i < UpdateTimes.Length; i++)
                UpdateTimes[i] = desc.get_timestamp(i);

            return true;
        }

        /// <summary>
        /// Sets the elasticity for a physics object
        /// </summary>
        /// <param name="elasticity">A value between 0 and MaxElasticity</param>
        /// <returns>boolean success</returns>
        public bool set_elasticity(float elasticity)
        {
            if (elasticity < 0.0f)
            {
                Elasticity = 0.0f;
                return true;
            }

            if (elasticity < PhysicsGlobals.MaxElasticity)
                Elasticity = elasticity;
            else
                Elasticity = PhysicsGlobals.MaxElasticity;

            return true;
        }

        /// <summary>
        /// Sets the ethereal (semi-transparent/walkthrough) flags
        /// </summary>
        public bool set_ethereal(bool ethereal, bool sendEvent)
        {
            if (ethereal)
            {
                State |= PhysicsState.Ethereal;
                TransientState &= ~TransientStateFlags.CheckEthereal;
                return true;
            }

            State &= ~PhysicsState.Ethereal;

            if (Parent != null || Cell != null || ethereal_check_for_collisions())
            {
                TransientState &= ~TransientStateFlags.CheckEthereal;
                return true;
            }

            TransientState |= TransientStateFlags.CheckEthereal;
            State |= PhysicsState.Ethereal;
            return false;
        }

        /// <summary>
        /// Sets the current frame of animation for this object
        /// </summary>
        public void set_frame(AFrame frame)
        {
            if (!frame.IsValid() && frame.IsValidExceptForHeading())
                frame = null;

            if (PartArray != null && !State.HasFlag(PhysicsState.ParticleEmitter))
                PartArray.SetFrame(frame);

            UpdateChildrenInternal();
        }

        /// <summary>
        /// Sets the angle this object is facing
        /// </summary>
        public void set_heading(float degrees, bool sendEvent)
        {
            Position.Frame.set_heading(degrees);
            set_frame(Position.Frame);
        }

        /// <summary>
        /// Sets this object to either hidden or unhidden
        /// </summary>
        public void set_hidden(bool hidden, bool sendEvent)
        {
            if (hidden)
            {
                State |= PhysicsState.Hidden;

                if (PhysicsScriptTable != null)
                {
                    var script = PhysicsScriptTable.GetScript(PlayScript.Hidden, 1.0f);
                    play_script_internal(script);
                }

                if (Children != null)
                {
                    for (var i = 0; i < Children.NumObjects; i++)
                    {
                        var childObj = Children.Objects[i];
                        childObj.State |= PhysicsState.NoDraw;
                        if (childObj.PartArray != null)
                            childObj.PartArray.SetNoDrawInternal(true);
                    }
                }

                if (State.HasFlag(PhysicsState.ReportCollisions))
                {
                    report_collision_end(true);
                    State &= ~PhysicsState.ReportCollisions;
                }

                State |= PhysicsState.IgnoreCollisions;

                if (Cell != null)
                    Cell.hide_object(this);

                if (PartArray != null)
                    PartArray.HandleEnterWorld();

                return;
            }

            State &= ~PhysicsState.Hidden;
            if (PhysicsScriptTable != null)
            {
                var script = PhysicsScriptTable.GetScript(PlayScript.UnHide, 1.0f);
                play_script_internal(script);
            }

            if (Children != null)
            {
                for (var i = 0; i < Children.NumObjects; i++)
                {
                    var childObj = Children.Objects[i];
                    childObj.State &= ~PhysicsState.NoDraw;
                    if (childObj.PartArray != null)
                        childObj.PartArray.SetNoDrawInternal(false);
                }
            }

            State &= ~PhysicsState.IgnoreCollisions;

            if (!State.HasFlag(PhysicsState.ReportCollisions))
            {
                State |= PhysicsState.ReportCollisions;
                report_collision_start();
            }

            if (PartArray != null)
                PartArray.HandleEnterWorld();

            if (Cell != null)
                Cell.unhide_object(this);
        }

        /// <summary>
        /// Sets the initial frame of animation for this object
        /// </summary>
        public void set_initial_frame(AFrame frame)
        {
            if (PartArray != null && !State.HasFlag(PhysicsState.ParticleEmitter))
                PartArray.SetFrame(frame);

            UpdateChildrenInternal();
        }

        public void set_lights(int lights_on, bool sendEvent)
        {
            // needed for server physics?
        }

        /// <summary>
        /// Converts the local to global velocity for this object
        /// </summary>
        public void set_local_velocity(Vector3 newVel, bool sendEvent)
        {
            set_velocity(Position.LocalToGlobalVec(newVel), sendEvent);
        }

        public bool set_nodraw(bool nodraw, bool sendEvent)
        {
            // needed for server physics?
            return true;
        }

        /// <summary>
        /// Sets the angular velocity for this physics object
        /// </summary>
        public void set_omega(Vector3 omega, bool sendEvent)
        {
            Omega = omega;
        }

        /// <summary>
        /// Handles leaving / hitting ground in movement manager,
        /// and calculates acceleration
        /// </summary>
        /// <param name="isOnWalkable">Flag indicates if this object is currently standing on the ground</param>
        public void set_on_walkable(bool isOnWalkable)
        {
            if (MovementManager != null)
            {
                if (TransientState.HasFlag(TransientStateFlags.OnWalkable) && !isOnWalkable)
                    MovementManager.LeaveGround();

                if (!TransientState.HasFlag(TransientStateFlags.OnWalkable) && isOnWalkable)
                    MovementManager.HitGround();
            }
            calc_acceleration();
        }

        /// <summary>
        /// Sets the parent object for this physics object
        /// </summary>
        /// <param name="obj">The parent object</param>
        /// <param name="idx">The child index in the parent object</param>
        /// <returns>True if success</returns>
        public bool set_parent(PhysicsObj obj, int idx)
        {
            if (obj == null) return false;
            if (!obj.add_child(this, idx)) return false;

            unset_parent();
            leave_world();

            Parent = obj;

            if (obj.Cell != null)
            {
                change_cell(obj.Cell);

                if (obj.Children != null)
                {
                    var index = obj.Children.FindChildIndex(this);
                    if (index != -1)
                    {
                        obj.UpdateChild(this, obj.Children.PartNumbers[index], obj.Children.Frames[index]);
                        recalc_cross_cells();
                    }
                }
            }

            if (Parent.State.HasFlag(PhysicsState.Hidden))
            {
                State |= PhysicsState.NoDraw;
                if (PartArray != null)
                    PartArray.SetNoDrawInternal(true);
            }

            return true;
        }

        /// <summary>
        /// Sets the parent object for this physics object
        /// </summary>
        public bool set_parent(PhysicsObj obj, int partIdx, AFrame frame)
        {
            if (obj == null) return false;
            if (!obj.add_child(this, partIdx, frame)) return false;

            ExaminationObject = obj.ExaminationObject;

            unset_parent();
            leave_world();

            Parent = obj;

            if (obj.Cell != null)
            {
                change_cell(obj.Cell);
                obj.UpdateChild(this, partIdx, frame);
                recalc_cross_cells();
            }

            return true;
        }

        public void set_sequence_animation(int animID, bool interrupt, int startFrame, float framerate)
        {
            if (PartArray == null) return;

            if (interrupt)
                PartArray.Sequence.ClearAnimations();

            PartArray.Sequence.AppendAnimation(new AnimData(animID, startFrame, 0, framerate));
        }

        public bool set_state(PhysicsState newState, bool sendEvent)
        {
            var stateDiff = State ^ newState;

            // lighting stuff needed for server?
            if (stateDiff.HasFlag(PhysicsState.LightingOn))
            {
                if (newState.HasFlag(PhysicsState.LightingOn))
                {
                    State |= PhysicsState.LightingOn;
                    if (PartArray != null)
                        PartArray.InitLights();
                }
                else
                {
                    State &= ~PhysicsState.LightingOn;
                    if (PartArray != null)
                        PartArray.DestroyLights();
                }
            }
            if (stateDiff.HasFlag(PhysicsState.NoDraw))
                set_nodraw((State & PhysicsState.NoDraw) != 0, false);

            if (stateDiff.HasFlag(PhysicsState.Hidden))
                set_hidden((State & PhysicsState.Hidden) != 0, false);

            return true;
        }

        public void set_target(int contextID, int objectID, float radius, double quantum)
        {
            if (TargetManager == null)
                TargetManager = new TargetManager();

            TargetManager.SetTarget(contextID, objectID, radius, quantum);
        }

        public void set_target_quantum(double new_quantum)
        {
            if (TargetManager != null)
                TargetManager.SetTargetQuantum(new_quantum);
        }

        /// <summary>
        /// Sets the global velocity for this physics object
        /// </summary>
        /// <param name="velocity">The velocity in global space</param>
        /// <param name="sendEvent">Flag indicates if this event should send a network broadcast</param>
        public void set_velocity(Vector3 velocity, bool sendEvent)
        {
            if (!velocity.Equals(Velocity))
            {
                Velocity = velocity;

                if (Velocity.Length() > PhysicsGlobals.MaxVelocity)
                {
                    Velocity /= Velocity.Length();    // todo: add normalize method
                    Velocity *= PhysicsGlobals.MaxVelocity;
                }
                JumpedThisFrame = true;
            }

            if (!State.HasFlag(PhysicsState.Static))
            {
                if (!TransientState.HasFlag(TransientStateFlags.Active))
                    UpdateTime = Timer.CurrentTime;

                TransientState |= TransientStateFlags.Active;
            }
        }

        /// <summary>
        /// Sets the weenie for this physics object
        /// </summary>
        public void set_weenie_obj_ptr(WeenieObject wobj)
        {
            WeenieObj = wobj;
            if (MovementManager != null)
                MovementManager.SetWeenieObject(wobj);
        }

        public void stick_to_object(int objectID)
        {
            MakePositionManager();
            if (ObjMaint == null) return;

            var objectA = ObjMaint.GetObjectA(objectID);
            if (objectA == null) return;
            if (objectA.Parent != null)
                objectA = Parent;

            if (objectA.PartArray != null)
                PositionManager.StickTo(objectA.ID, objectA.PartArray.GetRadius(), objectA.PartArray.GetHeight());
            else
                PositionManager.StickTo(objectA.ID, 0, 0);
        }

        public bool stop_particle_emitter(int emitterID)
        {
            if (ParticleManager == null) return false;
            return ParticleManager.StopParticleEmitter(emitterID);
        }

        public void store_position(Position pos)
        {
            if (pos.ObjCellID < 0x100) LandDefs.AdjustToOutside(pos);

            if (PartArray != null && !State.HasFlag(PhysicsState.ParticleEmitter))
                PartArray.SetCellID(pos.ObjCellID);

            set_frame(pos.Frame);
        }

        public void teleport_hook(bool hide)
        {
            if (MovementManager != null)
                MovementManager.CancelMoveTo(0x3C);

            if (PositionManager != null)
            {
                PositionManager.Unstick();
                PositionManager.StopInterpolating();
                PositionManager.Unconstrain();
            }
            if (TargetManager != null)
            {
                TargetManager.ClearTarget();
                TargetManager.NotifyVoyeurOfEvent(TargetStatus.Teleported);
            }
            report_collision_end(true);
        }

        public bool track_object_collision(PhysicsObj obj, bool prev_has_contact)
        {
            if (State.HasFlag(PhysicsState.Static))
                return report_environment_collision(prev_has_contact);

            if (CollisionTable == null)
                CollisionTable = new Dictionary<long, CollisionRecord>();

            if (!CollisionTable.ContainsKey(obj.ID)) return false;
            CollisionTable.Remove(obj.ID);
            return report_object_collision(obj, prev_has_contact);
        }

        public Transition transition(Position oldPos, Position newPos, bool adminMove)
        {
            var trans = Transition.MakeTransition();
            if (trans == null) return null;
            var objectInfo = get_object_info(trans, adminMove);
            trans.InitObject(this, objectInfo.State);
            if (PartArray == null || PartArray.GetNumSphere() == 0)
                trans.InitSphere(1, PhysicsGlobals.DummySphere, 1.0f);
            else
            {
                var sphere = PartArray == null ? PartArray.GetSphere() : null;
                if (PartArray != null)
                    trans.InitSphere(PartArray.GetNumSphere(), sphere, Scale);
                else
                    trans.InitSphere(0, sphere, Scale);
            }
            trans.InitPath(Cell, oldPos, newPos);

            if (TransientState.HasFlag(TransientStateFlags.StationaryStuck))
                trans.CollisionInfo.FramesStationaryFall = 3;
            else if (TransientState.HasFlag(TransientStateFlags.StationaryStop))
                trans.CollisionInfo.FramesStationaryFall = 2;
            else if (TransientState.HasFlag(TransientStateFlags.StationaryFall))
                trans.CollisionInfo.FramesStationaryFall = 1;

            var validPos = trans.FindValidPosition();
            trans.CleanupTransition();
            if (!validPos) return null;
            return trans;
        }

        public void unpack_movement(Object addr, int size)
        {
            if (MovementManager == null)
            {
                MovementManager = MovementManager.Create(this, WeenieObj);
                if (!State.HasFlag(PhysicsState.Static) && TransientState.HasFlag(TransientStateFlags.Active))
                    UpdateTime = Timer.CurrentTime;
            }
            MovementManager.unpack_movement(addr, size);
        }

        public void unparent_children()
        {
            if (Children == null) return;
            foreach (var child in Children.Objects)
                child.unset_parent();
        }

        public void unset_parent()
        {
            if (Parent == null) return;
            if (Parent.Children != null)
                Parent.Children.RemoveChild(this);
            if (Parent.State.HasFlag(PhysicsState.Hidden))
            {
                State &= ~PhysicsState.Hidden;
                if (PartArray != null)
                    PartArray.SetNoDrawInternal(false);
            }
            Parent = null;
            UpdateTime = Timer.CurrentTime;
            clear_transient_states();
        }

        public void unstick_from_object()
        {
            if (PositionManager != null)
                PositionManager.Unstick();
        }

        public void update_object()
        {
            if (Parent != null || Cell != null | State.HasFlag(PhysicsState.Frozen))
            {
                TransientState &= ~TransientStateFlags.Active;
                return;
            }
            if (PlayerObject != null)
            {
                PlayerVector = PlayerObject.Position.GetOffset(Position);
                PlayerDistance = PlayerVector.Length();
                if (PlayerDistance > 96.0f && ObjMaint.IsActive)
                    TransientState &= ~TransientStateFlags.Active;
                else
                    set_active(true);
            }
            var deltaTime = Timer.CurrentTime - UpdateTime;
            PhysicsTimer.CurrentTime = UpdateTime;
            if (deltaTime > PhysicsGlobals.EPSILON)
            {
                if (deltaTime <= 2.0f)
                {
                    while (deltaTime > PhysicsGlobals.MaxQuantum)
                    {
                        PhysicsTimer.CurrentTime += PhysicsGlobals.MaxQuantum;
                        UpdateObjectInternal(PhysicsGlobals.MaxQuantum);
                        deltaTime -= PhysicsGlobals.MaxQuantum;
                    }
                    if (deltaTime > PhysicsGlobals.MinQuantum)
                    {
                        PhysicsTimer.CurrentTime += deltaTime;
                        UpdateObjectInternal(deltaTime);
                    }
                }
                else
                {
                    UpdateTime = Timer.CurrentTime;
                }
            }
        }

        public void update_position()
        {
            if (Parent != null) return;
            PhysicsTimer.CurrentTime = Timer.CurrentTime;
            var deltaTime = Timer.CurrentTime - UpdateTime;
            if (deltaTime >= PhysicsGlobals.MinQuantum && deltaTime <= 2.0f)
            {
                var frame = new AFrame();
                UpdatePositionInternal(deltaTime, frame);
                set_frame(frame);
                if (ParticleManager != null)
                    ParticleManager.UpdateParticles();
                if (ScriptManager != null)
                    ScriptManager.UpdateScripts();
            }
            UpdateTime = Timer.CurrentTime;
        }
    }
}
