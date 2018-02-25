using System;
using System.Collections.Generic;
using System.Numerics;
using AnimData = ACE.Entity.AnimData;
using ACE.Entity.Enum;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Collision;
using ACE.Server.Physics.Combat;
using ACE.Server.Physics.Common;
using ACE.Server.Physics.Sound;

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
        public List<int> ShadowObjects;
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
        public PhysicsObjHook Hooks;
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
        public int CollidingWithEnvironment;
        public int[] UpdateTimes;

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

        }

        public void AddPartToShadowCells(PhysicsPart part)
        {

        }

        public void AdjustPosition(Position pos, Vector3 low_pt, List<ObjCell> newCell, bool dontCreateCells, bool searchCells)
        {

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

        public int ForceIntoCell(ObjCell newCell, Position pos)
        {
            return -1;
        }

        public double GetAutonomyBlipDistance()
        {
            return -1;
        }

        public BoundingBox GetBoundingBox(BoundingBox boundingBox)
        {
            return null;
        }

        public int GetDataID()
        {
            return -1;
        }

        public double GetHeight()
        {
            return -1;
        }

        public double GetMaxConstraintDistance()
        {
            return -1;
        }

        public Object GetObjectA(int objectID)
        {
            return null;
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
            return null;
        }

        public int GetSetupID()
        {
            return -1;
        }

        public double GetStartConstraintDistance()
        {
            return -1;
        }

        public double GetStepDownHeight()
        {
            return -1;
        }

        public double GetStepUpHeight()
        {
            return -1;
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

                if (!State.HasFlag(PhysicsState.IgnoreCollisions))
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
                    PartArray.Setup = PartArray.CreateSetup(this, dataDID, createParts);
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

        public void MakeMovementManager(int init_motion)
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

        }

        public void SetLighting(float luminosity, float diffuse)
        {

        }

        public void SetLuminosity(float start, float end, double delta)
        {

        }

        public bool SetMotionTableID(int mtableID)
        {
            return false;
        }

        public void SetNoDraw(bool noDraw)
        {

        }

        public void SetObjectMaintainer(ObjectMaint objMaint)
        {

        }

        public void SetPartDiffusion(int part, float start, float end, double delta)
        {

        }

        public bool SetPartLighting(int partIdx, float luminosity, float diffuse)
        {
            return false;
        }

        public void SetPartLuminosity(int part, float start, float end, double delta)
        {

        }

        public void SetTextureVelocity(int partIdx, float du, float dv)
        {

        }

        public void SetPartTranslucency(int partIdx, float startTrans, float endTrans, double delta)
        {

        }

        public int SetPlacementFrame(int frameID, bool sendEvent)
        {
            return -1;
        }

        public int SetPlacementFrameInternal(int frameID)
        {
            return -1;
        }

        public static void SetPlayer(PhysicsObj newPlayer)
        {
            PlayerObject = newPlayer;
        }

        public int SetPosition(SetPosition setPos)
        {
            return -1;
        }

        public int SetPositionInternal(Transition transition)
        {
            return -1;
        }

        public int SetPositionInternal(Position pos, SetPosition setPos, Transition transition)
        {
            return -1;
        }

        public int SetPositionInternal(SetPosition setPos, Transition transition)
        {
            return -1;
        }

        public int SetPositionSimple(Position pos, int sliding)
        {
            return -1;
        }

        public void SetScale(float newScale, double delta)
        {

        }

        public void SetStaticScale(float newScale)
        {

        }

        public int SetScatterPositionInternal(SetPosition setPos, Transition transition)
        {
            return -1;
        }

        public void SetTextureVelocity(float du, float dv)
        {

        }

        public void SetTranslucency(float _translucency, double delta)
        {

        }

        public void SetTranslucency2(float startTrans, float endTrans, double delta)
        {

        }

        public void SetTranslucencyHierarchial(float _translucency)
        {

        }

        public void SetTranslucencyInternal(float _translucency)
        {

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

        }

        public void UpdateChildrenInternal()
        {

        }

        public void UpdateObjectInternal(float quantum)
        {

        }

        public void UpdatePartsInternal()
        {

        }

        public void UpdatePhysicsInternal(float quantum, AFrame frameOffset)
        {

        }

        public void UpdatePositionInternal(float quantum, AFrame newFrame)
        {

        }

        public void UpdateViewerDistance(float cypt, Vector3 heading)
        {

        }

        public void UpdateViewerDistance()
        {

        }

        public void UpdateViewerDistanceRecursive()
        {

        }

        public void add_anim_hook(AnimHook hook)
        {

        }

        public bool add_child(PhysicsObj obj, int where)
        {
            return false;
        }

        public bool add_child(PhysicsObj obj, int partIdx, AFrame frame)
        {
            return false;
        }

        public void add_obj_to_cell(ObjCell newCell, AFrame newFrame)
        {

        }

        public void add_particle_shadow_to_cell()
        {

        }

        public void add_shadows_to_cell(List<ObjCell> cellArray)
        {

        }

        public void add_voyeur(int objectID, float radius, float quantum)
        {

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

        }

        public void calc_cross_cells()
        {

        }

        public void calc_cross_cells_static()
        {

        }

        public void calc_friction(float quantum, float velocity_mag2)
        {

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

        public int check_contact(int contact)
        {
            return -1;
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

        public void find_bbox_cell_list(List<ObjCell> cellArray)
        {

        }

        public int get_curr_frame_number()
        {
            return -1;
        }

        public double get_distance_to_object(PhysicsObj obj, int use_cyls)
        {
            return -1;
        }

        public AFrame get_frame()
        {
            return null;
        }

        public double get_heading()
        {
            return -1;
        }

        public int get_landscape_coord(int x, int y)
        {
            return -1;
        }

        public void get_local_physics_velocity(ref Vector3 retval)
        {

        }

        public MotionInterp get_minterp()
        {
            return null;
        }

        public int get_num_emitters()
        {
            return -1;
        }

        public int get_object_info(Transition transition, int adminMove)
        {
            return -1;
        }

        public PositionManager get_position_manager()
        {
            return null;
        }

        public int get_sticky_object()
        {
            return -1;
        }

        public double get_target_quantum()
        {
            return -1;
        }

        public Vector3 get_velocity()
        {
            return Vector3.Zero;
        }

        public double get_walkable_z()
        {
            //return PhysicsGlobals.FloorZ;
            return -1;
        }

        public int handle_all_collisions(CollisionInfo collisions, int prev_has_contact, int prev_on_walkable)
        {
            return -1;
        }

        public bool is_completely_visible()
        {
            return true;
        }

        public bool is_newer(int timestamp, int newTime)
        {
            return true;
        }

        public bool is_valid_walkable(Vector3 normal)
        {
            return true;
        }

        public void leave_cell(bool is_changing_cell)
        {

        }

        public void leave_visibility()
        {

        }

        public void leave_world()
        {

        }

        public static bool makeAnimObject(int setupID, bool createParts)
        {
            return false;
        }

        public static PhysicsObj makeNullObject(int objectIID, bool dynamic)
        {
            return null;
        }

        public static PhysicsObj makeObject(PhysicsObj template)
        {
            return null;
        }

        public static PhysicsObj makeObject(int dataDID, int objectIID, bool dynamic)
        {
            return null;
        }

        public static PhysicsObj makeParticle(int numParts)
        {
            return null;
        }

        public bool motions_pending()
        {
            return false;
        }

        public bool movement_is_autonomous()
        {
            return false;
        }

        public bool newer_event(PhysicsTimeStamp stamp, int newTime)
        {
            return true;
        }

        public bool obj_within_block()
        {
            return true;
        }

        public bool on_ground()
        {
            return true;
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

        public int report_attacks(AttackInfo attackInfo)
        {
            return -1;
        }

        public void report_collision_end(bool forceEnd)
        {

        }

        public void report_collision_start()
        {

        }

        public int report_environment_collision(int prev_has_contact)
        {
            return -1;
        }

        public void report_exhaustion()
        {

        }

        public int report_object_collision(PhysicsObj obj, int prev_has_contact)
        {
            return -1;
        }

        public int report_object_collision_end(int objectID)
        {
            return -1;
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
            if (!State.HasFlag(PhysicsState.IgnoreCollisions) && PartArray != null)
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

            if (PartArray != null && !State.HasFlag(PhysicsState.IgnoreCollisions))
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
            if (PartArray != null && !State.HasFlag(PhysicsState.IgnoreCollisions))
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

        }

        public bool stop_particle_emitter(int emitterID)
        {
            return false;
        }

        public void store_position(Position pos)
        {

        }

        public void teleport_hook(int hide)
        {

        }

        public int track_object_collision(PhysicsObj obj, int prev_has_contact)
        {
            return -1;
        }

        public Transition transition(Position oldPos, Position newPos, bool adminMove)
        {
            return null;
        }

        public void unpack_movement(Object addr, int size)
        {

        }

        public void unparent_child()
        {

        }

        public void unset_parent()
        {

        }

        public void unstick_from_object()
        {

        }

        public void update_object()
        {

        }

        public void update_position()
        {

        }
    }
}
