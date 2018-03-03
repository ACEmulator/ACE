using System.Collections.Generic;
using System.Numerics;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Animation
{
    public class MotionInterp
    {
        public bool Initted;                // 0
        public WeenieObject WeenieObj;      // 4
        public PhysicsObj PhysicsObj;       // 8
        public RawMotionState RawState;     // 12
        public InterpretedMotionState InterpretedState;     // 68
        public float CurrentSpeedFactor;
        public int StandingLongJump;
        public float JumpExtent;
        public int ServerActionStamp;
        public float MyRunRate;
        public List<MotionNode> PendingMotions;

        public MotionInterp() { }

        public MotionInterp(PhysicsObj obj, WeenieObject wobj)
        {
            RawState = new RawMotionState();
            InterpretedState = new InterpretedMotionState();
            SetPhysicsObject(obj);
            SetWeenieObject(wobj);
        }

        public static MotionInterp Create(PhysicsObj obj, WeenieObject wobj)
        {
            var motionInterp = new MotionInterp(obj, wobj);
            return motionInterp;
        }

        public Sequence DoInterpretedMotion(int motion, MovementParameters movementParams)
        {
            return null;
        }

        public Sequence DoMotion(int motion, MovementParameters movementParams)
        {
            return null;
        }

        public void HandleExitWorld()
        {

        }

        public void HitGround()
        {

        }

        public int InqStyle()
        {
            return InterpretedState.CurrentStyle;
        }

        public void LeaveGround()
        {

        }

        public void MotionDone(bool success)
        {

        }

        public int PerformMovement(MovementStruct mvs)
        {
            return -1;
        }

        public void ReportExhaustion()
        {

        }

        public void SetHoldKey(int holdKey, bool cancelMoveTo)
        {

        }

        public void SetPhysicsObject(PhysicsObj obj)
        {

        }

        public void SetWeenieObject(WeenieObject wobj)
        {

        }

        public bool StopCompletely()
        {
            return false;
        }

        public Sequence StopInterpretedMotion(int motion, MovementParameters movementParams)
        {
            return null;
        }

        public Sequence StopMotion(int motion, MovementParameters movementParams)
        {
            return null;
        }

        public void add_to_queue(int contextID, int motion, int jumpErrorCode)
        {

        }

        public void adjust_motion(int motion, float speed, int holdKey)
        {

        }

        public void apply_current_movement(bool cancelMoveTo, bool allowJump)
        {

        }

        public void apply_interpreted_movement(bool cancelMoveTo, bool allowJump)
        {

        }

        public void apply_raw_movement(bool cancelMoveTo, bool allowJump)
        {

        }

        public void apply_run_to_command(int motion, float speed)
        {

        }

        public int charge_jump()
        {
            return -1;
        }

        public int contact_allows_move(int motion)
        {
            return -1;
        }

        public void enter_default_state()
        {

        }

        public double get_adjusted_max_speed()
        {
            return -1;
        }

        public double get_jump_v_z()
        {
            return -1;
        }

        public Vector3 get_leave_ground_velocity()
        {
            return Vector3.Zero;
        }

        public double get_max_speed()
        {
            return -1;
        }

        public Vector3 get_state_velocity()
        {
            return Vector3.Zero;
        }

        public bool is_standing_still()
        {
            return false;
        }

        public int jump(float extent, int adjustStamina)
        {
            return -1;
        }

        public bool jump_charge_is_allowed()
        {
            return false;
        }

        public bool jump_is_allowed(float extent, int staminaCost)
        {
            return false;
        }

        public bool motion_allows_jump(int substate)
        {
            return false;
        }

        public bool motions_pending()
        {
            return false;
        }

        public bool move_to_interpreted_state(InterpretedMotionState state)
        {
            return false;
        }

        public void set_hold_run(int val, bool cancelMoveTo)
        {

        }
    }
}
