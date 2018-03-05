using System;
using System.Collections.Generic;
using System.Numerics;
using ACE.Entity.Enum;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Combat;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Extensions;
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
        public LinkedList<AnimHook> AnimHooks;
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
        public Dictionary<int, CollisionRecord> CollisionTable;
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
            MovementManager = null;
            PositionManager = null;
            ParticleManager = null;
            ScriptManager = null;
            Hooks = null;

            if (State.HasFlag(PhysicsState.Static) && (State.HasFlag(PhysicsState.HasDefaultAnim) || State.HasFlag(PhysicsState.HasDefaultScript)))
                Physics.RemoveStaticAnimatingObject(this);

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
                PartArray.AddPartsShadow(objCell, 1);
        }

        public void AddPartToShadowCells(PhysicsPart part)
        {
            if (Cell != null) part.Pos.ObjCellID = Cell.ID;
            foreach (var shadowObj in ShadowObjects.Values)
            {
                var shadowCell = shadowObj.Cell;
                if (shadowCell != null)
                {
                    shadowCell.AddPart(part, null,
                        shadowCell.Pos.Frame, ShadowObjects.Count);
                }
            }
        }

        public ObjCell AdjustPosition(Position position, Vector3 low_pt, bool dontCreateCells, bool searchCells)
        {
            var cellID = position.ObjCellID & 0xFFFF;
            if ((cellID < 1 || cellID > 0x40) && (cellID < 0x100 || cellID > 0xFFFD) && cellID != 0xFFFF)
                return null;
            cellID = position.ObjCellID;
            if (cellID < 0x100)
            {
                LandDefs.AdjustToOutside(position);
                return ObjCell.GetVisible(cellID);
            }
            var visibleCell = ObjCell.GetVisible(cellID);
            if (visibleCell == null) return null;
            var point = position.LocalToGlobal(position, low_pt);
            var child = EnvCell.find_visible_child_cell(visibleCell.Pos.Frame.Origin, searchCells);
            if (child != null)
            {
                position.ObjCellID = child.ID;
                return child;
            }
            if (!visibleCell.SeenOutside) return null;
            position.adjust_to_outside();
            return ObjCell.GetVisible(cellID);
        }

        public bool CacheHasPhysicsBSP()
        {
            if (PartArray != null && PartArray.CacheHasPhysicsBSP())
            {
                State |= PhysicsState.HasPhysicsBSP;
                return true;
            }
            else
            {
                State &= ~PhysicsState.HasPhysicsBSP;
                return false;
            }
        }

        public void CallPES(int pes, double delta)    // long double?
        {
            if (delta < PhysicsGlobals.EPSILON)
            {
                if (Cell != null) play_script_internal(pes);
                return;
            }
            var upperBound = (float)delta;
            var randp = Random.RollDice(0.0f, upperBound);
            var hook = new FPHook(HookType.Velocity | HookType.MotionTable | HookType.Setup, PhysicsTimer.CurrentTime, randp, 0.0f, 1.0f, pes);
            Hooks.AddLast(hook);
        }

        public void CallPESInternal(int pes, float curValue)
        {
            if (Cell != null && curValue >= 1.0f)
                play_script_internal(pes);
        }

        public void CheckForCompletedMotions()
        {
            if (PartArray != null)
                PartArray.CheckForCompletedMotions();
        }

        public bool CheckPositionInternal(ObjCell newCell, Position newPos, Transition transition, SetPosition setPos)
        {
            transition.InitPath(newCell, null, newPos);
            if (!setPos.Flags.HasFlag(SetPositionFlags.Slide))
                transition.SpherePath.PlacementAllowsSliding = false;
            if (!transition.FindValidPosition()) return false;
            if (setPos.Flags.HasFlag(SetPositionFlags.Slide))
                return true;
            var diff = transition.SpherePath.CurPos.Frame.Origin - newPos.Frame.Origin;
            if (transition.SpherePath.CurPos.ObjCellID == newCell.ID && diff.X < 0.05f && diff.Y < 0.05f)
            {
                newPos.Frame.Origin = transition.SpherePath.CurPos.Frame.Origin;
                return true;
            }
            return false;
        }

        public void ConstrainTo(Position pos, float startDistance, float maxDistance)
        {
            MakePositionManager();
            if (PositionManager != null)
                PositionManager.ConstrainTo(pos, startDistance, maxDistance);
        }

        public Sequence DoInterpretedMotion(int motion, MovementParameters movementParams)
        {
            if (PartArray == null) return null;
            return PartArray.DoInterpretedMotion(motion, movementParams);
        }

        public Sequence DoMotion(int motion, MovementParameters movementParams)
        {
            LastMoveWasAutonomous = true;
            if (MovementManager == null) return new Sequence(7);
            var mvs = new MovementStruct(MovementType.RawCommand, motion, movementParams);
            return MovementManager.PerformMovement(mvs);
        }

        public void DoObjDescChanges(int objDesc)
        {
            if (PartArray != null)
                PartArray.DoObjDescChanges(objDesc);

            if (Translucency == 0.0f) return;

            Translucency = Translucency < TranslucencyOriginal ?
                TranslucencyOriginal : Translucency;

            if (PartArray != null)
                PartArray.SetTranslucencyInternal(Translucency);
        }

        public void DoObjDescChangesFromDefault(int objDesc)
        {
            if (PartArray != null)
                PartArray.DoObjDescChangesFromDefault(objDesc);

            if (Translucency == 0.0f) return;

            Translucency = Translucency < TranslucencyOriginal ?
                TranslucencyOriginal : Translucency;

            if (PartArray != null)
                PartArray.SetTranslucencyInternal(Translucency);
        }

        public void DrawRecursive()
        {
            // not required for server physics?
        }

        public TransitionState FindObjCollisions(Transition transition)
        {
            bool ethereal = false;

            if (State.HasFlag(PhysicsState.Ethereal) && State.HasFlag(PhysicsState.IgnoreCollisions))
                return TransitionState.OK;

            if (WeenieObj != null && transition.ObjectInfo.State.HasFlag(ObjectInfoState.IsViewer) && WeenieObj.IsCreature())
                return TransitionState.OK;

            if (State.HasFlag(PhysicsState.Ethereal) || !State.HasFlag(PhysicsState.Static) && transition.ObjectInfo.Ethereal)
            {
                if (transition.SpherePath.StepDown)
                    return TransitionState.OK;
                ethereal = true;
            }
            transition.SpherePath.ObstructionEthereal = ethereal;

            var state = transition.ObjectInfo.State;
            var exemption = (WeenieObj == null || !WeenieObj.IsPlayer() || !state.HasFlag(ObjectInfoState.IsPlayer) ||
                state.HasFlag(ObjectInfoState.IsImpenetrable) || WeenieObj.IsImpenetable() ||
                state.HasFlag(ObjectInfoState.IsPK) && WeenieObj.IsPK() || state.HasFlag(ObjectInfoState.IsPKLite) && WeenieObj.IsPKLite());

            var result = TransitionState.OK;
            var missileIgnore = transition.ObjectInfo.MissileIgnore(this);
            var isCreature = State.HasFlag(PhysicsState.Missile) || WeenieObj != null && WeenieObj.IsCreature();

            if (!State.HasFlag(PhysicsState.HasPhysicsBSP) || missileIgnore || exemption)
            {
                if (PartArray == null || PartArray.GetNumCylsphere() == 0 || missileIgnore || exemption)
                {
                    if (PartArray != null && PartArray.GetNumSphere() != 0 && !missileIgnore && !exemption)
                    {
                        var spheres = PartArray.GetSphere();
                        for (var i = 0; i < PartArray.GetNumSphere(); i++)
                        {
                            var intersects = spheres[i].IntersectsSphere(transition, isCreature);
                            if (intersects != TransitionState.OK)
                            {
                                return FindObjCollisions_Inner(transition, intersects, ethereal, isCreature);
                            }
                        }
                    }
                }
                else
                {
                    if (PartArray != null && PartArray.GetNumCylsphere() != 0 && !missileIgnore && !exemption)
                    {
                        var cylSpheres = PartArray.GetCylSphere();
                        for (var i = 0; i < PartArray.GetNumCylsphere(); i++)
                        {
                            var intersects = cylSpheres[i].IntersectsSphere(transition);
                            if (intersects != TransitionState.OK)
                            {
                                return FindObjCollisions_Inner(transition, intersects, ethereal, isCreature);
                            }
                        }
                    }
                }
            }
            else if (PartArray != null)
            {
                var collisions = PartArray.FindObjCollisions(transition);
                transition.SpherePath.ObstructionEthereal = false;
                return collisions;
            }

            transition.SpherePath.ObstructionEthereal = false;
            return result;
        }

        public TransitionState FindObjCollisions_Inner(Transition transition, TransitionState result, bool ethereal, bool isCreature)
        {
            if (!transition.SpherePath.StepDown)
            {
                if (State.HasFlag(PhysicsState.Static))
                {
                    if (!transition.ObjectInfo.State.HasFlag(ObjectInfoState.Contact))
                        transition.CollisionInfo.CollidedWithEnvironment = true;
                }
                else
                {
                    if (ethereal || isCreature && transition.ObjectInfo.State.HasFlag(ObjectInfoState.IgnoreCreatures))
                        transition.CollisionInfo.CollisionNormalValid = false;
                    transition.CollisionInfo.AddObject(this, TransitionState.OK);
                }
            }
            transition.SpherePath.ObstructionEthereal = false;
            return result;
        }

        public SetPositionError ForceIntoCell(ObjCell newCell, Position pos)
        {
            if (newCell == null) return SetPositionError.NoCell;
            set_frame(pos.Frame);
            if (!Cell.Equals(newCell))
            {
                change_cell(newCell);
                calc_cross_cells();
            }
            return SetPositionError.OK;
        }

        public double GetAutonomyBlipDistance()
        {
            if ((Position.ObjCellID & 0xFFFF) < 0x100) return 100.0f;

            return PlayerObject.Equals(this) ? 25.0f : 20.0f;
        }

        public BBox GetBoundingBox()
        {
            if (PartArray == null) return null;
            return PartArray.GetBoundingBox();
        }

        public int GetDataID()
        {
            if (PartArray == null) return 0;
            return PartArray.GetDataID();
        }

        public float GetHeight()
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
            if (targetInfo.ContextID == 0) return;

            if (MovementManager != null)
                MovementManager.HandleUpdateTarget(targetInfo);
            if (PositionManager != null)
                PositionManager.HandleUpdateTarget(targetInfo);
        }

        public bool HasAnims()
        {
            if (PartArray == null) return false;
            return PartArray.HasAnims();
        }

        public void Hook_AnimDone()
        {
            if (PartArray != null)
                PartArray.AnimationDone(true);
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
                PartArray = PartArray.CreateMesh(this, dataDID);
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
            if (MovementManager == null)
                return null;
            else
                return MovementManager.InqInterpretedMotionState();
        }

        public RawMotionState InqRawMotionState()
        {
            if (MovementManager == null)
                return null;
            else
                return MovementManager.InqRawMotionState();
        }

        public void InterpolateTo(Position p, bool keepHeading)
        {
            MakePositionManager();
            PositionManager.InterpolateTo(p, keepHeading);
        }

        public bool IsFullyConstrained()
        {
            if (PositionManager == null)
                return false;
            else
                return PositionManager.IsFullyConstrained();
        }

        public bool IsInterpolating()
        {
            if (PositionManager == null)
                return false;
            else
                return PositionManager.IsInterpolating();
        }

        public bool IsMovingTo()
        {
            if (MovementManager == null)
                return false;
            else
                return MovementManager.IsMovingTo();
        }

        public void MakeMovementManager(bool init_motion)
        {
            if (MovementManager != null) return;

            MovementManager = MovementManager.Create(this, WeenieObj);

            if (init_motion)
                MovementManager.EnterDefaultState();

            if (!State.HasFlag(PhysicsState.Static))
            {
                if (!TransientState.HasFlag(TransientStateFlags.Active))
                    UpdateTime = Timer.CurrentTime;

                TransientState |= TransientStateFlags.Active;
            }
        }

        public void MakePositionManager()
        {
            if (PositionManager != null) return;

            PositionManager = PositionManager.Create(this);

            if (!State.HasFlag(PhysicsState.Static))
            {
                if (!TransientState.HasFlag(TransientStateFlags.Active))
                    UpdateTime = Timer.CurrentTime;

                TransientState |= TransientStateFlags.Active;
            }
        }

        public bool MorphToExistingObject(PhysicsObj obj)
        {
            var result = PartArray != null && obj.PartArray != null ?
                PartArray.MorphToExistingObject(obj.PartArray) : false;

            TranslucencyOriginal = Translucency = obj.TranslucencyOriginal;
            ExaminationObject = true;

            if (PartArray != null && Translucency != 0.0f)
                PartArray.SetTranslucencyInternal(Translucency);

            return result;
        }

        public void MotionDone(int motion, bool success)
        {
            if (MovementManager != null)
                MovementManager.MotionDone(motion, success);
        }

        public bool MoveOrTeleport(Position pos, int timestamp, bool contact, Vector3 velocity)
        {
            var updateTime = UpdateTimes[4];
            bool timeDiff;
            if (Math.Abs(updateTime - timestamp) > Int16.MaxValue)
                timeDiff = updateTime < timestamp;
            else
                timeDiff = timestamp < updateTime;
            if (timeDiff)
                return false;
            if (Cell == null || newer_event((int)PhysicsTimeStamp.Teleport, timestamp))
            {
                teleport_hook(true);
                var setPos = new SetPosition(pos, SetPositionFlags.Teleport | SetPositionFlags.DontCreateCells);
                SetPosition(setPos);
                return true;
            }
            else
            {
                if (!contact) return false;
                if (PlayerDistance < 96.0f)
                    InterpolateTo(pos, IsMovingTo());
                else
                {
                    if (PositionManager != null) PositionManager.StopInterpolating();
                    SetPositionSimple(pos, true);
                }
            }
            return true;
        }

        public void MoveToObject(int objectID, MovementParameters movementParams)
        {
            if (MovementManager == null)
            {
                MovementManager = MovementManager.Create(this, WeenieObj);
                MovementManager.EnterDefaultState();
                if (!State.HasFlag(PhysicsState.Static))
                {
                    if (!TransientState.HasFlag(TransientStateFlags.Active))
                        UpdateTime = Timer.CurrentTime;

                    TransientState &= ~TransientStateFlags.Active;
                }
            }

            if (ObjMaint == null) return;
            var obj = ObjMaint.GetObjectA(objectID);
            if (obj == null)
                return;
            var height = PartArray != null ? PartArray.GetHeight() : 0;
            var radius = PartArray != null ? PartArray.GetRadius() : 0;
            var parent = obj.Parent != null ? obj.Parent : obj;

            MoveToObject_Internal(objectID, parent.ID, radius, height, movementParams);
        }

        public void MoveToObject_Internal(int objectID, int topLevelID, float objRadius, float objHeight, MovementParameters movementParams)
        {
            if (MovementManager == null)
            {
                MovementManager = MovementManager.Create(this, WeenieObj);
                MovementManager.EnterDefaultState();
                if (!State.HasFlag(PhysicsState.Static))
                {
                    if (!TransientState.HasFlag(TransientStateFlags.Active))
                        UpdateTime = Timer.CurrentTime;

                    TransientState &= ~TransientStateFlags.Active;
                }
            }
            var mvs = new MovementStruct();
            // packobj vtable
            mvs.TopLevelId = topLevelID;
            mvs.Radius = objRadius;
            mvs.ObjectId = objectID;
            mvs.Params = movementParams;
            mvs.Type = MovementType.MoveToObject;
            mvs.Height = objHeight;
            MovementManager.PerformMovement(mvs);
        }

        public void RemoveLinkAnimations()
        {
            if (PartArray != null)
                PartArray.HandleEnterWorld();
        }

        public void RemoveObjectFromSingleCell(ObjCell objCell)
        {
            if (Cell != null) leave_cell(true);
            Position.ObjCellID = 0;
            if (PartArray != null && !State.HasFlag(PhysicsState.ParticleEmitter))
                PartArray.SetCellID(0);
            Cell = null;
            if (objCell != null)
            {
                objCell.remove_shadow_object(ShadowObjects[0].PhysObj);
                NumShadowObjects = 0;
                if (PartArray != null)
                    PartArray.RemoveParts(objCell);
            }
        }

        public void RemovePartFromShadowCells(PhysicsPart part)
        {
            if (Cell != null) part.Pos.ObjCellID = Cell.ID;
            foreach (var shadowObj in ShadowObjects.Values)
                shadowObj.Cell.RemovePart(part);
        }

        public void RestoreLighting()
        {
            /*if (PartArray != null)
                PartArray.RestoreLightingInternal();*/
        }

        public void SetDiffusion(float start, float end, double delta)
        {
            /*if (delta < PhysicsGlobals.EPSILON)
            {
                if (PartArray != null)
                    PartArray.SetDiffusionInternal(end);
                return;
            }
            var hook = new FPHook(HookType.Velocity, PhysicsTimer.CurrentTime, delta, start, end, 0);
            Hooks.AddLast(hook);*/
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
                transition.InitSphere(PartArray.GetNumSphere(), PartArray.GetSphere()[0], Scale);
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
            if (!ExaminationObject) return true;
            return !(CYpt > degradeDistance || Cell == null /*|| vftable unknown release */);
        }

        public void StopCompletely(bool sendEvent)
        {
            if (MovementManager == null) return;
            var mvs = new MovementStruct(MovementType.StopCompletely);
            MovementManager.PerformMovement(mvs);
        }

        public void StopCompletely_Internal()
        {
            if (PartArray != null)
                PartArray.StopCompletelyInternal();
        }

        public void StopInterpolating()
        {
            if (PositionManager != null)
                PositionManager.StopInterpolating();
        }

        public Sequence StopInterpretedMotion(int motion, MovementParameters movementParams)
        {
            var sequence = new Sequence();
            if (PartArray != null)
                sequence = PartArray.StopInterpretedMotion(motion, movementParams);
            return sequence;
        }

        public Sequence StopMotion(int motion, MovementParameters movementParams, bool sendEvent)
        {
            LastMoveWasAutonomous = true;
            if (MovementManager == null) return new Sequence(7);
            var mvs = new MovementStruct(MovementType.StopRawCommand);
            mvs.Motion = motion;
            mvs.Params = movementParams;
            return MovementManager.PerformMovement(mvs);
        }

        public void TurnToHeading(MovementParameters movementParams)
        {
            if (MovementManager == null)
            {
                // refactor into common method
                MovementManager = MovementManager.Create(this, WeenieObj);
                MovementManager.EnterDefaultState();
                if (!State.HasFlag(PhysicsState.Static))
                {
                    if (!TransientState.HasFlag(TransientStateFlags.Active))
                        UpdateTime = Timer.CurrentTime;

                    TransientState &= ~TransientStateFlags.Active;
                }
            }
            var mvs = new MovementStruct(MovementType.TurnToHeading);
            mvs.Params = movementParams;
            MovementManager.PerformMovement(mvs);
        }

        public void TurnToObject(int objectID, MovementParameters movementParams)
        {
            if (ObjMaint == null) return;
            var obj = ObjMaint.GetObjectA(objectID);
            if (obj != null) return;
            var parent = obj.Parent != null ? obj.Parent : obj;

            TurnToObject_Internal(objectID, parent.ID, movementParams);
        }

        public void TurnToObject_Internal(int objectID, int topLevelID, MovementParameters movementParams)
        {
            if (MovementManager == null)
            {
                // refactor into common method
                MovementManager = MovementManager.Create(this, WeenieObj);
                MovementManager.EnterDefaultState();
                if (!State.HasFlag(PhysicsState.Static))
                {
                    if (!TransientState.HasFlag(TransientStateFlags.Active))
                        UpdateTime = Timer.CurrentTime;

                    TransientState &= ~TransientStateFlags.Active;
                }
            }
            var mvs = new MovementStruct(MovementType.TurnToObject);
            mvs.ObjectId = objectID;
            mvs.TopLevelId = topLevelID;
            mvs.Params = movementParams;
            MovementManager.PerformMovement(mvs);
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
            AnimHooks.AddLast(hook);
        }

        public bool add_child(PhysicsObj obj, int where)
        {
            if (PartArray == null || obj.Equals(this)) return false;

            var setup = PartArray.Setup.GetHoldingLocation(where);
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
                PartArray.AddPartsShadow(Cell, 1);
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
                        PartArray.AddPartsShadow(cell, NumShadowObjects);

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
            if (Cell == null) return;

            PhysicsTimer.CurrentTime = Timer.CurrentTime;
            var deltaTime = Timer.CurrentTime = UpdateTime;
            if (deltaTime < PhysicsGlobals.MinQuantum) return;
            if (PartArray == null || deltaTime < PhysicsGlobals.EPSILON || deltaTime > PhysicsGlobals.MaxQuantum)
            {
                UpdateTime = Timer.CurrentTime;
                return;
            }
            if (State.HasFlag(PhysicsState.HasDefaultAnim))
            {
                PartArray.Update(deltaTime, null);
                Position.Frame.Rotate(Omega);
                UpdatePartsInternal();
                UpdateChildrenInternal();
            }
            if (ScriptManager != null && State.HasFlag(PhysicsState.HasDefaultScript))
                ScriptManager.UpdateScripts();
            if (ParticleManager != null)
            {
                ParticleManager.UpdateParticles();
            }
            process_hooks();
            UpdateTime = Timer.CurrentTime;
        }

        public void attack(AttackCone attackCone)
        {
            if (Cell.ID == 0) return;
            if (AttackManager == null)AttackManager = new AttackManager();

            var sphere = new Sphere();
            sphere.Center.Z = attackCone.Height * Scale;
            sphere.Radius = AttackManager.AttackRadius + attackCone.Radius * Scale;

            var cellArray = new CellArray();
            ObjCell.find_cell_list(Position, sphere, cellArray, null);

            var attackInfo = AttackManager.NewAttack(attackCone.PartIdx);

            foreach (var cell in cellArray.Cells)
                cell.CheckAttack(ID, Position, Scale, attackCone, attackInfo);

            if (!attackInfo.WaitingForCells)
                report_attacks(attackInfo);
        }

        public AtkCollisionProfile build_collision_profile(PhysicsObj obj, bool prev_has_contact, Vector3 velocityCollide)
        {
            AtkCollisionProfile profile = null;

            if (!State.HasFlag(PhysicsState.Missile))
                profile = (AtkCollisionProfile)build_collision_profile(obj, velocityCollide, prev_has_contact,
                    obj.State.HasFlag(PhysicsState.Missile), obj.TransientState.HasFlag(TransientStateFlags.Contact));
            else
            {
                profile = new AtkCollisionProfile();
                profile.ID = obj.ID;
                profile.Part = -1;
                profile.Location = obj.Position.DetermineQuadrant(obj.GetHeight(), Position);
            }
            return profile;
        }

        public static ObjCollisionProfile build_collision_profile(PhysicsObj obj, Vector3 velocity, bool amIInContact, bool objIsMissile, bool objHasContact)
        {
            if (obj.WeenieObj != null /* && vfptr */) return null;
            var prof = new ObjCollisionProfile();
            prof.Velocity = velocity;
            if (objIsMissile)
                prof.Flags |= ObjCollisionProfileFlags.Missile;
            if (objHasContact)
                prof.Flags |= ObjCollisionProfileFlags.Contact;
            if (amIInContact)
                prof.Flags |= ObjCollisionProfileFlags.MyContact;
            return prof;
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
                    ObjCell.find_cell_list(Position, PartArray.GetNumCylsphere(), PartArray.GetCylSphere()[0], CellArray, null);
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
                ObjCell.find_cell_list(Position, PartArray.GetNumCylsphere(), PartArray.GetCylSphere()[0], CellArray, null);
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
            if (MovementManager != null)
                MovementManager.CancelMoveTo(0x36);
        }

        public void change_cell(ObjCell newCell)
        {
            if (Cell != null) leave_cell(true);
            if (newCell == null)
            {
                Position.ObjCellID = 0;
                if (PartArray != null && !State.HasFlag(PhysicsState.ParticleEmitter))
                    PartArray.SetCellID(0);

                Cell = null;
            }
            else
                enter_cell(newCell);
        }

        public bool check_attack(Position attackerPos, float attackerScale, AttackCone attackCone, float attackerAttackRadius)
        {
            if (Parent != null || State.HasFlag(PhysicsState.IgnoreCollisions) || State.HasFlag(PhysicsState.ReportCollisionsAsEnvironment))
                return false;

            var targetHeight = PartArray != null ? PartArray.GetHeight() : 0;
            var targetRadius = PartArray != null ? PartArray.GetRadius() : 0;

            var attackHeight = attackerScale * attackCone.Height;
            var attackRad = attackerScale * attackCone.Radius + attackerAttackRadius;

            return Sphere.Attack(Position, targetRadius, targetHeight, attackerPos, attackCone.Left, attackCone.Right, attackRad, attackHeight);
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
            if (PartArray != null)
                PartArray.Sequence.clear_animations();
        }

        public void clear_target()
        {
            if (TargetManager != null) TargetManager.ClearTarget();
        }

        public void clear_transient_states()
        {
            TransientState &= ~TransientStateFlags.Contact;
            calc_acceleration();
            var walkable = TransientState.HasFlag(TransientStateFlags.OnWalkable);
            TransientState &= ~(TransientStateFlags.OnWalkable | TransientStateFlags.WaterContact);
            if (MovementManager != null && walkable)MovementManager.LeaveGround();
            calc_acceleration();
            TransientState = 0;
        }

        public bool create_blocking_particle_emitter(int emitterInfoID, int partIdx, AFrame offset, int emitterID)
        {
            if (ParticleManager == null)
                ParticleManager = new ParticleManager();

            return ParticleManager.CreateBlockingParticleEmitter(this, emitterInfoID, partIdx, offset, emitterID);
        }

        public bool create_particle_emitter(int emitterInfoID, int partIdx, AFrame offset, int emitterID)
        {
            if (ParticleManager == null)
                ParticleManager = new ParticleManager();

            return ParticleManager.CreateParticleEmitter(this, emitterInfoID, partIdx, offset, emitterID);
        }

        public bool destroy_particle_emitter(int emitterID)
        {
            if (ParticleManager == null) return false;
            return ParticleManager.DestroyParticleEmitter(emitterID);
        }

        public void destroy_particle_manager()
        {
            ParticleManager = null;
        }

        public void enter_cell(ObjCell newCell)
        {
            if (PartArray == null) return;
            newCell.AddObject(this);
            foreach (var child in Children.Objects)
                child.enter_cell(newCell);

            Cell = newCell;
            Position.ObjCellID = newCell.ID;
            if (PartArray != null && !State.HasFlag(PhysicsState.ParticleEmitter))
                PartArray.SetCellID(newCell.ID);
        }

        public bool enter_world(Position pos)
        {
            store_position(pos);
            return enter_world(true);
        }

        public bool enter_world(bool slide)
        {
            if (Parent != null) return false;
            UpdateTime = Timer.CurrentTime;
            var sps = new SetPosition();
            sps.Pos = Position;
            if (slide)
                sps.Flags |= SetPositionFlags.Slide;
            if (SetPosition(sps) != SetPositionError.OK)
                return false;
            if (!State.HasFlag(PhysicsState.Static))
                TransientState |= TransientStateFlags.Active;
            if (PartArray != null)
                PartArray.HandleEnterWorld();
            if (MovementManager != null)
                MovementManager.HandleEnterWorld();
            return true;
        }

        public bool ethereal_check_for_collisions()
        {
            foreach (var shadowObj in ShadowObjects.Values)
            {
                if (shadowObj.Cell.check_collisions())
                    return true;
            }
            return false;
        }

        public void exit_world()
        {
            if (PartArray != null)
                PartArray.HandleExitWorld();
            if (MovementManager != null)
                MovementManager.HandleExitWorld();
            if (PositionManager != null)
                PositionManager.Unstick();
            if (TargetManager != null)
            {
                TargetManager.ClearTarget();
                TargetManager.NotifyVoyeurOfEvent(TargetStatus.ExitWorld);
            }
            if (DetectionManager != null)
                DetectionManager.DestroyDetectionCylsphere(0);

            report_collision_end(true);
        }

        public void find_bbox_cell_list(CellArray cellArray)
        {
            if (PartArray == null || Cell == null) return;
            cellArray.NumCells = 0;
            cellArray.AddedOutside = false;
            cellArray.add_cell(Cell.ID, Cell);

            foreach (var cell in cellArray.Cells)
                PartArray.calc_cross_cells_static(cell, cellArray);
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

            return Position.CylinderDistance(curRadius, curHeight, Position, radius, height, obj.Position);
        }

        public AFrame get_frame()
        {
            return Position.Frame;
        }

        public float get_heading()
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
            // removed lighting
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
            return MovementManager != null && MovementManager.motions_pending();
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
            var sortingSphere = PartArray != null ? PartArray.GetSortingSphere() : PhysicsGlobals.DummySphere;
            var globCenter = Position.Frame.LocalToGlobal(sortingSphere.Center);

            if (State.HasFlag(PhysicsState.HasPhysicsBSP))
            {
                var radius = sortingSphere.Radius - PhysicsGlobals.EPSILON;
                return globCenter.LengthSquared() >= radius * radius;  // verify
            }
            if (PartArray != null && PartArray.GetNumCylsphere() > 0)
            {
                for (var i = 0; i < PartArray.GetNumCylsphere(); i++)
                {
                    var cylSphere = PartArray.GetCylSphere()[i];
                    var diff = Vector3.Dot(Position.Frame.Origin, cylSphere.LowPoint);
                    if (diff < cylSphere.Radius) return false;
                    if (globCenter.LengthSquared() >= cylSphere.Radius * cylSphere.Radius) return false;
                }
                return true;
            }
            if (PartArray != null && PartArray.GetNumSphere() > 0)
                return LandDefs.InBlock(globCenter, sortingSphere.Radius);
            else
                return LandDefs.InBlock(Position.Frame.Origin, 0.0f);
        }

        public bool on_ground()
        {
            return TransientState.HasFlag(TransientStateFlags.Contact) && TransientState.HasFlag(TransientStateFlags.OnWalkable);
        }

        public bool play_default_script(int partIdx)
        {
            if (Children == null) return false;
            for (var i = 0; i < Children.NumObjects; i++)
            {
                if (Children.PartNumbers[i] == partIdx && Children.Objects[i] != null)
                {
                    var childObj = Children.Objects[i];
                    if (childObj.Cell != null)
                    {
                        if (childObj.PhysicsScriptTable == null) return false;
                        var script = childObj.PhysicsScriptTable.GetScript(childObj.DefaultScript, childObj.DefaultScriptIntensity);
                        return childObj.play_script_internal(script);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool play_default_script()
        {
            if (Cell == null) return true;
            if (PhysicsScriptTable == null) return false;

            var script = PhysicsScriptTable.GetScript(DefaultScript, DefaultScriptIntensity);
            play_script_internal(script);
            return true;
        }

        public bool play_script(int scriptID)
        {
            if (Cell != null)
                return play_script_internal(scriptID);
            else
                return false;
        }

        public bool play_script(PlayScript scriptType, float mod)
        {
            if (Cell == null) return true;
            if (PhysicsScriptTable == null) return false;

            var script = PhysicsScriptTable.GetScript(scriptType, mod);
            return play_script_internal(script);
        }

        public bool play_script_internal(int scriptID)
        {
            if (scriptID == 0) return false;
            if (ScriptManager == null) ScriptManager = new ScriptManager(this);

            return ScriptManager.AddScript(scriptID);
        }

        public bool play_sound(int soundType, float volume)
        {
            if (SoundTable == null) return false;
            //SoundManager.PlaySoundA(soundType, volume);
            return true;
        }

        public void prepare_to_enter_world()
        {
            UpdateTime = Timer.CurrentTime;

            ObjMaint.RemoveFromLostCell(this);
            ObjMaint.RemoveObjectToBeDestroyed(ID);

            foreach (var child in Children.Objects)
                ObjMaint.RemoveObjectToBeDestroyed(child.ID);

            if (!State.HasFlag(PhysicsState.Static))
            {
                if (!TransientState.HasFlag(TransientStateFlags.Active))
                    UpdateTime = Timer.CurrentTime;     // hiword?

                TransientState |= TransientStateFlags.Active;
            } 
        }

        public bool prepare_to_leave_visibility()
        {
            remove_shadows_from_cells();
            ObjMaint.RemoveFromLostCell(this);
            leave_cell(false);

            ObjMaint.AddObjectToBeDestroyed(ID);
            foreach (var child in Children.Objects)
            {
                ObjMaint.AddObjectToBeDestroyed(child.ID);
            }
            return true;
        }

        public void process_fp_hook(int type, float curr_value, Object userData)
        {
            switch (type)
            {
                case 0: SetScaleStatic(curr_value);
                    break;
                case 1:
                    SetTranslucencyInternal(curr_value);
                    break;
                case 2:
                    if (PartArray != null) PartArray.SetPartTranslucencyInternal((int)userData, curr_value);
                    break;
                case 3:
                    if (PartArray != null) PartArray.SetLuminosityInternal(curr_value);
                    break;
                case 5:     // combined types?
                    if (PartArray != null) PartArray.SetPartLuminosityInternal((int)userData, curr_value);
                    break;
                case 4:
                    if (PartArray != null) PartArray.SetDiffusionInternal(curr_value);
                    break;
                case 6:
                    if (PartArray != null) PartArray.SetPartDiffusionInternal((int)userData, curr_value);
                    break;
                case 7:
                    CallPESInternal((int)userData, curr_value);
                    break;
            }
        }

        public void process_hooks()
        {
            foreach (var hook in Hooks)
                hook.Execute(this);

            Hooks.Clear();

            foreach (var animHook in AnimHooks)
                animHook.Execute(this);

            AnimHooks.Clear();
        }

        public void queue_netblob(int blob)
        {
            // convert to ACE network protocol
        }

        public void recalc_cross_cells()
        {
            if (PartArray == null) return;
            if (Position.ObjCellID != 0)
                calc_cross_cells();
            else
            {
                if (!ExaminationObject || !State.HasFlag(PhysicsState.ParticleEmitter)) return;
                add_particle_shadow_to_cell();
            }
            foreach (var child in Children.Objects)
                child.recalc_cross_cells();
        }

        public void receive_detection_update(DetectionInfo info)
        {
            if (DetectionManager == null) return;
            DetectionManager.ReceiveDetectionUpdate(info);

            if (State.HasFlag(PhysicsState.Static)) return;
            if (!TransientState.HasFlag(TransientStateFlags.Active))
                UpdateTime = Timer.CurrentTime;

            TransientState |= TransientStateFlags.Active;
        }

        public void receive_target_update(TargetInfo info)
        {
            if (TargetManager != null)
                TargetManager.ReceiveUpdate(info);
        }

        public bool renter_visibility()
        {
            prepare_to_enter_world();
            var setPos = new SetPosition(Position, SetPositionFlags.Placement | SetPositionFlags.SendPositionEvent);
            return SetPosition(setPos) == SetPositionError.OK;
        }

        public void remove_parts(ObjCell objCell)
        {
            if (PartArray != null)
                PartArray.RemoveParts(objCell);
        }

        public void remove_shadows_from_cells()
        {
            foreach (var shadowObj in ShadowObjects.Values)
            {
                if (shadowObj.Cell == null) continue;
                shadowObj.Cell.remove_shadow_object(shadowObj.PhysObj);
                if (PartArray != null)
                    PartArray.RemoveParts(shadowObj.Cell);
            }
            NumShadowObjects = 0;   // should be ShadowObjects.Count
            var i = 0;
            while (true)
            {
                var totalChilds = Children == null ? 0 : Children.NumObjects;
                if (i >= totalChilds) break;
                if (Children != null)
                    Children.Objects[i++].remove_shadows_from_cells();
                else
                    remove_shadows_from_cells();
            }
        }

        public bool remove_voyeur(int objectID)
        {
            if (TargetManager == null) return false;
            return TargetManager.RemoveVoyeur(objectID);
        }

        public void report_attacks(AttackInfo attackInfo)
        {
            // wobj virtual function call
            for (var i = 0; i < attackInfo.NumObjects; i++)
            {
                var obj = attackInfo.ObjectList[i];

                var prof = new AtkCollisionProfile();
                prof.ID = obj.ObjectID;
                prof.Location = obj.HitLocation;
                prof.Part = attackInfo.PartIndex;

                // atkcollisionprofile virtual function call
            }
            if (AttackManager != null)
                AttackManager.AttackDone(attackInfo);
        }

        public void report_collision_end(bool forceEnd)
        {
            if (CollisionTable == null) return;

            var ends = new List<int>();

            foreach (var kvp in CollisionTable)
            {
                var collision_id = kvp.Key;
                var collision = kvp.Value;

                var deltaTime = PhysicsTimer.CurrentTime - collision.TouchedTime;

                if (deltaTime > 1.0f || collision.Ethereal && deltaTime > 0.0f || forceEnd)
                    ends.Add(collision_id);
            }

            foreach (var end in ends)
            {
                CollisionTable.Remove(end);
                report_object_collision_end(end);
            }
        }

        public void report_collision_start()
        {
            if (CollisionTable == null || ObjMaint == null) return;

            foreach (var objectID in CollisionTable.Keys)
            {
                var obj = ObjMaint.GetObjectA(objectID);
                if (obj != null)
                    report_object_collision(obj, TransientState.HasFlag(TransientStateFlags.Contact));
            }
        }

        public bool report_environment_collision(bool prev_has_contact)
        {
            if (CollidingWithEnvironment) return false;

            var result = false;
            if (State.HasFlag(PhysicsState.ReportCollisions) && WeenieObj != null)
            {
                var collision = new EnvCollisionProfile();
                collision.Velocity = Velocity;
                collision.SetMeInContact(prev_has_contact);
                // WeenieObj virtual function call
                result = true;
            }
            CollidingWithEnvironment = true;
            if (State.HasFlag(PhysicsState.Missile)) State &= ~PhysicsState.Missile;
            return result;
        }

        public void report_exhaustion()
        {
            if (MovementManager != null) MovementManager.ReportExhaustion();
        }

        public bool report_object_collision(PhysicsObj obj, bool prev_has_contact)
        {
            if (obj.State.HasFlag(PhysicsState.ReportCollisionsAsEnvironment))
                return report_environment_collision(prev_has_contact);

            var velocityCollide = Velocity - obj.Velocity;

            bool collided = false;
            if (!obj.State.HasFlag(PhysicsState.IgnoreCollisions))
            {
                if (State.HasFlag(PhysicsState.ReportCollisions) && WeenieObj != null)
                {
                    var profile = build_collision_profile(obj, prev_has_contact, velocityCollide);
                    WeenieObj.DoCollision(profile);
                    collided = true;
                }

                if (State.HasFlag(PhysicsState.Missile))
                    State &= ~(PhysicsState.Missile | PhysicsState.AlignPath | PhysicsState.PathClipped);
            }

            if (obj.State.HasFlag(PhysicsState.ReportCollisions) &&!State.HasFlag(PhysicsState.IgnoreCollisions) && WeenieObj != null)
            {
                var profile = obj.build_collision_profile(this, prev_has_contact, velocityCollide);
                obj.WeenieObj.DoCollision(profile);
                collided = true;
            }
            return collided;
        }

        public bool report_object_collision_end(int objectID)
        {
            if (ObjMaint != null)
            {
                var collision = ObjMaint.GetObjectA(objectID);
                if (collision != null)
                {
                    if (!collision.State.HasFlag(PhysicsState.ReportCollisionsAsEnvironment))
                    {
                        if (State.HasFlag(PhysicsState.ReportCollisions) && WeenieObj != null)
                            WeenieObj.DoCollisionEnd(objectID);

                        if (collision.State.HasFlag(PhysicsState.ReportCollisions) && collision.WeenieObj != null)
                            collision.WeenieObj.DoCollisionEnd(objectID);
                    }
                }
                return true;
            }
            if (State.HasFlag(PhysicsState.ReportCollisions) && WeenieObj != null)
                WeenieObj.DoCollisionEnd(objectID);

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
                PartArray.Sequence.clear_animations();

            PartArray.Sequence.append_animation(new AnimData(animID, startFrame, 0, framerate));
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
                CollisionTable = new Dictionary<int, CollisionRecord>();

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
                    trans.InitSphere(PartArray.GetNumSphere(), sphere[0], Scale);
                else
                    trans.InitSphere(0, sphere[0], Scale);
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
